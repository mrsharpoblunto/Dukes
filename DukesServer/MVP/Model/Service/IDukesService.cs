using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using DukesServer.MVP.Model.Service.Entities;
using DukesServer.MVP.Model.Service.Messages;

namespace DukesServer.MVP.Model.Service
{
    [ServiceContract]
    [ServiceKnownType(typeof(PlayerState))]
    [ServiceKnownType(typeof(SetPlayerState))]
    public interface IDukesService
    {

        [OperationContract]
        LoginResponse Login(LoginRequest request);

        [OperationContract]
        GetSongsResponse GetSongsByArtistAndAlbum(GetSongsByArtistAndAlbumRequest request);
		
        [OperationContract]
        GetSongsResponse GetSongsByTitle(GetSongsByTitle request);

        [OperationContract]
        GetQueueResponse GetMyQueue(BaseRequest request);

        [OperationContract]
        GetQueueResponse GetQueue(BaseRequest request);

        [OperationContract]
        GetCurrentSongResponse GetCurrentSong(BaseRequest request);

        [OperationContract]
        BaseResponse RemoveFromMyQueue(RemoveQueuedSongRequest request);

        [OperationContract]
        BaseResponse ClearMyQueue(BaseRequest request);

        [OperationContract]
        BaseResponse AddToMyQueue(QueueSongsRequest request);

        [OperationContract]
        PlayerStateResponse GetPlayerState(BaseRequest request);

        [OperationContract]
        PlayerStateResponse SetPlayerState(PlayerStateRequest request);

        [OperationContract]
        UserResponse GetUser(BaseRequest request);
    }
}