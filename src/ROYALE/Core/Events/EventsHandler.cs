
using System.Resources;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Database;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;

namespace CRepublic.Royale.Core.Events
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

        internal delegate void EventHandler(Logic.Enums.Exit_Options Type = Logic.Enums.Exit_Options.CTRL_CLOSE_EVENT);

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
                List<Logic.Player> Players = Server_Resources.Players.Values.ToList();

                lock (Server_Resources.Players.Gate)
                {
                    if (Server_Resources.Players.Count > 0)
                    {
                        Parallel.ForEach(Players, (_Player) =>
                        {
                            if (_Player != null)
                            {
                                Server_Resources.Players.Remove(_Player);
                                Redis.Players.KeyDeleteAsync(_Player.UserId.ToString());
                            }
                        });
                    }
                }

                List<Logic.Clan> Clans = Server_Resources.Clans.Values.ToList();

                lock (Server_Resources.Clans.Gate)
                {
                    if (Server_Resources.Clans.Count > 0)
                    {
                        Parallel.ForEach(Clans, (_Clan) =>
                        {
                            if (_Clan != null)
                            {
                                Server_Resources.Clans.Remove(_Clan);
                                Redis.Players.KeyDeleteAsync(_Clan.ClanID.ToString());
                            }
                        });
                    }
                }

                Thread.Sleep(30000);
            }
            catch (Exception Exception)
            {
                Server_Resources.Exceptions.Catch(Exception);
            }
        }

        internal void Handler(Logic.Enums.Exit_Options Type = Logic.Enums.Exit_Options.CTRL_CLOSE_EVENT)
        {
            Console.WriteLine("The program is closing");
            this.ExitHandler();
        }
    }
}