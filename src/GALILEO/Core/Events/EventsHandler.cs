using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Core.Events
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
                lock (Resources.Players.Gate)
                {
                    if (Resources.Players.Count > 0)
                    {
                        List<Level> Players = Resources.Players.Values.ToList();

                        Parallel.ForEach(Players, (_Player) =>
                        {
                            if (_Player != null)
                            {
                                Resources.Players.Save(_Player, Constants.Database);
                                //Resources.Players.Remove(_Player); //Let's not waste resource to delete them,Save only should be ok
                                //Redis.Players.KeyDelete(_Player.LowID.ToString());
                            }
                        });
                    }
                }

                lock (Resources.Clans.Gate)
                {
                    if (Resources.Clans.Count > 0)
                    {
                        List<Clan> Clans = Resources.Clans.Values.ToList();

                        foreach (Clan _Clan in Clans)
                        {
                            if (_Clan != null)
                            {
                                Resources.Clans.Save(_Clan, Constants.Database);
                                //Redis.Clans.KeyDelete(_Clan.LowID.ToString());
                            }
                        }
                    }
                }

                Thread.Sleep((int)TimeSpan.FromSeconds(0.5).TotalMilliseconds);
            }
            catch (Exception)
            {
                Console.WriteLine("Mmh, something happen when we tried to save everything.");
            }
        }
        internal void Handler(Logic.Enums.Exits Type = Logic.Enums.Exits.CTRL_CLOSE_EVENT)
        {
            Console.WriteLine("The program is shutting down.");
            this.ExitHandler();
        }
    }
}
