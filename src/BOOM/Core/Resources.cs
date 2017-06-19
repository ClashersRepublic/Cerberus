using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Core.Events;
using CRepublic.Boom.Core.Network.TCP;

namespace CRepublic.Boom.Core
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
