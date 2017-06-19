using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRepublic.Royale.Core.API
{
    internal class WebAPI
    {
        private HttpListener Listener = null;
        private Prefixer Prefixer = null;
        private Thread Thread = null;

        public WebAPI()
        {
            this.Thread = new Thread(() =>
            {
                this.Prefixer = new Prefixer();
                this.Listener = new HttpListener();

                this.Listener.Prefixes.Add("http://barbarianland.xyz:8080/");
                this.Listener.Start();

                Console.WriteLine("The Web API has been sucessfully started on http://barbarianland.xyz:8080/");

                while (true)
                {
                    HttpListenerContext _Context = this.Listener.GetContext();
                    ThreadPool.QueueUserWorkItem(this.Process, _Context);
                }
            });
            this.Thread.Start();
        }

        public void Process(object _Param)
        {
            HttpListenerContext _Context = (HttpListenerContext)_Param;

            try
            {
                Request _Request = this.Prefixer.Read(_Context);

                if (_Request != null)
                {
                    _Request.Process();
                    _Request.Answer();
                }
                else
                {
                    this.Send(_Context, "unhandled");
                }
            }
            catch (Exception Exception)
            {
                Console.WriteLine(Exception);

                this.Send(_Context, "error");
            }
        }

        public void Send(HttpListenerContext _Context, string _Action)
        {
            switch (_Action)
            {
                case "unauthorized":
                    {
                        _Context.Response.StatusCode = 403;
                        _Context.Response.StatusDescription = "PROCESSED";
                        _Context.Response.ContentType = "text/plain";
                        _Context.Response.ContentLength64 = "UNAUTHORIZED".Length;
                        _Context.Response.KeepAlive = false;
                        _Context.Response.AddHeader("Content-Type", "text/plain");
                        _Context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                        _Context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("UNAUTHORIZED"), 0, "UNAUTHORIZED".Length);
                        break;
                    }

                case "error":
                    {
                        _Context.Response.StatusCode = 500;
                        _Context.Response.StatusDescription = "PROCESSED";
                        _Context.Response.ContentType = "text/plain";
                        _Context.Response.ContentLength64 = "ERROR".Length;
                        _Context.Response.KeepAlive = false;
                        _Context.Response.AddHeader("Content-Type", "text/plain");
                        _Context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                        _Context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("ERROR"), 0, "ERROR".Length);
                        break;
                    }

                case "unhandled":
                    {
                        _Context.Response.StatusCode = 501;
                        _Context.Response.StatusDescription = "PROCESSED";
                        _Context.Response.ContentType = "text/plain";
                        _Context.Response.ContentLength64 = "UNHANDLED".Length;
                        _Context.Response.KeepAlive = false;
                        _Context.Response.AddHeader("Content-Type", "text/plain");
                        _Context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                        _Context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("UNHANDLED"), 0, "UNHANDLED".Length);
                        break;
                    }

                default:
                    {
                        _Context.Response.Close();
                        break;
                    }
            }

            _Context.Response.Close();
        }

        private bool Used
        {
            get
            {
                TcpConnectionInformation[] _Connections = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
                return _Connections.Any(_Connection => _Connection.LocalEndPoint.Port == 8080);
            }
        }
    }
}
