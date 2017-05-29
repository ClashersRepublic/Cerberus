using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Core
{
    internal class Exceptions
    {
        private RavenClient RavenClient;

        internal Exceptions()
        {
            this.RavenClient = new RavenClient("https://ca909d4637c544af9fdad37498660015:973ad43758624eaa95f6d04fd90b22da@sentry.io/172984");

#if DEBUG
            this.RavenClient.Environment = "debug";
            #endif

            #if RELEASE
            this.RavenClient.Environment = "production"
            #endif

            if (Constants.Local)
            {
                this.RavenClient.Environment = "local";
            }

            this.RavenClient.Release = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.RavenClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public void Catch(Exception Exception)
        {
            SentryEvent Event = new SentryEvent(Exception);

            this.RavenClient.CaptureAsync(Event);
        }

        public void Catch(Exception Exception, string Message, string Model = "", string OS = "", string Token = "", long ID = 0)
        {
            SentryEvent Event = new SentryEvent(Exception);

            Event.Message = Message;

            Event.Tags.Add("token", Token);
            Event.Tags.Add("userid", ID.ToString());
            Event.Tags.Add("model", Model);
            Event.Tags.Add("os", OS);

            this.RavenClient.CaptureAsync(Event);
        }
    }
}
