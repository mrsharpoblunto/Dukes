using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using DukesServer.MVP.Model.Service.Entities;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class GetSongsResponse : BaseResponse
    {
        private List<Entities.Song> _songs = new List<Entities.Song>();

        [DataMember]
        public List<Entities.Song> Songs
        {
            get { return _songs; }
            set { _songs = value; }
        }
    }
}