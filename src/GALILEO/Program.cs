using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure;
using Newtonsoft.Json.Linq;
using SharpRaven.Data;

namespace BL.Servers.CoC
{
    internal class Program
    {
        internal static Stopwatch Stopwatch = Stopwatch.StartNew();
        internal static void Main(string[] args)
        {
            Console.Title = $"BarbarianLand Clash Server - ©BarbarianLand ";
            //NativeCalls.SetWindowLong(NativeCalls.GetConsoleWindow(), -20, (int) NativeCalls.GetWindowLong(NativeCalls.GetConsoleWindow(), -20) ^ 0x80000);
            //NativeCalls.SetLayeredWindowAttributes(NativeCalls.GetConsoleWindow(), 0, 217, 0x2);

            Console.SetOut(new Prefixed());

            Console.ForegroundColor = Utils.ChooseRandomColor();

            Console.WriteLine(@"__________             ___.                .__                     .____                       .___");
            Console.WriteLine(@"\______   \_____ ______\_ |__ _____ _______|__|____    ____   _____|    |   _____    ____    __| _/");
            Console.WriteLine(@" |    |  _/\__  \\_  __ \ __ \\__  \\_  __ \  \__  \  /    \ /  ___/    |   \__  \  /    \  / __ | ");
            Console.WriteLine(@" |    |   \ / __ \|  | \/ \_\ \/ __ \|  | \/  |/ __ \|   |  \\___ \|    |___ / __ \|   |  \/ /_/ | ");
            Console.WriteLine(@" |______  /(____  /__|  |___  (____  /__|  |__(____  /___|  /____  >_______ (____  /___|  /\____ | ");
            Console.WriteLine(@"        \/      \/          \/     \/              \/     \/     \/        \/    \/     \/      \/  ");
            Console.WriteLine(@"                                                                           Developer Edition  ");

            Console.ResetColor();

            /*
#if DEBUG
            Console.ForegroundColor = Utils.ChooseRandomColor();
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Initialized Flux capacitor v6.69");
            Console.WriteLine("Loading hash tables to run flux capacitor v6.69 to connect with the International Space Station.");
            Console.WriteLine("Connecting to custom MongoDB v5.2.0 server powered by nuclear energy (Uranium & Plutonium).");
            Console.WriteLine("Hacking into the NSA to get CSV tables.");
            Console.WriteLine("Hacking into Supercell's Amazon Web Service to extract the latest keys.");
            Console.WriteLine("Server is ready to start sending nuclear warheads into space and handle Clash Of Clans connections.");
            Console.ResetColor();
            Console.WriteLine(Environment.NewLine);
#endif*/
            Resources.Initialize();
            Console.WriteLine(new DateTime(1308762992));
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + Environment.NewLine);
            Thread.Sleep(Timeout.Infinite);

        }
    }
}
