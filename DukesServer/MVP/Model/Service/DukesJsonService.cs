using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DukesServer.MVP.Model.Service.Entities;
using DukesServer.MVP.Model.Service.Messages;
using Newtonsoft.Json;

namespace DukesServer.MVP.Model.Service
{
    /// <summary>
    /// This class provides a wrapper around the wcf service and returns json responses for use in the WebUI.
    /// </summary>
    internal class DukesJsonService
    {
        private readonly DukesService _service = new DukesService();

        public LoginResponse Login(string authenticationHeader)
        {
            LoginRequest request = new LoginRequest();
            string[] authComponents = authenticationHeader.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string[] authString = Encoding.UTF8.GetString(Convert.FromBase64String(authComponents[1])).Split(new[] { ':' });
            request.Username = authString[0];
            request.Password = authString[1];

            LoginResponse response = _service.Login(request);
            return response;
        }

        public byte[] GetUser(Guid guid)
        {
            BaseRequest request = new BaseRequest {UserToken = guid};
            var response = _service.GetUser(request);
            return SerializeResponse(response);
        }

        public byte[] GetSongsByArtistAndAlbum(Guid guid, string s)
        {
            var qs = ParseQueryString(s);
            GetSongsByArtistAndAlbumRequest request = new GetSongsByArtistAndAlbumRequest {UserToken = guid};

            var artistKvp = qs.SingleOrDefault(kvp => kvp.Key.Equals("artist",StringComparison.InvariantCultureIgnoreCase));
            request.Artist = artistKvp.Value;

            var albumKvp = qs.SingleOrDefault(kvp => kvp.Key.Equals("album", StringComparison.InvariantCultureIgnoreCase));
            request.Album = albumKvp.Value;

            var response =  _service.GetSongsByArtistAndAlbum(request);
            return SerializeResponse(response);
        }

        public byte[] GetSongsByTitle(Guid guid, string s)
        {
            var qs = ParseQueryString(s);
            GetSongsByTitle request = new GetSongsByTitle { UserToken = guid };

            var titleKvp = qs.SingleOrDefault(kvp => kvp.Key.Equals("title", StringComparison.InvariantCultureIgnoreCase));
            request.Title = titleKvp.Value;

            var response = _service.GetSongsByTitle(request);
            return SerializeResponse(response);
        }

        public byte[] GetQueue(Guid guid)
        {
            BaseRequest request = new BaseRequest {UserToken = guid};
            var response = _service.GetQueue(request);
            return SerializeResponse(response);
        }

        public byte[] GetMyQueue(Guid guid)
        {
            BaseRequest request = new BaseRequest { UserToken = guid };
            var response = _service.GetMyQueue(request);
            return SerializeResponse(response);
        }

        public byte[] GetPlayerState(Guid guid)
        {
            BaseRequest request = new BaseRequest { UserToken = guid };
            var response = _service.GetPlayerState(request);
            return SerializeResponse(response);
        }

        public byte[] GetCurrentSong(Guid guid)
        {
            BaseRequest request = new BaseRequest { UserToken = guid };
            var response = _service.GetCurrentSong(request);
            return SerializeResponse(response);
        }

        public byte[] AddToMyQueue(Guid guid, string s)
        {
            string[] ids = HttpUtility.UrlDecode(s).Substring(4).Split(new[] {','});
            QueueSongsRequest request = new QueueSongsRequest { UserToken = guid };
            foreach (var id in ids)
            {
                request.SongIds.Add(new Guid(id));
            }
            var response = _service.AddToMyQueue(request);
            return SerializeResponse(response);
        }

        public byte[] ClearMyQueue(Guid guid)
        {
            BaseRequest request = new BaseRequest { UserToken = guid };
            var response = _service.ClearMyQueue(request);
            return SerializeResponse(response);
        }

        public byte[] RemoveFromMyQueue(Guid guid, string s)
        {
            var qs = ParseQueryString(s);
            RemoveQueuedSongRequest request = new RemoveQueuedSongRequest { UserToken = guid };
            var idKvp = qs.SingleOrDefault(kvp => kvp.Key.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            request.SongId = new Guid(idKvp.Value);
            var response = _service.RemoveFromMyQueue(request);
            return SerializeResponse(response);
        }

        public byte[] SetPlayerState(Guid guid, string s)
        {
            var qs = ParseQueryString(s);
            PlayerStateRequest request = new PlayerStateRequest {UserToken = guid};
            var stateKvp = qs.SingleOrDefault(kvp => kvp.Key.Equals("state", StringComparison.InvariantCultureIgnoreCase));
            request.SetPlayerState = (SetPlayerState)Enum.Parse(typeof(SetPlayerState), stateKvp.Value, true);
            var response = _service.SetPlayerState(request);
            return SerializeResponse(response);
        }

        private static List<KeyValuePair<string,string>> ParseQueryString(string queryString)
        {
            List<KeyValuePair<string,string>> results = new List<KeyValuePair<string, string>>();

            string[] queryStringComponents = queryString.Substring(1).Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string component in queryStringComponents)
            {
                int equalsIndex = component.IndexOf("=");
                if (equalsIndex>0)
                {
                    results.Add(new KeyValuePair<string, string>(HttpUtility.UrlDecode(component.Substring(0, equalsIndex)), HttpUtility.UrlDecode(component.Substring(equalsIndex + 1))));
                }
                else
                {
                    results.Add(new KeyValuePair<string, string>(string.Empty, HttpUtility.UrlDecode(component)));
                }
            }

            return results;
        }

        private static byte[] SerializeResponse(object response)
        {
            JsonSerializer serializer = new JsonSerializer();
            MemoryStream stream = new MemoryStream();
            using (TextWriter writer = new StreamWriter(stream))
            {
                serializer.Serialize(writer, response);
            }
            return stream.ToArray();
        }
    }
}
