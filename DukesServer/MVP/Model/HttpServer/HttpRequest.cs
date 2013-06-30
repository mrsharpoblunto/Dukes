using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace DukesServer.MVP.Model.HttpServer
{
    internal enum RequestState
    {
        Method,
        Url,
        UrlParam,
        UrlValue,
        Version,
        HeaderKey,
        HeaderValue,
        Body,
        Ok
    } ;

    internal enum ResponseState
    {
        Ok = 200,
        BadRequest = 400,
        NotFound = 404
    }

    public struct HttpRequestStruct
    {
        public byte[] BodyData { get; set;}
        public int BodySize { get; set; }
        public bool Execute { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string Version { get; set; }
    }

    public struct HttpResponseStruct
    {
        public byte[] BodyData { get; set; }
        public int BodySize { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public int Status { get; set; }
        public string Version { get; set; }
    }

    public class HttpRequest
    {
        private readonly TcpClient _client;
        private readonly HttpServerBase _parent;

        private HttpRequestStruct _httpRequest;
        private HttpResponseStruct _httpResponse;

        private byte[] _myReadBuffer;
        private RequestState _parserState;

        public HttpRequest(TcpClient client, HttpServerBase Parent)
        {
            _client = client;
            _parent = Parent;

            _httpResponse.BodySize = 0;
        }

        public void Process()
        {
            _myReadBuffer = new byte[_client.ReceiveBufferSize];
            String myCompleteMessage = string.Empty;

            _parent.WriteLog("Connection accepted. Buffer: " + _client.ReceiveBufferSize);
            NetworkStream ns = _client.GetStream();

            string hValue = string.Empty;
            string hKey = string.Empty;

            try
            {
                // binary data buffer index
                int bfndx = 0;

                // Incoming message may be larger than the buffer size.
                do
                {
                    int numberOfBytesRead = ns.Read(_myReadBuffer, 0, _myReadBuffer.Length);
                    myCompleteMessage = String.Concat(myCompleteMessage, Encoding.ASCII.GetString(_myReadBuffer, 0, numberOfBytesRead));

                    // read buffer index
                    int ndx = 0;
                    do
                    {
                        switch (_parserState)
                        {
                            case RequestState.Method:
                                if (_myReadBuffer[ndx] != ' ')
                                    _httpRequest.Method += (char)_myReadBuffer[ndx++];
                                else
                                {
                                    ndx++;
                                    _parserState = RequestState.Url;
                                }
                                break;
                            case RequestState.Url:
                                if (_myReadBuffer[ndx] == '?')
                                {
                                    ndx++;
                                    _httpRequest.QueryString = "?";
                                    _httpRequest.Execute = true;
                                    _parserState = RequestState.UrlParam;
                                }
                                else if (_myReadBuffer[ndx] != ' ')
                                    _httpRequest.Url += (char)_myReadBuffer[ndx++];
                                else
                                {
                                    ndx++;
                                    _httpRequest.Url
                                        = HttpUtility.UrlDecode(_httpRequest.Url);
                                    _parserState = RequestState.Version;
                                }
                                break;
                            case RequestState.UrlParam:
                                if (_myReadBuffer[ndx] == ' ')
                                {
                                    ndx++;

                                    _httpRequest.QueryString
                                        = HttpUtility.UrlDecode(_httpRequest.QueryString);
                                    _httpRequest.Url
                                        = HttpUtility.UrlDecode(_httpRequest.Url);
                                    _parserState = RequestState.Version;
                                }
                                else
                                {
                                    _httpRequest.QueryString += (char)_myReadBuffer[ndx++];
                                }
                                break;
                            case RequestState.Version:
                                if (_myReadBuffer[ndx] == '\r')
                                    ndx++;
                                else if (_myReadBuffer[ndx] != '\n')
                                    _httpRequest.Version += (char)_myReadBuffer[ndx++];
                                else
                                {
                                    ndx++;
                                    hKey = "";
                                    _httpRequest.Headers = new Dictionary<string, string>();
                                    _parserState = RequestState.HeaderKey;
                                }
                                break;
                            case RequestState.HeaderKey:
                                if (_myReadBuffer[ndx] == '\r')
                                    ndx++;
                                else if (_myReadBuffer[ndx] == '\n')
                                {
                                    ndx++;
                                    if (_httpRequest.Headers.ContainsKey("Content-Length"))
                                    {
                                        _httpRequest.BodySize =
                                            Convert.ToInt32(_httpRequest.Headers["Content-Length"]);
                                        _httpRequest.BodyData
                                            = new byte[_httpRequest.BodySize];
                                        _parserState = RequestState.Body;
                                    }
                                    else
                                        _parserState = RequestState.Ok;
                                }
                                else if (_myReadBuffer[ndx] == ':')
                                {
                                    ndx++;
                                    hValue = "";
                                    _parserState = RequestState.HeaderValue;
                                }
                                else
                                    hKey += (char)_myReadBuffer[ndx++];
                                break;
                            case RequestState.HeaderValue:
                                if (_myReadBuffer[ndx] == '\r')
                                    ndx++;
                                else if (_myReadBuffer[ndx] != '\n')
                                    hValue += (char)_myReadBuffer[ndx++];
                                else
                                {
                                    ndx++;
                                    _httpRequest.Headers.Add(hKey.Trim(), hValue.Trim());
                                    hKey = "";
                                    _parserState = RequestState.HeaderKey;
                                }
                                break;
                            case RequestState.Body:
                                // Append to request BodyData

                                Array.Copy(_myReadBuffer, ndx,
                                           _httpRequest.BodyData,
                                           bfndx, numberOfBytesRead - ndx);
                                bfndx += numberOfBytesRead - ndx;
                                ndx = numberOfBytesRead;
                                if (_httpRequest.BodySize <= bfndx)
                                {
                                    _parserState = RequestState.Ok;
                                }
                                break;
                        }
                    } while (ndx < numberOfBytesRead);
                } while (ns.DataAvailable);

                // Print out the received message to the console.
                _parent.WriteLog("You received the following message : \n" + myCompleteMessage);

                _httpResponse.Version = "HTTP/1.1";

                if (_parserState != RequestState.Ok)
                    _httpResponse.Status = (int)ResponseState.BadRequest;
                else
                    _httpResponse.Status = (int)ResponseState.Ok;

                _httpResponse.Headers = new Dictionary<string, string>();
                if (_httpRequest.Url.EndsWith("#"))
                {
                    _httpRequest.Url = _httpRequest.Url.Substring(0, _httpRequest.Url.Length - 1);
                }
                _parent.OnResponse(ref _httpRequest, ref _httpResponse);

                string HeadersString = _httpResponse.Version + " " + _parent.ResponseStatus[_httpResponse.Status] + "\r\n";

                if (_httpResponse.BodyData != null && !_httpResponse.Headers.ContainsKey("Content-Length"))
                {
                    _httpResponse.Headers.Add("Content-Length", _httpResponse.BodyData.Length.ToString());
                }

                foreach (var header in _httpResponse.Headers)
                {
                    HeadersString += header.Key + ":" + header.Value + "\r\n";
                }

                HeadersString += "\r\n";
                byte[] bHeadersString = Encoding.ASCII.GetBytes(HeadersString);

                // Send headers   
                ns.Write(bHeadersString, 0, bHeadersString.Length);

                // Send body
                if (_httpResponse.BodyData != null)
                {
                    ns.Write(_httpResponse.BodyData, 0, _httpResponse.BodyData.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex, "Failed to process request");
                _parent.WriteLog(ex.ToString());
            }
            finally
            {
                ns.Close();
                _client.Close();
            }
            _parent.OnResponseComplete(this);
        }
    }
}
