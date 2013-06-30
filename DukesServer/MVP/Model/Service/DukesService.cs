using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DukesServer.MVP.Model.Service.Messages;

namespace DukesServer.MVP.Model.Service
{
    public class DukesService : IDukesService
    {
        #region IDukesService Members

        /// <summary>
        /// Check login credentials
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LoginResponse Login(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                User user = Database.GetUser(request.Username, request.Password);
                response.TokenId = user.Id;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Find all songs matching the specified album and artist.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetSongsResponse GetSongsByArtistAndAlbum(GetSongsByArtistAndAlbumRequest request)
        {
            GetSongsResponse response = new GetSongsResponse();

            try
            {
                AssertLoginRights(request);
                IList<Song> songs = Database.GetSongs(request.Artist, request.Album);
                response.Songs = Mapper.MapFromDomain(songs);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Find all songs matching the specified title.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetSongsResponse GetSongsByTitle(GetSongsByTitle request)
        {
            GetSongsResponse response = new GetSongsResponse();

            try
            {
                AssertLoginRights(request);
                IList<Song> songs = Database.GetSongs(request.Title);
                response.Songs = Mapper.MapFromDomain(songs);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Get (up to the first 50) songs currently in the queue.
        /// </summary>
        /// <returns></returns>
        public GetQueueResponse GetMyQueue(BaseRequest request)
        {
            GetQueueResponse response = new GetQueueResponse();
            try
            {
                User user = AssertLoginRights(request);
                response.QueuedSongs = Mapper.MapFromDomain(Database.GetQueueForUser(user));
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Get (up to the first 50) songs currently in the queue. regardless of user
        /// </summary>
        /// <returns></returns>
        public GetQueueResponse GetQueue(BaseRequest request)
        {
            GetQueueResponse response = new GetQueueResponse();
            try
            {
                AssertLoginRights(request);
                response.QueuedSongs = Mapper.MapFromDomain(Database.GetQueue());
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Returns currently playing song and next song sheduled to play
        /// </summary>
        /// <returns></returns>
        public GetCurrentSongResponse GetCurrentSong(BaseRequest request)
        {
            GetCurrentSongResponse response = new GetCurrentSongResponse();

            try
            {
                AssertLoginRights(request);
                response.CurrentSong = Player.Current.CurrentSong;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public BaseResponse RemoveFromMyQueue(RemoveQueuedSongRequest request)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                User user = AssertLoginRights(request);
                Database.RemoveFromMyQueue(user.Id,request.SongId);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public BaseResponse ClearMyQueue(BaseRequest request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                User user = AssertLoginRights(request);
                Database.ClearMyQueue(user);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public BaseResponse AddToMyQueue(QueueSongsRequest request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                User user = AssertLoginRights(request);
                Database.QueueMySongs(user, request.SongIds);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public PlayerStateResponse GetPlayerState(BaseRequest request)
        {
            PlayerStateResponse response = new PlayerStateResponse();
            try
            {
                AssertLoginRights(request);
                response.PlayerState = Player.Current.State;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public PlayerStateResponse SetPlayerState(PlayerStateRequest request)
        {
            PlayerStateResponse response = new PlayerStateResponse();
            try
            {
                AssertLoginRights(request);
                if (request.SetPlayerState == Entities.SetPlayerState.Play)
                {
                    Player.Current.Resume();
                }
                else
                {
                    Player.Current.Pause();
                }
                response.PlayerState = Player.Current.State;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public UserResponse GetUser(BaseRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                User user = AssertLoginRights(request);
                response.User = Mapper.MapFromDomain(user);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        #endregion

        private static User AssertLoginRights(BaseRequest request)
        {
            User user = Database.GetUser(request.UserToken);
            if (user == null)
                throw new Exception("You are not logged in!");

            return user;
        }
    }
}