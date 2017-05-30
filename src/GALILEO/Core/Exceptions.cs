using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using SharpRaven;
using SharpRaven.Data;

namespace BL.Servers.CoC.Core
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
            this.RavenClient =
                new RavenClient(
                    "https://99b0a95463704edc8d7af53f46f8bf8d:9ccb19cc83474ab8b1509dadb5c5fdac@sentry.io/173491")
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
