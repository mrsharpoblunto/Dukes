using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class LoginResponse : BaseResponse
    {
        private Guid _tokenId;

        [DataMember]
        public Guid TokenId
        {
            get { return _tokenId; }
            set { _tokenId = value; }
        }
    }
}