using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class UserResponse: BaseResponse
    {
        private Entities.User _user;

        [DataMember]
        public Entities.User User
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
