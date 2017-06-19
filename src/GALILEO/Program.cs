using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Republic.Magic.Core;
using Republic.Magic.Extensions;

namespace Republic.Magic
{
    internal class Program
    {
        internal static Stopwatch Stopwatch = Stopwatch.StartNew();

        internal static void Main() => StartAsync().Wait();

        internal static async Task StartAsync()
        {
            Console.Title = $"Republic Clash Server - ©Republic Of Clashers";
            //NativeCalls.SetWindowLong(NativeCalls.GetConsoleWindow(), -20, (int) NativeCalls.GetWindowLong(NativeCalls.GetConsoleWindow(), -20) ^ 0x80000);
            //NativeCalls.SetLayeredWindowAttributes(NativeCalls.GetConsoleWindow(), 0, 217, 0x2);

            Console.SetOut(new Prefixed());

            Console.ForegroundColor = Utils.ChooseRandomColor();
        
            Console.WriteLine(@"__________                   ___.   .__  .__         ________   _____  _________ .__                .__                  ");
            Console.WriteLine(@"\______   \ ____ ______  __ _\_ |__ |  | |__| ____   \_____  \_/ ____\ \_   ___ \|  | _____    _____|  |__   ___________ ______");
            Console.WriteLine(@" |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\   /   |   \   __\  /    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/");
            Console.WriteLine(@" |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___  /    |    \  |    \     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \ ");
            Console.WriteLine(@" |____|_  /\___  >   __/|____/|___  /____/__|\___  > \_______  /__|     \______  /____(____  /____  >___|  /\___  >__|  /____  >");
            Console.WriteLine(@"        \/     \/|__|             \/             \/          \/                \/          \/     \/     \/     \/           \/");
            Console.WriteLine(@"                                                                                                   Developer Edition  ");

            Console.ResetColor();

            /*Console.ForegroundColor = Utils.ChooseRandomColor();
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Initialized Flux capacitor v6.69");
            Console.WriteLine("Loading hash tables to run flux capacitor v6.69 to connect with the International Space Station.");
            Console.WriteLine("Connecting to custom MongoDB v5.2.0 server powered by nuclear energy (Uranium & Plutonium).");
            Console.WriteLine("Hacking into the NSA to get CSV tables.");
            Console.WriteLine("Hacking into Supercell's Amazon Web Service to extract the latest keys.");
            Console.WriteLine("Server is ready to start sending nuclear warheads into space and handle Clash Of Clans connections.");
            Console.ResetColor();
            Console.WriteLine(Environment.NewLine);*/

            Console.WriteLine(@"Republic Of Clashers's programs are protected by our policies, available on our main website.");
            Console.WriteLine(@"Republic Of Clashers's programs are under the 'CC Non-Commercial-NoDerivs 3.0 Unported' license.");
            Console.WriteLine(@"Republic Of Clashers is NOT affiliated to 'Supercell Oy'.");
            Console.WriteLine(@"Republic Of Clashers does NOT own 'Clash of Clans', 'Boom Beach', 'Clash Royale'.");

            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + Environment.NewLine);

            Resources.Initialize();

            Console.WriteLine(@"-------------------------------------" + Environment.NewLine);

            await Task.Delay(-1);

        }
    }
}
