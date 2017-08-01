using System;
using System.Diagnostics;
using System.Threading;
using static System.Console;
using static Magic.ClashOfClans.Core.Logger;

namespace Magic.ClashOfClans.Core.Settings
{
    internal class MagicControl
    {
        public static void UpdateTitle(bool Status)
        {
            if (Status == false)
            {
                Console.Title = Constants.DefaultTitle + "OFFLINE";
            }
            else if (Status == true)
            {
                Constants.DefaultTitle = Constants.DefaultTitle + "ONLINE | Players > ";
                Console.Title = Constants.DefaultTitle;
            }
        }

        public static void WelcomeMessage()
        {
            UpdateTitle(true);
            Say();
            SayAscii(@"                       .__              .__ __________                   __               __          ");
            SayAscii(@"  _____ _____     ____ |__| ____ _____  |  |\______   \_______  ____    |__| ____   _____/  |_  ______");
            SayAscii(@" /     \\__  \   / ___\|  |/ ___\\__  \ |  | |     ___/\_  __ \/  _ \   |  |/ __ \_/ ___\   __\/  ___/");
            SayAscii(@"|  Y Y  \/ __ \_/ /_/  >  \  \___ / __ \|  |_|    |     |  | \(  <_> )  |  \  ___/\  \___|  |  \___ \ ");
            SayAscii(@"|__|_|  (____  /\___  /|__|\___  >____  /____/____|     |__|   \____/\__|  |\___  >\___  >__| /____  >");
            SayAscii(@"      \/     \//_____/         \/     \/    Clash of Clans Server    \______|    \/     \/          \/ ");
            SayAscii(@"___________________________________________________________________________________________________");
            SayAscii(@"            ");

            if (Constants.IsRc4)
                SayInfo("Crypto: RC4");
            else
                SayInfo("Crypto: Pepper");

            Say();
        }
    }
}