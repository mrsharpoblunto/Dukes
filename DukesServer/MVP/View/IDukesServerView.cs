using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DukesServer.MVP.Model;
using DukesServer.MVP.Model.Backup;
using DukesServer.MVP.Model.Service.Entities;
using QueuedSong = DukesServer.MVP.Model.QueuedSong;

namespace DukesServer.MVP.View
{
    internal class RestoreDatabaseEventArgs : EventArgs
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    internal interface IDukesServerView : IView
    {
        event EventHandler OnAddUser;
        event EventHandler OnRemoveUser;
        event EventHandler OnAddMediaSource;
        event EventHandler OnRemoveMediaSource;
        event EventHandler OnIndexMediaSources;
        event EventHandler<RestoreDatabaseEventArgs> OnRestoreDatabase;
        event EventHandler OnBackupDatabase;
        event EventHandler OnChangePort;
        event EventHandler<CancelEventArgs> OnBeforeClose;
        event EventHandler OnPause;
        event EventHandler OnPlay;

        void ShowError(string error);

        string ServiceUrl { set; }
        string Username { get; set; }
        string Password { get; set; }
        List<Model.User> Users { set; }
        Model.User SelectedUser { get; }
        Model.Service.Entities.QueuedSong CurrentSong { set; }
        PlayerState PlayerState { set; }
        List<QueuedSong> PlayerQueue { set; }
        bool ForceClose { get; set; }
        string MediaSource { get; set; }
        List<Directory> MediaDirectories { set; }
        List<IDatabaseBackup> DatabaseBackups { set; }
        Directory SelectedMediaDirectory { get; }
        int Port { get; set; }
        bool IndexEnabled { get; set; }
        void MediaIndexUpdate(IndexingProgressEventArgs e);
        void PlayerUpdate(PlayerMessageArgs e);
    }
}