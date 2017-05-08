using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.BB.Core.Events;
using BL.Servers.BB.Core.Network.TCP;

namespace BL.Servers.BB.Core
{
    internal class Resources
    {
        internal static Classes Classes;
        internal static Gateway Gateway;
        internal static Devices Devices;
        internal static Players Players;
        internal static Region Region;
        internal static Random Random;

        internal static void Initialize()
        {
            Resources.Classes = new Classes();
            Resources.Devices = new Devices();
            Resources.Players = new Players();
            Resources.Random = new Random();
            Resources.Region = new Region();
            Resources.Gateway = new Gateway();
        }

    }
}
