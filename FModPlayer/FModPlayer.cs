using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DukesServer;
using DukesServer.MVP.Model.Service.Entities;
using FMOD;

namespace FModPlayer
{
    public class FModPlayer:IPlayer
    {
        private readonly FMOD.System _system;
        private Sound _sound;
        private Channel _channel;
        private readonly Thread _playerThread;
        private readonly object _lock = new object();
        private uint _pausePosition;
        public event EventHandler OnPlaying;
        public event EventHandler OnPaused;
        public event EventHandler OnStopped;
        public event EventHandler<PlayerMessageArgs> OnPlayerMessage;
        public event EventHandler<PlayerSongArgs> OnPlayNewSong;

        public FModPlayer()
        {
            uint version = 0;

            RESULT result = Factory.System_Create(ref _system);
            if (ERRCHECK(result))
            {
                result = _system.getVersion(ref version);
                if (ERRCHECK(result))
                {
                    if (version < VERSION.number)
                    {
                        OnPlayerMessage(this, new PlayerMessageArgs { Message = "You are using an old version of FMOD " + version.ToString("X") + ".  This program requires " + VERSION.number.ToString("X") + "." });
                    }

                    result = _system.init(1, INITFLAGS.NORMAL, (IntPtr)null);
                    if (ERRCHECK(result))
                    {
                        _playerThread = new Thread(StartPlayer);
                        _playerThread.Start();
                    }
                }
            }
        }

        public void Dispose()
        {
            _playerThread.Abort();
            _playerThread.Join();

            RESULT result;
            if (_sound != null)
            {
                result = _sound.release();
                ERRCHECK(result);
            }
            if (_system != null)
            {
                result = _system.close();
                ERRCHECK(result);
                result = _system.release();
                ERRCHECK(result);
            }
        }


        private void StartPlayer()
        {
            while (true)
            {
                lock (_lock) 
                {
                    if (_channel != null)
                    {
                        bool playing = false;
                        RESULT result = _channel.isPlaying(ref playing);
                        if ((result != RESULT.OK) && (result != RESULT.ERR_INVALID_HANDLE))
                        {
                            if (!ERRCHECK(result))
                            {
                                Stop();
                            }
                        }

                        if (!playing && State == PlayerState.Playing)
                        {
                            Stop();
                        }
                    }

                    if (_system != null)
                    {
                        _system.update();
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void Stop()
        {
            if (CurrentSong == null) return;
            if (_sound != null)
            {
                RESULT result = _sound.release();
                ERRCHECK(result);
                _sound = null;
                _channel = null;
            }
            State = PlayerState.Stopped;
            if (OnStopped != null)
            {
                OnStopped(this, new EventArgs());
            }
        }

        public void Play()
        {
            lock (_lock)
            {
                bool playing = _channel != null;
                if (playing)
                {
                    RESULT result = _channel.isPlaying(ref playing);
                    if ((result != RESULT.OK) && (result != RESULT.ERR_INVALID_HANDLE))
                    {
                        if (!ERRCHECK(result))
                        {
                            Stop();
                        }
                    }
                }

                if (!playing && CurrentSong != null && CurrentSong.Song!=null)
                {
                    if (!File.Exists(CurrentSong.Song.FileName))
                    {
                        if (OnPlayerMessage != null)
                        {
                            OnPlayerMessage(this, new PlayerMessageArgs { Message = "Can't find " + CurrentSong.Song.Title + " Requested by " + CurrentSong.Submitter+" Skipping..." });
                        }
                    }
                    else
                    {
                        RESULT result = _system.createSound(CurrentSong.Song.FileName, (MODE._2D | MODE.HARDWARE | MODE.CREATESTREAM), ref _sound);
                        if (ERRCHECK(result))
                        {

                            result = _system.playSound(CHANNELINDEX.FREE, _sound, false, ref _channel);
                            if (ERRCHECK(result))
                            {
                                State = PlayerState.Playing;
                                if (OnPlayerMessage != null)
                                {
                                    OnPlayerMessage(this, new PlayerMessageArgs { Message = "Playing " + CurrentSong.Song.Title + " Requested by " + CurrentSong.Submitter });
                                }
                                if (OnPlayNewSong != null)
                                {
                                    OnPlayNewSong(this, new PlayerSongArgs { Song = CurrentSong });
                                }
                                if (OnPlaying != null)
                                {
                                    OnPlaying(this, new EventArgs());
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (CurrentSong == null) return;
                if (_sound == null)
                {
                    RESULT result = _system.createSound(CurrentSong.Song.FileName, (MODE._2D | MODE.HARDWARE | MODE.CREATESTREAM), ref _sound);
                    if (ERRCHECK(result))
                    {
                        result = _system.playSound(CHANNELINDEX.FREE, _sound, true, ref _channel);

                        if (ERRCHECK(result))
                        {
                            _channel.setPosition(_pausePosition, TIMEUNIT.MS);
                            ERRCHECK(result);
                            result = _channel.setPaused(false);
                            if (ERRCHECK(result))
                            {
                                State = PlayerState.Playing;
                                if (OnPlaying != null)
                                {
                                    OnPlaying(this, new EventArgs());
                                }
                            }
                        }
                    }
                }
            }
        }


        public void Pause()
        {
            lock (_lock)
            {
                if (CurrentSong == null) return;
                if (_sound != null)
                {
                    RESULT result = _channel.getPosition(ref _pausePosition,TIMEUNIT.MS);
                    ERRCHECK(result);

                    result = _sound.release();
                    ERRCHECK(result);

                    _sound = null;
                    _channel = null;    
          
                    State = PlayerState.Paused;
                    if (OnPaused != null)
                    {
                        OnPaused(this, new EventArgs());
                    }
                }
            }
        }

        public PlayerState State
        {   get;
            private set;
        }

        private QueuedSong _currentSong;
        public QueuedSong CurrentSong
        {
            get { return _currentSong; }
            set { lock (_lock) {_currentSong = value;} }
        }

        private bool ERRCHECK(RESULT result)
        {
            if (result != RESULT.OK)
            {
                if (OnPlayerMessage != null)
                {
                    OnPlayerMessage(this, new PlayerMessageArgs { Message = "FMOD error! " + result + " - " + Error.String(result) });
                }
                return false;
            }
            return true;
        }
    }
}
