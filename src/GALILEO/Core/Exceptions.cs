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
            this.RavenClient = new RavenClient("https://66e83d54af364f3f82dfd0872b0a723a:66c50f4676aa4496bd927d1c5a440422@sentry.io/173499")
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
