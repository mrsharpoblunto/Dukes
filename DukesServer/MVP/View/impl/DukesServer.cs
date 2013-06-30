using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DukesServer.MVP.Model;
using DukesServer.MVP.Model.Backup;
using DukesServer.MVP.Model.Service.Entities;
using DukesServer.MVP.View;
using QueuedSong=DukesServer.MVP.Model.QueuedSong;

namespace DukesServer.MVP.View.impl
{
    internal partial class DukesServerForm : ViewBase, IDukesServerView
    {
        public event EventHandler OnAddUser;
        public event EventHandler OnRemoveUser;
        public event EventHandler OnAddMediaSource;
        public event EventHandler OnRemoveMediaSource;
        public event EventHandler OnIndexMediaSources;
        public event EventHandler<RestoreDatabaseEventArgs> OnRestoreDatabase;
        public event EventHandler OnBackupDatabase;
        public event EventHandler OnPause;
        public event EventHandler OnPlay;
        public event EventHandler OnChangePort;
        public event EventHandler<CancelEventArgs> OnBeforeClose;

        private bool _forceClose = false;

        public DukesServerForm()
        {
            InitializeComponent();
            Closing += DukesServerFormClosing;
        }

        void DukesServerFormClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !ForceClose;
            if (e.Cancel)
            {
                Hide();
            }
            else
            {
                if (OnBeforeClose != null)
                {
                    OnBeforeClose(this, e);
                }
            }
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error);
        }

        public int Port
        {
            set { portTextBox.Text = value.ToString(); }
            get { return int.Parse(portTextBox.Text); }
        }

        public string ServiceUrl
        {
            set { serviceURLTextBox.Text = value; }
        }

        public string Username
        {
            get { return userNameTextBox.Text; }
            set { userNameTextBox.Text = value; }
        }

        public string Password
        {
            get { return passwordTextBox.Text; }
            set { passwordTextBox.Text = value; }
        }

        public List<Model.User> Users
        {
            set
            {
                UsersList.DisplayMember = "Name";
                UsersList.DataSource = value;
            }
        }

        public Model.User SelectedUser
        {
            get
            {
                if (UsersList.SelectedItem!=null)
                {
                    return (Model.User)UsersList.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public Model.Service.Entities.QueuedSong CurrentSong
        {
            set
            {
                if (value != null)
                {
                    currentlyPlayingTextBox.Text = value.Song.Title;
                }
                else
                {
                    currentlyPlayingTextBox.Text = string.Empty;
                }
            }
        }

        public PlayerState PlayerState
        {
            set { playerState.Text = value.ToString(); }
        }

        public List<QueuedSong> PlayerQueue
        {
            set
            {
                PlayerQueueList.Items.Clear();
                foreach (var song in value)
                {
                    PlayerQueueList.Items.Add(song.Song.Title + " Requested by " + song.User.Name);
                }
            }
        }

        public string MediaSource
        {
            get { return mediaSourceTextBox.Text; }
            set { mediaSourceTextBox.Text = value; }
        }

        public List<Directory> MediaDirectories
        {
            set
            {
                mediaSourcesList.DataSource = null;
                mediaSourcesList.DisplayMember = "Path";
                mediaSourcesList.DataSource = value;
            }
        }

        public List<IDatabaseBackup> DatabaseBackups
        {
            set
            {
                restoreDatabaseToolStripMenuItem.DropDownItems.Clear();
                foreach (IDatabaseBackup backup in value)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(backup.Name);
                    IDatabaseBackup b = backup;
                    item.Click += (sender, args) => 
                                      {
                                          if (OnRestoreDatabase!=null)
                                          {
                                              OnRestoreDatabase(this,new RestoreDatabaseEventArgs{Name = b.Name});
                                          }
                                      };
                    restoreDatabaseToolStripMenuItem.DropDownItems.Add(item);
                }
            }
        }

        public Directory SelectedMediaDirectory
        {
            get
            {
                if (mediaSourcesList.SelectedItem != null)
                {
                    return (Directory)mediaSourcesList.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IndexEnabled
        {
            get { return IndexButton.Enabled; }
            set
            {
                removeMediaSource.Enabled = value;
                addMediaSourceButton.Enabled = value;
                IndexButton.Enabled = value;
                if (!value)
                {
                    mediaIndexNotifications.Clear();
                }
            }
        }

        public bool ForceClose
        {
            get { return _forceClose; }
            set { _forceClose = value; }
        }

        public void MediaIndexUpdate(IndexingProgressEventArgs e)
        {
            mediaIndexNotifications.AppendText(e.Message + "\r\n");
        }

        public void PlayerUpdate(PlayerMessageArgs e)
        {
            playerNotificationsTextBox.AppendText(e.Message + "\r\n");
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            if (OnAddUser!=null)
            {
                OnAddUser(this,new EventArgs());
            }
        }

        private void removeUserButton_Click(object sender, EventArgs e)
        {
            if (OnRemoveUser != null)
            {
                OnRemoveUser(this, new EventArgs());
            }
        }

        private void addMediaSourceButton_Click(object sender, EventArgs e)
        {
            if (OnAddMediaSource!=null)
            {
                OnAddMediaSource(this,new EventArgs());
            }
        }

        private void removeMediaSource_Click(object sender, EventArgs e)
        {
            if (OnRemoveMediaSource != null)
            {
                OnRemoveMediaSource(this, new EventArgs());
            }
        }

        private void indexButton_Click(object sender, EventArgs e)
        {
            if (OnIndexMediaSources!=null)
            {
                OnIndexMediaSources(this,new EventArgs());
            }
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnBackupDatabase!=null)
            {
                OnBackupDatabase(this,new EventArgs());
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (OnPause != null)
            {
                OnPause(this, new EventArgs());
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (OnPlay != null)
            {
                OnPlay(this, new EventArgs());
            }
        }

        private void changePortButton_Click(object sender, EventArgs e)
        {
            if (OnChangePort != null)
            {
                OnChangePort(this, new EventArgs());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ForceClose = true;
            Close();
        }
    }
}