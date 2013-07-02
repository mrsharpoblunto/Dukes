using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using DukesServer.MVP.Model.Service;
using DukesServer.MVP.Model.Service.Messages;

namespace DukesServer.MVP.Model.HttpServer
{
    class WebUIHttpServer : HttpServerBase
    {
        private readonly DukesJsonService _service;
        private readonly Dictionary<string, byte[]> _cache;
        private readonly bool _cacheFileContent;

        public WebUIHttpServer(bool cacheFileContent)
            : base()
        {
            _cacheFileContent = cacheFileContent;
            _service = new DukesJsonService();
            _cache = new Dictionary<string, byte[]>();
        }

        public override void OnResponse(ref HttpRequestStruct request, ref HttpResponseStruct response)
        {
            try
            {
                if (request.Headers.ContainsKey("Authorization"))
                {
                    //get the credentials
                    LoginResponse loginResponse = _service.Login(request.Headers["Authorization"]);
                    if (loginResponse.Success)
                    {
                        if (request.Url.StartsWith("/ClearCache", StringComparison.InvariantCultureIgnoreCase))
                        {
                            lock (_cache)
                            {
                                _cache.Clear();
                            }
                        }
                        else if (request.Url.StartsWith("/DukesService",StringComparison.InvariantCultureIgnoreCase))
                        {
                            JsonServiceResponse(loginResponse.TokenId,ref request, ref response);
                        }
                        else
                        {
                            FileResponse(ref request, ref response);
                        }
                    }
                    else
                    {
                        UnauthenticatedResponse(ref response);
                    }

                }
                else
                {
                    UnauthenticatedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                ServerErrorResponse(ex,ref response);
            }
        }

        private void FileResponse(ref HttpRequestStruct request, ref HttpResponseStruct response)
        {
             if (request.Url=="/")
             {
                 request.Url = "/default.html";
             }

            string rootDir = Path.Combine(Constants.AppDir, "WebUI");
            string requestedFile = Path.Combine(rootDir, request.Url.Substring(1).Replace('/','\\'));

            FileInfo fi = new FileInfo(requestedFile);
            //404 if the file doesn't exist of if the client has requested a file outside of the webui directory
            if (!fi.Exists || !fi.FullName.Contains(rootDir))
            {
                NotFoundResponse(request.Url,ref response);
            }
            else
            {
                response.Status = 200;
                response.Headers.Add("Content-Type", GetContentType(fi.Extension));
                response.BodyData = GetFileContent(fi.FullName);
            }
        }

        private byte[] GetFileContent(string name)
        {
            name = name.ToLowerInvariant();
            byte[] content= null;
            if (!_cacheFileContent || !_cache.TryGetValue(name, out content))
            {
                lock (_cache)
                {
                    if (!_cacheFileContent || !_cache.TryGetValue(name, out content))
                    {
                        using (var filestream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (var ms = new MemoryStream())
                            {
                                filestream.CopyTo(ms);

                                content = ms.ToArray();
                                if (_cacheFileContent)
                                {
                                    _cache.Add(name, content);
                                }
                            }
                        }
                    }
                }
            }
            return content;
        }

        private static string GetContentType(string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".html":
                case ".htm":
                    return "text/html";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".ico":
                    return "image/x-icon";
                case ".css":
                    return "text/css";
                case ".js":
                    return "text/javascript";
                default:
                    return "text/plain";
            }
        }

        private void JsonServiceResponse(Guid tokenId, ref HttpRequestStruct request, ref HttpResponseStruct response)
        {
            string[] requestComponents = request.Url.Split(new [] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            if (requestComponents.Length!=2)
            {
                throw new Exception("Invalid Json service request");
            }

            switch (requestComponents[1].ToLowerInvariant())
            {
                case "getsongsbyartistandalbum":
                    response.BodyData = _service.GetSongsByArtistAndAlbum(tokenId, request.QueryString);
                    break;
                case "getsongsbytitle":
                    response.BodyData = _service.GetSongsByTitle(tokenId, request.QueryString);
                    break;
                case "getqueue":
                    response.BodyData = _service.GetQueue(tokenId);
                    break;
                case "getmyqueue":
                    response.BodyData = _service.GetMyQueue(tokenId);
                    break;
                case "getplayerstate":
                    response.BodyData = _service.GetPlayerState(tokenId);
                    break;
                case "getcurrentsong":
                    response.BodyData = _service.GetCurrentSong(tokenId);
                    break;
                case "addtomyqueue":
                    response.BodyData = _service.AddToMyQueue(tokenId, Encoding.UTF8.GetString(request.BodyData));
                    break;
                case "clearmyqueue":
                    response.BodyData = _service.ClearMyQueue(tokenId);
                    break;
                case "removefrommyqueue":
                    response.BodyData = _service.RemoveFromMyQueue(tokenId, request.QueryString);
                    break;
                case "setplayerstate":
                    response.BodyData = _service.SetPlayerState(tokenId, request.QueryString);
                    break;
                case "getuser":
                    response.BodyData = _service.GetUser(tokenId);
                    break;
            }

            response.Status = 200;
            response.Headers["Content-Type"] = "application/json";
        }

        private static void ServerErrorResponse(Exception ex,ref HttpResponseStruct response)
        {
            response.Status = 500;
            if (response.Headers.ContainsKey("Content-Type"))
            {
                response.Headers["Content-Type"] = "text/html";
            }
            else
            {
                response.Headers.Add("Content-Type", "text/html");
            }
            response.BodyData = Encoding.UTF8.GetBytes(
                @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <title>Error</title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
  </head>
  <body><h1>500 Server error.</h1><h2>" + ex+@"</h2></body>
</html>
");
        }

        private static void UnauthenticatedResponse(ref HttpResponseStruct response)
        {
            response.Status = 401;
            response.Headers.Add("WWW-Authenticate","Basic realm=\"Dukes WebUI\"");
            response.Headers.Add("Content-Type", "text/html");
            response.BodyData = Encoding.UTF8.GetBytes(
                @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <title>Error</title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
  </head>
  <body><h1>401 Unauthorised.</h1></body>
</html>
");
        }

        private static void NotFoundResponse(string url,ref HttpResponseStruct response)
        {
            response.Status = 404;
            if (response.Headers.ContainsKey("Content-Type"))
            {
                response.Headers["Content-Type"] = "text/html";
            }
            else
            {
                response.Headers.Add("Content-Type", "text/html");
            }
            response.BodyData = Encoding.UTF8.GetBytes(
                @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <title>Error</title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
  </head>
  <body><h1>404 Not found " + url+@"</h1></body>
</html>
");
        }
    }
}
