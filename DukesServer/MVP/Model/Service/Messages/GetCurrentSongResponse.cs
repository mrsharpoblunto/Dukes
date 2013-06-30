using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using DukesServer.MVP.Model.Service.Entities;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class GetCurrentSongResponse : BaseResponse
    {
        private Entities.QueuedSong _currentSong;

        [DataMember]
        public Entities.QueuedSong CurrentSong
        {
            get { return _currentSong; }
            set { _currentSong = value; }
        }
    }
}