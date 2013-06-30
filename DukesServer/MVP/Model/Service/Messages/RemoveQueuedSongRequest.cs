using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class RemoveQueuedSongRequest : BaseRequest
    {
        Guid _songId;

        [DataMember]
        public Guid SongId
        {
            get { return _songId; }
            set { _songId = value; }
        }

    }
}