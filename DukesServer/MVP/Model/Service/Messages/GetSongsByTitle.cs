using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class GetSongsByTitle : BaseRequest
    {
        private string _title;

        [DataMember]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}