using System.Runtime.Serialization;
using DukesServer.MVP.Model.Service.Entities;

namespace DukesServer.MVP.Model.Service.Messages
{
    [DataContract]
    public class PlayerStateResponse: BaseResponse
    {
        [DataMember]
        public PlayerState PlayerState { get; set; }
    }
}