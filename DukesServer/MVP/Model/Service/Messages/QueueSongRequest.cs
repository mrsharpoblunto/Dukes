using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class QueueSongRequest : BaseRequest
    {
        Guid _songId;
        int _sortOrdinal;

        [DataMember]
        public Guid SongId
        {
            get { return _songId; }
            set { _songId = value; }
        }

        [DataMember]
        public int SortOrdinal
        {
            get { return _sortOrdinal; }
            set { _sortOrdinal = value; }
        }
    }
}