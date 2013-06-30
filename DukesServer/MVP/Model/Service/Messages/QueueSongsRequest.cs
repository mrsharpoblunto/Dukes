using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class QueueSongsRequest : BaseRequest
    {
        private List<Guid> _songIds = new List<Guid>();

        [DataMember]
        public List<Guid> SongIds
        {
            get { return _songIds; }
            set { _songIds = value; }
        }
    }
}