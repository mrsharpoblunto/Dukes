using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DukesServer.MVP.Model.HttpServer
{
	public enum HttpServerState
	{
		Stopped,
		Starting,
		Started,
		Stopping
	}

	public abstract class HttpServerBase
	{
		private Thread _thread;
		private TcpListener _listener;
		private object _lock = new object();
		private int _requestsInProgress;

		public const string Name = "DukesServer/1.0";
		public Dictionary<int, string> ResponseStatus { get; private set;}

		public event EventHandler OnListenError;
		public event EventHandler OnError;

		protected HttpServerBase()
		{
			Port = 8086;
			InitResponseStatus();
		}

		protected HttpServerBase(int thePort)
		{
			Port = thePort;
			InitResponseStatus();
		}

		public bool IsAlive
		{
			get { return _thread.IsAlive; }
		}

		public HttpServerState State
		{
			get;
			private set;
		}

		public int Port
		{
			get;
			set;
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
			bool restart = true;
			while (restart)
			{
				try
				{
					_listener = new TcpListener(IPAddress.Any, Port);
					_listener.Start();

					lock (_lock)
					{
						State = HttpServerState.Started;
					}

					WriteLog("Server Listening On: " + Port);

					while (true)
					{
						WriteLog("Waiting for connection...");
						var tcpClient = _listener.AcceptTcpClient();
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
					HttpServerState state;
					lock (_lock)
					{
						state = State;
					}

					if (state == HttpServerState.Starting)
					{
						// indicated we couldn't listen on the specified port
						if (OnListenError != null)
						{
							OnListenError(this, new EventArgs());
						}
						Logger.Current.Write(ex, "Error in httpServer");
						restart = false;
					}
					else if (state == HttpServerState.Started)
					{
                        // unknown error. log an error and restart the server
						if (OnError != null)
						{
							OnError(this, new EventArgs());
						}
						Logger.Current.Write(ex, "Error in httpServer");
					}
					else if (state == HttpServerState.Stopping)
					{
						restart = false;
					}
				}

				//wait for any requests to complete
				while (_requestsInProgress > 0)
				{
					Thread.Sleep(10);
				}
			}

			lock (_lock)
			{
				State = HttpServerState.Stopped;
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
				if (State != HttpServerState.Stopped) return;
				State = HttpServerState.Starting;
			}
			_thread = new Thread(Listen);
			_thread.Start();
		}

		public void Stop()
		{
			lock (_lock)
			{
				if (State != HttpServerState.Started) return;
				State = HttpServerState.Stopping;
			}
			_listener.Stop();
			_thread.Join();
		}

		public void OnResponseBegin(HttpRequest request)
		{
			Interlocked.Increment(ref _requestsInProgress);
		}

		public void OnResponseEnd(HttpRequest request)
		{
			Interlocked.Decrement(ref _requestsInProgress);
		}

		public abstract void OnResponse(ref HttpRequestStruct request, ref HttpResponseStruct response);
	}
}
