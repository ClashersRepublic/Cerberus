
namespace BL.Servers.BB.Core.Events
{

    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using System.Linq;
    using System.Threading;

    internal class EventsHandler
    {
        internal static EventHandler EHandler;

        internal delegate void EventHandler(Logic.Enums.Exits Type = Logic.Enums.Exits.CTRL_CLOSE_EVENT);

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsHandler"/> class.
        /// </summary>
        internal EventsHandler()
        {
            EventsHandler.EHandler += this.Handler;
            EventsHandler.SetConsoleCtrlHandler(EventsHandler.EHandler, true);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        internal void ExitHandler()
        {

        }

        internal void Handler(Logic.Enums.Exits Type = Logic.Enums.Exits.CTRL_CLOSE_EVENT)
        {

        }
    }
}