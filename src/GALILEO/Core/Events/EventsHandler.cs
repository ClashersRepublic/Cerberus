using Republic.Magic.Extensions;
using Republic.Magic.Logic.Enums;
using System;
using System.Runtime.InteropServices;

namespace Republic.Magic.Core.Events
{
    internal class EventsHandler
    {
        internal static EventHandler EHandler;
        internal delegate void EventHandler(Exits Type = Exits.CTRL_CLOSE_EVENT);
        internal EventsHandler()
        {
            EventsHandler.EHandler += this.Handler;
            EventsHandler.SetConsoleCtrlHandler(EventsHandler.EHandler, true);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        internal void ExitHandler()
        {
            try
            {
                if (Resources.Players.Count > 0)
                {
                    lock (Resources.Players.Gate)
                    {
                        Resources.Players.Save(Constants.Database).Wait();
                    }
                }

                if (Resources.Clans.Count > 0)
                {
                    lock (Resources.Clans.Gate)
                    {
                        Resources.Clans.Save(Constants.Database).Wait();
                    }
                }


                if (Resources.Battles.Count > 0)
                {
                    lock (Resources.Battles.Gate)
                    {
                        Resources.Battles.Save(Constants.Database).Wait();
                    }
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Mmh, something happen when we tried to save everything.");
            }
        }
        internal void Handler(Exits Type = Exits.CTRL_CLOSE_EVENT)
        {
            Console.WriteLine("The program is shutting down.");
            this.ExitHandler();
        }
    }
}
