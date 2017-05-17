using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BL.Networking.Lyra.Core;
using BL.Networking.Lyra.Extensions;

namespace BL.Networking.Lyra
{
    internal class Program
    {
        internal static void Main()
        {
#if DEBUG
            Console.Title = $"[DEBUG] BarbarianLand Proxy - © BarbarianLand Development";
#else
            Console.Title = $"BarbarianLand Proxy - © BarbarianLand Development";
#endif

            Console.SetOut(new Prefixed());
            Console.ForegroundColor = Utils.ChooseRandomColor();

            Console.WriteLine(@"__________             ___.                .__                     .____                       .___");
            Console.WriteLine(@"\______   \_____ ______\_ |__ _____ _______|__|____    ____   _____|    |   _____    ____    __| _/");
            Console.WriteLine(@" |    |  _/\__  \\_  __ \ __ \\__  \\_  __ \  \__  \  /    \ /  ___/    |   \__  \  /    \  / __ | ");
            Console.WriteLine(@" |    |   \ / __ \|  | \/ \_\ \/ __ \|  | \/  |/ __ \|   |  \\___ \|    |___ / __ \|   |  \/ /_/ | ");
            Console.WriteLine(@" |______  /(____  /__|  |___  (____  /__|  |__(____  /___|  /____  >_______ (____  /___|  /\____ | ");
            Console.WriteLine(@"        \/      \/          \/     \/              \/     \/     \/        \/    \/     \/      \/ ");
            Console.WriteLine(@"                                                                                             V1.8.2");

            Console.ResetColor();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine(@"BarbarianLand's programs are protected by our policies, available on our main website.");
            Console.WriteLine(@"BarbarianLand's programs are under the 'CC Non-Commercial-NoDerivs 3.0 Unported' license.");
            Console.WriteLine(@"BarbarianLand is NOT affiliated to 'Supercell Oy'.");
            Console.WriteLine(@"BarbarianLand does NOT own 'Clash of Clans', 'Boom Beach', 'Clash Royale', 'Hay Day', and 'GunShine'.");

            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + Environment.NewLine);

            Resources.Initialize();

            Console.WriteLine(@"-------------------------------------" + Environment.NewLine);
            while (true) ;
        }
    }
}
