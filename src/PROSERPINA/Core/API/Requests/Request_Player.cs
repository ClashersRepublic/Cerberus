using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Core.API.Requests
{
    internal class Request_Player : Request
    {
        public Request_Player(HttpListenerContext Context) : base (Context)
        {

        }

        internal override void Answer()
        {
            base.Answer();
        }

        internal override void Process()
        {
            Console.WriteLine("Test");
        }
    }
}
