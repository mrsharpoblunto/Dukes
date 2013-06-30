using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DukesServer.MVP.Model.Service.Entities;

namespace DukesServer
{
    public class PlayerMessageArgs: EventArgs
    {
        public string Message { get; set; }
    }

    public class PlayerSongArgs : EventArgs
    {
        public QueuedSong Song { get; set; }
    }

    public interface IPlayer: IDisposable
    {
        event EventHandler OnPlaying;
        event EventHandler OnPaused;
        event EventHandler OnStopped;
        event EventHandler<PlayerMessageArgs> OnPlayerMessage;
        event EventHandler<PlayerSongArgs> OnPlayNewSong;

        void Play();
        void Resume();
        void Pause();

        PlayerState State { get; }
		QueuedSong CurrentSong { get; set; }
    }
}