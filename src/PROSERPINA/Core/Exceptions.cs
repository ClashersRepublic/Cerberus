using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RollbarDotNet;

namespace BL.Servers.CR.Core
{
    internal class Exceptions
    {
        internal Exceptions()
        {
            Rollbar.Init(new RollbarConfig
            {
                AccessToken = "5fe9e1702a124202b8af85a8c2382e41",
                Environment = "production"             
            });
        }

        public void Catch(System.Exception Exception, ErrorLevel ErrorLevel = ErrorLevel.Info)
        {
            Rollbar.Report(Exception, ErrorLevel);
        }

        internal void Catch(System.Exception ex, string v)
        {
            throw new NotImplementedException();
        }
    }
}
