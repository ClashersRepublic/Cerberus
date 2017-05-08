using System;
using BL.Servers.CoC.Core.Networking;

namespace BL.Servers.CoC.Core
{
    internal class Resources
    {
        internal static Devices Devices;
        internal static Players Players;
        internal static Random Random;
        internal static Classes Classes;
        internal static Global_Chat GChat;
        internal static Region Region;
        internal static Player_Region PRegion;
        internal static Gateway Gateway;

        internal static void Initialize()
        {
            Resources.Classes = new Classes();
            Resources.Devices = new Devices();
            Resources.Players = new Players();
            Resources.Random = new Random();
            Resources.GChat = new Global_Chat();
            Resources.Region = new Region();
            Resources.PRegion = new Player_Region();
            Resources.Gateway = new Gateway();
        }
    }
}
