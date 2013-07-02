using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DukesServer.MVP.Model;
using DukesServer.MVP.Model.Backup;
using DukesServer.MVP.Model.HttpServer;
using DukesServer.MVP.Model.Service;
using DukesServer.MVP.Model.Service.Entities;
using DukesServer.MVP.View;
using QueuedSong=DukesServer.MVP.Model.QueuedSong;

namespace DukesServer.MVP.Presenter
{
    internal class DukesServerPresenter: PresenterBase<IDukesServerView>
    {
        private readonly System.Windows.Forms.Timer _playerUpdateTimer;
        private readonly object _playerUpdatelock = new object();
        private const int PlayerUpdateLockTimeout = 10000;

        private readonly System.Windows.Forms.Timer _onIndexTimer;
        private bool _indexing = false;

        private readonly WebUIHttpServer _webUIServer;
        private List<IDatabaseBackup> _backups;
        private Settings _settings;
        private bool _closing = false;
        private bool _restart = false;

        public DukesServerPresenter(IDukesServerView view) : base(view)
        {
            // if we don't have any database file, create a default one.
            if (!File.Exists(Constants.DatabaseFile))
            {
                RestoreBackup(new DefaultDatabaseBackup());
            }

            // load or configure the default settings
            _settings = Settings.Load();
            if (_settings==null)
            {
                _settings = new Settings { Port = 8086, RestoreDB = false };
            }

            if (_settings.BackupDB)
            {
                // make sure the old instance is shutdown
                Thread.Sleep(1000);
                DiskDatabaseBackup.CreateDiskBackup();
                _settings.BackupDB = false;
                _settings.Save();
                MessageBox.Show("Database backed up");
            }

            _backups = DiskDatabaseBackup.LoadDiskBackups();
            _backups.Add(new DefaultDatabaseBackup());

            if (_settings.RestoreDB) 
            {
                // make sure the old instance is shutdown
                Thread.Sleep(1000);
                if (!string.IsNullOrEmpty(_settings.RestoreBackup))
                {
                    File.Delete(Constants.DatabaseFile);
                    IDatabaseBackup backup = _backups.Where(db => db.Name == _settings.RestoreBackup).First();
                    RestoreBackup(backup);
                    MessageBox.Show("Database restored to " + _settings.RestoreBackup);
                }
                else
                {
                    RestoreBackup(new DefaultDatabaseBackup());
                }

                _settings.RestoreDB = false;
                _settings.RestoreBackup = string.Empty;
                _settings.Save();
            }

            // hook up event handlers and populate the UI
            view.OnAddUser += ViewOnAddUser;
            view.OnRemoveUser += ViewOnRemoveUser;
            view.OnAddMediaSource += ViewOnAddMediaSource;
            view.OnRemoveMediaSource += ViewOnRemoveMediaSource;
            view.OnIndexMediaSources += ViewOnIndexMediaSources;
            view.OnRestoreDatabase += ViewOnRestoreDatabase;
            view.OnBackupDatabase += ViewOnBackupDatabase;
            view.OnPause += (s, e) => Player.Current.Pause();
            view.OnPlay += (s, e) => Player.Current.Resume();
            view.OnChangePort += ViewOnChangePort;
            view.OnBeforeClose += ViewOnClosing;
            view.DatabaseBackups = _backups;
            view.Shown += ViewOnShown;
            try
            {
                view.Users = Database.GetUsers();
                view.MediaDirectories = Database.GetRootDirectories();
                view.PlayerQueue = Database.GetQueue();
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex, "There was a problem reading the dukes database");
                if (MessageBox.Show("There was a problem reading the dukes database, do you want to restore the default dukes database?","Dukes database unreadable",MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    _settings.RestoreDB = true;
                    _settings.RestoreBackup = string.Empty;
                    _settings.Save();
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                }
                View.ForceClose = true;
                View.CloseView();
                return;
            }
            Database.OnQueueChanged += (s, e) => view.Invoke(() => view.PlayerQueue = Database.GetQueue());
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            var ip = (
                       from addr in hostEntry.AddressList
                       where addr.AddressFamily.ToString() == "InterNetwork"
                       select addr.ToString()
                ).FirstOrDefault();
            view.ServiceUrl = "http://" + ip;
            view.PlayerState = Player.Current.State;
            view.Port = _settings.Port;

            // start up the web UI http server
#if DEBUG
            _webUIServer = new WebUIHttpServer(false);
#else
            _webUIServer = new WebUIHttpServer(true);
#endif
            _webUIServer.Port = _settings.Port;
            _webUIServer.OnListenError += (s, e) =>
            {
                MessageBox.Show("Dukes was unable to set up an HTTP Listener on port " + _settings.Port + " another program may be using it. Please try a different port");
            };
            _webUIServer.OnError += (s, e) =>
            {
                view.Invoke(() => view.PlayerUpdate(new PlayerMessageArgs() { Message = "WebUI server encountered an error and will restart" }));
            };
            _webUIServer.Start();

            // hook up the player events
            Player.Current.OnPaused += (s, e) => view.Invoke(() => view.PlayerState = Player.Current.State);
            Player.Current.OnPlaying += (s, e) => view.Invoke(() => view.PlayerState = Player.Current.State);
            Player.Current.OnStopped += (s, e) => view.Invoke(() => view.PlayerState = Player.Current.State);
            Player.Current.OnPlayNewSong += (s, e) => view.Invoke(() => view.CurrentSong = e.Song);
            Player.Current.OnPlayerMessage += (s, e) => view.Invoke(() =>
            {
                Logger.Current.Write(LogInfoLevel.Info,e.Message);
                view.PlayerUpdate(e);
            });

            _playerUpdateTimer = new System.Windows.Forms.Timer {Interval = 1000};
            _playerUpdateTimer.Tick += OnPlayerUpdate;
            _playerUpdateTimer.Start();

            _onIndexTimer = new System.Windows.Forms.Timer {Interval = 3600000};
            _onIndexTimer.Tick += OnIndex;
            _onIndexTimer.Start();
        }

        private void ViewOnShown(object state, EventArgs e)
        {
        }

        private void ViewOnClosing(object state, CancelEventArgs e)
        {
            if (_indexing)
            {
                MessageBox.Show("Please wait for the current indexing operation to complete before exiting");
                e.Cancel = true;
                return;
            }
            else
            {
                _closing = true;
                if (_onIndexTimer!=null) _onIndexTimer.Stop();
            }

            if (_playerUpdateTimer != null) _playerUpdateTimer.Stop();
            if (Monitor.TryEnter(_playerUpdatelock, PlayerUpdateLockTimeout))
            {
                Monitor.Exit(_playerUpdatelock);
            }
            else
            {
                Logger.Current.Write(LogInfoLevel.Error, "Player update thread did not complete in a timely fashion");
            }

            if (_webUIServer != null) _webUIServer.Stop();

            if (_restart)
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath);
            }
        }

        private void ViewOnChangePort(object state, EventArgs e)
        {
            try
            {
                _settings.Port = View.Port;
                _settings.Save();
                _webUIServer.Stop();
                _webUIServer.Port = _settings.Port;
                _webUIServer.Start();
            }
            catch (Exception)
            {
                View.Port = _settings.Port;
            }
        }

        private void OnPlayerUpdate(object state, EventArgs e)
        {
            if (_closing) return;

            ThreadPool.QueueUserWorkItem(obj =>
            {
                if (Monitor.TryEnter(_playerUpdatelock))
                {
                    try
                    {
                        if (ServiceLocator.Get<IPlayer>().State == PlayerState.Stopped)
                        {
                            QueuedSong song = Database.GetNextSong();
                            if (song != null)
                            {
                                ServiceLocator.Get<IPlayer>().CurrentSong = Mapper.MapFromDomain(song);
                                View.Invoke(() => ServiceLocator.Get<IPlayer>().Play());
                                Database.RemoveFromMyQueue(song.UserId, song.SongId);
                            }
                            else
                            {
                                ServiceLocator.Get<IPlayer>().CurrentSong = null;
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(_playerUpdatelock);
                    }
                }
            });
        }

        void ViewOnBackupDatabase(object sender, EventArgs e)
        {
            if (_indexing)
            {
                MessageBox.Show("Please wait for the current indexing operation to complete before backing up the database");
                return;
            }
            else
            {
                _settings.BackupDB = true;
                _settings.Save();
                _restart = true;
                View.ForceClose = true;
                View.CloseView();
            }
        }

        void ViewOnRestoreDatabase(object sender, RestoreDatabaseEventArgs e)
        {
            if (MessageBox.Show("Restoring the database will replace the current user and media source information.\r\nDo you want to do this?", "Do you want to restore?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (_indexing)
            {
                MessageBox.Show("Please wait for the current indexing operation to complete before restoring the database");
                return;
            }
            else
            {
                _settings.RestoreDB = true;
                _settings.RestoreBackup = e.Name;
                _settings.Save();
                _restart = true;
                View.ForceClose = true;
                View.CloseView();
            }
        }

        private static void RestoreBackup(IDatabaseBackup backup)
        {
            using (Stream input = backup.Data)
            {
                using (Stream output = new FileStream(Constants.DatabaseFile, FileMode.Create))
                {
                    input.CopyTo(output);
                }
            }
        }

        void ViewOnIndexMediaSources(object sender, EventArgs e)
        {
            if (!_indexing)
            {
                if (_closing) return;
                _indexing = true;
                ThreadPool.QueueUserWorkItem(DoIndex);
            }
        }

        private void OnIndex(object state, EventArgs e)
        {
            if (!_indexing)
            {
                if (_closing) return;
                _indexing = true;
                ThreadPool.QueueUserWorkItem(DoIndex);
            }
        }

        private void DoIndex(object state)
        {
            try
            {
                View.Invoke(() =>View.IndexEnabled = false);
                Database.IndexDirectories(IndexingProgress);
                View.Invoke(() =>View.IndexEnabled = true);
            }
            finally
            {
                View.Invoke(() => _indexing = false);
            }
        }

        void ViewOnRemoveMediaSource(object sender, EventArgs e)
        {
            if (_indexing)
            {
                MessageBox.Show("Please wait for the current indexing operation to complete before removing this media source");
                return;
            }
            else
            {
                var directory = View.SelectedMediaDirectory;
                if (directory == null) return;

                View.IndexEnabled = false;
                _indexing = true;

                ThreadPool.QueueUserWorkItem(selectedDirectory =>
                {
                    try
                    {
                        Database.RemoveRootDirectory((Model.Directory)selectedDirectory, IndexingProgress);
                        View.Invoke(() => 
                        {
                            View.IndexEnabled = true;
                            View.MediaDirectories = Database.GetRootDirectories();
                        });
                    }
                    finally
                    {
                        View.Invoke(() => _indexing = false);
                    }
                }, directory);
            }
        }

        private void IndexingProgress(object sender, IndexingProgressEventArgs e)
        {
            View.Invoke(() => View.MediaIndexUpdate(e));
        }

        void ViewOnAddMediaSource(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(View.MediaSource))
            {
                try
                {
                    Database.AddRootDirectory(View.MediaSource);
                    View.MediaDirectories = Database.GetRootDirectories();
                }
                catch (Exception ex)
                {
                    Logger.Current.Write(ex, "This doesn't appear to a valid media source");
                    View.ShowError("This doesn't appear to a valid media source");
                }
            }
        }

        void ViewOnRemoveUser(object sender, EventArgs e)
        {
            if (View.SelectedUser!=null)
            {
                Database.RemoveUser(View.SelectedUser);
                View.Users = Database.GetUsers();
            }
        }

        void ViewOnAddUser(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(View.Username) && !string.IsNullOrEmpty(View.Password))
            {
                Database.AddUser(View.Username, View.Password);
                View.Users = Database.GetUsers();
                View.Username = string.Empty;
                View.Password = string.Empty;
            }
        }
    }
}