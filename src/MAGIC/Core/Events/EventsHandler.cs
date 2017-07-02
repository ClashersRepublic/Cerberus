using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic.Enums;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CRepublic.Magic.Core.Events
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

        internal async void ExitHandler()
        {
            try
            {
                Task.WaitAll(Resources.Players.Save(Constants.Database), Resources.Clans.Save(Constants.Database), Resources.Battles.Save(Constants.Database));
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
