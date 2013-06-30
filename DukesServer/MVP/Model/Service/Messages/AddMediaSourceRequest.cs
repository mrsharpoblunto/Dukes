using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class AddMediaSourceRequest : BaseRequest
    {
        private string _sourceFolderPath;

        public string SourceFolderPath
        {
            get { return _sourceFolderPath; }
            set { _sourceFolderPath = value; }
        }
    }
}