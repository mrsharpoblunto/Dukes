using System.Runtime.Serialization;
using DukesServer.MVP.Model.Service.Entities;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class PlayerStateRequest: BaseRequest
    {
        [DataMember]
        public SetPlayerState SetPlayerState { get; set; }
    }
}