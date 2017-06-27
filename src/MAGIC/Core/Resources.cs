using System;
using System.Threading;
using CRepublic.Magic.Core.API;
using CRepublic.Magic.Core.Events;
using CRepublic.Magic.Core.Networking;

namespace CRepublic.Magic.Core
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
        internal static WebApi Api;

        internal static void Initialize()
        {
           
            Resources.Exceptions = new Exceptions();
            Resources.Players = new Players();
            Resources.Clans = new Clans();
            Resources.GChat = new Global_Chat();
            Resources.Battles = new Battles();
            Resources.Battles_V2 = new Battles_V2();
            Resources.Classes = new Classes();
            Resources.Devices = new Devices();
            Resources.Random = new Random(DateTime.Now.ToString().GetHashCode());
            Resources.Region = new Region();
            Resources.PRegion = new Player_Region();
            Resources.Gateway = new Gateway();
            Resources.Api = new WebApi();
            Resources.Parser = new Parser();
        }
    }
}
