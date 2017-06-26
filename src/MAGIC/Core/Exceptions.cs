using System;
using System.Reflection;
using CRepublic.Magic.Extensions;
using SharpRaven;
using SharpRaven.Data;

namespace CRepublic.Magic.Core
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
                    "https://4b2650dcbbed430eac450a5eb400aaa0:c9ce7df40227445c8bd027d77b88b9f3@sentry.io/180208")
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
