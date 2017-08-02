using System;
using System.Reflection;
using CRepublic.Magic.Extensions;

namespace CRepublic.Magic.Core.Interface
{
    internal class Control
    {
        private static readonly object s_lock = new object();

        internal static void Title()
        {
            Console.Title = Constants.Title += "ONLINE | Players > ";
        }

        internal static void Hi()
        {
            Title();

            SayAscii(@"_________ .__                .__                          __________                   ___.   .__  .__        ");
            SayAscii(@"\_   ___ \|  | _____    _____|  |__   ___________  ______ \______   \ ____ ______  __ _\_ |__ |  | |__| ____  ");
            SayAscii(@"/    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/  |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\ ");
            SayAscii(@"\     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \   |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___ ");
            SayAscii(@" \______  /____(____  /____  >___|  /\___  >__|  /____  >  |____|_  /\___  >   __/|____/|___  /____/__|\___  >");
            SayAscii(@"        \/          \/     \/     \/     \/           \/          \/     \/|__|             \/             \/ ");
            SayAscii(@"                                                                                  Savage Development Edition  ");

            Say(Constants.UseRC4 ? "Crypto: RC4" : "Crypto: Pepper");

            Say(@"Clashers Republic's programs are protected by our policies, available only to our partner.");
            Say(@"Clashers Republic's programs are under the 'Proprietary' license.");
            Say(@"Clashers Republic is NOT affiliated to 'Supercell Oy'.");
            Say(@"Clashers Republic does NOT own 'Clash of Clans', 'Boom Beach', 'Clash Royale'.");

            Say(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + Environment.NewLine);
        }

        internal static void SayAscii(string message)
        {
            lock (s_lock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                Console.WriteLine(message);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                Console.ResetColor();
            }
        }

        internal static void Say(string message)
        {
            lock (s_lock)
            {
                Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                Console.WriteLine(message);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            }
        }

        internal static void SayInfo(string message)
        {
            lock (s_lock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);

                Console.WriteLine(message);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                Console.ResetColor();
            }
        }

        internal static void Say()
        {
            lock (s_lock)
            {
                Console.WriteLine();
            }
        }

        internal static void Error(string message)
        {
            lock (s_lock)
            {
                var text = "[ERROR]  " + message;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ResetColor();

                //s_errWriter.WriteLine(text);
            }
        }
    }
}
