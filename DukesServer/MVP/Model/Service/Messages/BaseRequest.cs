using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class BaseRequest
    {
        private Guid _userToken;

        [DataMember]
        public Guid UserToken
        {
            get { return _userToken; }
            set { _userToken = value; }
        }	
    }
}