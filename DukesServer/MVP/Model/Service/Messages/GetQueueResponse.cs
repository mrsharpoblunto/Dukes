using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using DukesServer.MVP.Model.Service.Entities;
using System.Collections.Generic;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class GetQueueResponse : BaseResponse
    {
        private List<Entities.QueuedSong> _queuedSongs = new List<Entities.QueuedSong>();

        [DataMember]
        public List<Entities.QueuedSong> QueuedSongs
        {
            get { return _queuedSongs; }
            set { _queuedSongs = value; }
        }
    }
}