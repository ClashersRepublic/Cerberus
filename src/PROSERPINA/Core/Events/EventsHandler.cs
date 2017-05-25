
using System.Resources;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Database;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Core.Events
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
            EHandler += this.Handler;
            SetConsoleCtrlHandler(EventsHandler.EHandler, true);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        internal void ExitHandler()
        {
            Console.WriteLine("The Server is currently saving all players and clans, before shutting down.");

            try
            {
                List<Logic.Player> Players = Resources.Players.Values.ToList();

                lock (Resources.Players.Gate)
                {
                    if (Resources.Players.Count > 0)
                    {
                        Parallel.ForEach(Players, (_Player) =>
                        {
                            if (_Player != null)
                            {
                                Resources.Players.Remove(_Player);
                                Redis.Players.KeyDelete(_Player.UserId.ToString());
                            }
                        });
                    }
                }

                Thread.Sleep(30000);
            }
            catch
            {
                Console.WriteLine("There was an exception while saving players and clans.");
            }
        }

        internal void Handler(Logic.Enums.Exits Type = Logic.Enums.Exits.CTRL_CLOSE_EVENT)
        {
            Console.WriteLine("The program is closing");
            this.ExitHandler();
        }
    }
}