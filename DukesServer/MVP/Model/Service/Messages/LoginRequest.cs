using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class LoginRequest
    {
        private string _username, _password;

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [DataMember]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
    }
}