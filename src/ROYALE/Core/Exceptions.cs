using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Core
{
    internal class Exceptions
    {
        internal RavenClient RavenClient;

        internal Exceptions()
        {
            string Enviroment;
            if (Constants.Local)
                Enviroment = "local";
            else
            {
#if DEBUG
                Enviroment = "debug";
#else
                Enviroment = "production";
#endif
            }
            this.RavenClient = new RavenClient("https://4e333c94a81f4f7eb851c2525959a97c:8315a2aad73246b8b1d4f4ff8e53d732@sentry.io/173535")
            {
                Environment = Enviroment,
                Release = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        internal async void Catch(Exception Exception)
        {
            SentryEvent Event = new SentryEvent(Exception);

            await this.RavenClient.CaptureAsync(Event);
        }

        internal async void Catch(Exception Exception, string Message, string Model = "", string OS = "", string Token = "", long ID = 0)
        {
            SentryEvent Event = new SentryEvent(Exception)
            {
                Message = Message,
            };


            Event.Tags.Add("token", Token);
            Event.Tags.Add("userid", ID.ToString());
            Event.Tags.Add("model", Model);
            Event.Tags.Add("os", OS);

            await this.RavenClient.CaptureAsync(Event);
        }

    }
}
