using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DukesServer.MVP.Model.Service.Entities
{
    [DataContract]
    public enum PlayerState
    {
        [EnumMember]
        Stopped,
        [EnumMember]
        Paused,
        [EnumMember]
        Playing
    }

    [DataContract]
    public enum SetPlayerState
    {
        [EnumMember]
        Play,
        [EnumMember]
        Pause
    }
}
