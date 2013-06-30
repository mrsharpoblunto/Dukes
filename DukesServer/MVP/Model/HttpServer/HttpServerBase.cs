using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DukesServer.MVP.Model.HttpServer
{
    public abstract class HttpServerBase
    {
        private int _port = 8080;
        private Thread _thread;
        private TcpListener _listener;
        private bool _started;
        private object _lock = new object();
        private int _requestsInProgress;

        public string Name = "DukesServer/1.0";
        public Dictionary<int, string> ResponseStatus { get; private set;}

        protected HttpServerBase()
        {
            InitResponseStatus();
        }

        protected HttpServerBase(int thePort)
        {
            _port = thePort;
            InitResponseStatus();
        }

        public bool IsAlive
        {
            get { return _thread.IsAlive; }
        }

        public bool Started
        {
            get { return _started; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private void InitResponseStatus()
        {
            ResponseStatus = new Dictionary<int, string>
			                 {
			                     {100, "100 Continue"},
			                     {101, "101 Switching Protocols"},
			                     {200, "200 Ok"},
			                     {201, "201 Created"},
			                     {202, "202 Accepted"},
			                     {203, "203 Non-Authoritative Information"},
			                     {204, "204 No Content"},
			                     {205, "205 Reset Content"},
			                     {206, "206 Partial Content"},
                                 {207, "207 Multi-Status"},
                                 {300, "300 Multiple Choices"},
			                     {301, "301 Moved Permanently"},
			                     {302, "302 Redirection"},
			                     {303, "303 See Other"},
			                     {304, "304 Not Modified"},
			                     {305, "305 Use Proxy"},
			                     {307, "307 Temporary Redirect"},
			                     {400, "400 Bad Request"},
			                     {401, "401 Unauthorized"},
			                     {403, "403 Forbidden"},
			                     {404, "404 Not Found"},
			                     {500, "500 Internal Server Error"},
			                     {501, "501 Not Implemented"},
			                     {502, "502 Bad Gateway"},
			                     {503, "503 Service Unavailable"}
			                 };
        }

        private void Listen()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, Port);
                _listener.Start();

                WriteLog("Server Listening On: " + Port);

                while (true)
                {
                    var tcpClient = _listener.AcceptTcpClient();
                    WriteLog("Waiting for connection...");
                    Interlocked.Increment(ref _requestsInProgress);
                    ThreadPool.QueueUserWorkItem(
                        request => ((HttpRequest)request).Process(),
                        new HttpRequest(tcpClient, this));
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Logger.Current.Write(ex,"Error in httpServer");
            }
        }

        public void WriteLog(string EventMessage)
        {
            Console.WriteLine(EventMessage);
        }

        public void Start()
        {
            lock (_lock)
            {
                if (_started) return;
                OnStart();
                _thread = new Thread(Listen);
                _thread.Start();
                _started = true;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!_started) return;
                _listener.Stop();
                _thread.Abort();
                _started = false;
                //wait for any requests to complete
                while (_requestsInProgress > 0)
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void OnResponseComplete(HttpRequest request)
        {
            Interlocked.Decrement(ref _requestsInProgress);
        }

        public abstract void OnStart();
        public abstract void OnResponse(ref HttpRequestStruct request, ref HttpResponseStruct response);
    }
}
