using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Core.Events;
using BL.Servers.CR.Core.Network.TCP;
using BL.Servers.CR.Logic.Slots;

namespace BL.Servers.CR.Core
{
    internal class Resources
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

        internal static void Initialize()
        {
            Resources.Classes = new Classes();
            Resources.Exceptions = new Exceptions();
            Resources.Devices = new Devices();
            Resources.Players = new Players();
            Resources.Clans = new Clans();
            Resources.Battles = new Battles();
            Resources.Random = new Random();
            Resources.Region = new Region();
            Resources.Gateway = new Gateway();
            Resources.Parser = new Parser();
        }
    }
}
