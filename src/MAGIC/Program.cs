using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;

namespace CRepublic.Magic
{
    internal class Program
    {
        internal static Stopwatch Stopwatch = Stopwatch.StartNew();

        internal static void Main() => StartAsync().Wait();

        internal static async Task StartAsync()
        {
            Console.Title = $"Clashers Republic Clash Server - ©Clashers Repbulic";
            //NativeCalls.SetWindowLong(NativeCalls.GetConsoleWindow(), -20, (int) NativeCalls.GetWindowLong(NativeCalls.GetConsoleWindow(), -20) ^ 0x80000);
            //NativeCalls.SetLayeredWindowAttributes(NativeCalls.GetConsoleWindow(), 0, 217, 0x2);

            Console.SetOut(new Prefixed());

            Console.WriteLine(@"_________ .__                .__                          __________                   ___.   .__  .__        ");
            Console.WriteLine(@"\_   ___ \|  | _____    _____|  |__   ___________  ______ \______   \ ____ ______  __ _\_ |__ |  | |__| ____  ");
            Console.WriteLine(@"/    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/  |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\ ");
            Console.WriteLine(@"\     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \   |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___ ");
            Console.WriteLine(@" \______  /____(____  /____  >___|  /\___  >__|  /____  >  |____|_  /\___  >   __/|____/|___  /____/__|\___  >");
            Console.WriteLine(@"        \/          \/     \/     \/     \/           \/          \/     \/|__|             \/             \/ ");
            Console.WriteLine(@"                                                                                        Developer Edition  ");


            Console.WriteLine(@"Clashers Repbulic's programs are protected by our policies, available on our main website.");
            Console.WriteLine(@"Clashers Repbulic's programs are under the 'CC Non-Commercial-NoDerivs 3.0 Unported' license.");
            Console.WriteLine(@"Clashers Repbulic is NOT affiliated to 'Supercell Oy'.");
            Console.WriteLine(@"Clashers Repbulic does NOT own 'Clash of Clans', 'Boom Beach', 'Clash Royale'.");

            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + System.Environment.NewLine);

            Resources.Initialize();

            Console.WriteLine(@"-------------------------------------" + System.Environment.NewLine);

            await Task.Delay(-1);

        }
    }
}
