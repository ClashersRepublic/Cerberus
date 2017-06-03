using BL.Servers.CR.Core.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Core.API
{
    internal class Prefixer
    {
        private readonly Dictionary<string, Type> Prefixes = new Dictionary<string, Type>();

        public Prefixer()
        {
            this.Prefixes = new Dictionary<string, Type>();

            Prefixes.Add("player", typeof(Request_Player));

        }

        public void Dispose()
        {
            this.Prefixes.Clear();
        }

        public Request Read(HttpListenerContext _Context)
        {
            string _Command = _Context.Request.QueryString.Get("command");

            if (!string.IsNullOrEmpty(_Command))
            {
                if (this.Prefixes.ContainsKey(_Command))
                {
                    Request Request = Activator.CreateInstance(this.Prefixes[_Command], this, _Command) as Request;

                    Request.Answer();
                    Request.Process();
                }
                else
                {
                    Console.WriteLine("Command does not exist!");
                }

                return null;
            }
            else
            {
                Console.WriteLine("Command is null! " + _Context.Request.QueryString.Get("command"));
                return null;
            }
        }
    }
}
