#define Raven
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Logic.Enums;
using SharpRaven;
using SharpRaven.Data;

namespace CRepublic.Royale.Core
{
    internal static class Exceptions
    {
        private static readonly object s_sync = new object();
        internal static RavenClient RavenClient;

        public static void Initialize()
        {
            if (Constants.CloudLogging)
            {
                string Enviroment;
#if DEBUG
            Enviroment = "debug"; 
#else
                Enviroment = "production";
#endif

                RavenClient =
                    new RavenClient(
                        "https://c011a726734a4b5baf17f2d1a7519374:534d376264934c7084a24079726b8e99@sentry.io/187787")
                    {
                        Environment = Enviroment,
                        Release = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        Timeout = TimeSpan.FromSeconds(5)
                    };
            }
        }

        internal static void Log(Exception ex, string moreInfo = "", string Model = "", string OS = "", string Token = "", long ID = 0)
        {
            var time = DateTime.Now;
            var type = ex.GetType();
            var name = type.Name;
            var dirPath = Path.Combine("logs", name);

            if (!Directory.Exists(name))
                Directory.CreateDirectory(dirPath);

            var text = "[" + time.ToString("yyyy-MM-dd hh:mm:ss.fff") + "]: " + ex.Message + "\r\n" + ex.StackTrace + "\r\n\r\n";
            var filePath = Path.Combine(dirPath, GetLogFileName(moreInfo));

            lock (s_sync)
                File.AppendAllText(filePath, text);

            if (Constants.CloudLogging)
            {
                SentryEvent Event = new SentryEvent(ex)
                {
                    Message = moreInfo,
                };


                Event.Tags.Add("token", Token);
                Event.Tags.Add("userid", ID.ToString());
                Event.Tags.Add("model", Model);
                Event.Tags.Add("os", OS);

                RavenClient.Capture(Event);
            }
        }

        private static string GetLogFileName(string moreInfo)
        {
            if (string.IsNullOrWhiteSpace(moreInfo))
                return "unknown.log";
            else
                return StripString(moreInfo).Replace(".", string.Empty) + ".log";
        }

        private static string StripString(string value)
        {
            // Trim the string.
            if (value.Length > 50)
                value = value.Substring(0, 50);

            var invalidChars = Path.GetInvalidFileNameChars();

            return (from t1 in value let invalid = invalidChars.Any(t => t1 == t) where !invalid select t1).Aggregate(string.Empty, (current, t1) => current + t1);
        }
    }
}
