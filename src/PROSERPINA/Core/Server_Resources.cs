using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.Servers.CR.Core.API;
using BL.Servers.CR.Core.Events;
using BL.Servers.CR.Core.Network.TCP;
using BL.Servers.CR.Core.Server_Components;

namespace BL.Servers.CR.Core
{
    internal class Server_Resources
    {
        internal static Classes Classes;
        internal static Exceptions Exceptions;
        internal static Gateway Gateway;
        internal static Devices Devices;
        internal static Players Players;
        internal static Clans Clans;
        internal static Battles Battles;
        internal static Region Region;
        internal static Random Random;
        internal static Parser Parser;
        internal static WebAPI WebAPI;

        internal static void Initialize()
        {
            Server_Resources.Classes = new Classes();
            Server_Resources.Exceptions = new Exceptions();
            Server_Resources.Devices = new Devices();
            Server_Resources.Players = new Players();
            Server_Resources.Clans = new Clans();
            Server_Resources.Battles = new Battles();
            Server_Resources.Random = new Random();
            Server_Resources.Region = new Region();
            Server_Resources.WebAPI = new WebAPI();
            Server_Resources.Gateway = new Gateway();
            Server_Resources.Parser = new Parser();
        }
    }
}
