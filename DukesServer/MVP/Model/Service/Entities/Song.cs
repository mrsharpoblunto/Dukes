using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Entities
{
    [DataContract]
    public class Song
    {
        private Guid _id;
        private string _title, _artist, _album;
        private string _fileName;

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        [DataMember]
        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        [DataMember]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [DataMember]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


    }
}