using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Core.API
{
    internal class Request
    {
        public Request()
        {
            this.Context = null;
            this.Requests = new NameValueCollection();
            this.Depth = 0;
            this.Response = Encoding.UTF8.GetBytes("OK");
        }

        public Request(HttpListenerContext _Context)
        {
            this.Context = _Context;
            this.Requests = _Context.Request.QueryString;
            this.Response = Encoding.UTF8.GetBytes("OK");
        }

        public HttpListenerContext Context
        {
            get;
            set;
        }

        public int Depth
        {
            get;
            set;
        }

        public NameValueCollection Requests
        {
            get;
            set;
        }

        public byte[] Response
        {
            get;
            set;
        }

        internal virtual void Answer()
        {
            this.Context.Response.StatusCode = 200;
            this.Context.Response.StatusDescription = "PROCESSED";
            this.Context.Response.ContentType = "text/plain";
            this.Context.Response.ContentLength64 = this.Response.LongLength;
            this.Context.Response.KeepAlive = false;
            this.Context.Response.AddHeader("Content-Type", "text/plain");
            this.Context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
            this.Context.Response.OutputStream.Write(this.Response, 0, this.Response.Length);
            this.Context.Response.Close();
        }

        internal virtual void Process()
        {
        }
    }
}