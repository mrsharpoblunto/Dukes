using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class BaseResponse
    {
        private bool _success = true;
        private string _errorMessage;

		
        [DataMember]
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { 
                _errorMessage = value;
                if (!String.IsNullOrEmpty(value))
                {
                    _success = false;
                }
            }
        }
    }
}