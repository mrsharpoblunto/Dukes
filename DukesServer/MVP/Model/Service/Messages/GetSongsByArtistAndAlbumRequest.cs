using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class GetSongsByArtistAndAlbumRequest : BaseRequest
    {
        private string _artist, _album;

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
    }
}