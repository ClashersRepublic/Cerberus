using System;
using System.Threading;
using Republic.Magic.Core.Events;
using Republic.Magic.Core.Networking;

namespace Republic.Magic.Core
{
    internal class Resources
    {
        internal static Exceptions Exceptions;
        internal static Devices Devices;
        internal static Players Players;
        internal static Clans Clans;
        internal static Battles Battles;
        internal static Battles_V2 Battles_V2;
        internal static Random Random;
        internal static Classes Classes;
        internal static Global_Chat GChat;
        internal static Region Region;
        internal static Player_Region PRegion;
        internal static Gateway Gateway;
        internal static Parser Parser;

        internal static void Initialize()
        {
           
            Resources.Exceptions = new Exceptions();
            Resources.Classes = new Classes();
            Resources.Devices = new Devices();
            Resources.Players = new Players();
            Resources.Clans = new Clans();
            Resources.GChat = new Global_Chat();
            Resources.Battles = new Battles();
            Resources.Battles_V2 = new Battles_V2();
            Resources.Random = new Random(DateTime.Now.ToString().GetHashCode());
            Resources.Region = new Region();
            Resources.PRegion = new Player_Region();
            Resources.Gateway = new Gateway();
            Resources.Parser = new Parser();
        }
    }
}
