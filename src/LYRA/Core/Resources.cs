using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Networking.Lyra.Core.Network;

namespace BL.Networking.Lyra.Core
{
    internal class Resources
    {
        internal static Client Client;
        internal static Proxy Proxy;
        internal static SendReceiveThread SendReceiveThread;

        internal static void Initialize()
        {
            Resources.Client = new Client();
            Resources.Proxy = new Proxy();
        }
    }
}
