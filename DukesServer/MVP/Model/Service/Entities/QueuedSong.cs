using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Entities
{
    [DataContract]
    public class QueuedSong
    {
        private Entities.Song _song;
        private string _submitter;

        [DataMember]
        public Entities.Song Song
        {
            get { return _song; }
            set { _song = value; }
        }

        [DataMember]
        public string Submitter
        {
            get { return _submitter; }
            set { _submitter = value; }
        }
    }
}