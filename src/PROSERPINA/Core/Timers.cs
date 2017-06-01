using System.Threading.Tasks;
using BL.Servers.CR.Logic.Enums;

namespace BL.Servers.CR.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Timers;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Extensions;
    using BL.Servers.CR.Logic.Manager;
    using BL.Servers.CR.Packets.Messages.Server;
    using BL.Servers.CR.Core.Network;

    internal class Timers
    {
        internal readonly Dictionary<int, Timer> LTimers = new Dictionary<int, Timer>();

        internal Timers()
        {
            this.Save();
            this.DeadSockets();
            this.Random();
            this.Run();
        }
        internal void Maintenance(int durations)
        {
            foreach (var _Device in Resources.Players.Values.ToList())
            {
                if (_Device.Device != null)
                {
                    new Shutdown_Started(_Device.Device).Send();
                }
            }

            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds,
                AutoReset = false,
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                foreach (var _Device in Resources.Devices.Values.ToList())
                {
                    Resources.Gateway.Disconnect(_Device.Token.Args);
                }

                Constants.Maintenance_Timer = new Maintenance_Timer();
                Constants.Maintenance_Timer.StartTimer(DateTime.Now, (int)TimeSpan.FromMinutes(durations).TotalSeconds);

                Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Entered Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                Console.WriteLine("# ----------------------------------- #");
                Console.WriteLine("# Maintanance Duration    # " + Utils.Padding(durations.ToString()) + " #");
                Console.WriteLine("# Maintanance End Time    # " + Utils.Padding(Constants.Maintenance_Timer.GetEndTime.ToString("T")) + " #");
                Console.WriteLine("# ----------------------------------- #");
                Timer Timer2 = new Timer
                {
                    Interval = TimeSpan.FromSeconds(Constants.Maintenance_Timer.GetRemainingSeconds(DateTime.Now)).TotalMilliseconds,
                    AutoReset = false
                };
                Timer2.Start();
                this.LTimers.Add(5, Timer2);

                Timer2.Elapsed += (_Sender2, _Args2) =>
                {
                    Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Exited from Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                    Constants.Maintenance_Timer = null;
                    Timer2.Stop();
                    this.LTimers.Remove(4);
                    this.LTimers.Remove(5);
                };
            };

            this.LTimers.Add(4, Timer);
        }

        internal void Random()
        {
            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromHours(1).TotalMilliseconds,
                AutoReset = true
            };
            Timer.Elapsed += (_Sender, _Args) =>
            {
                Resources.Random = new Random(DateTime.Now.ToString().GetHashCode());
            };
            this.LTimers.Add(3, Timer);
        }

        internal void Save()
        {
            Timer Timer = new Timer
            {
                Interval = 60000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                Debug.WriteLine("[DATABASE] Executed at " + DateTime.Now.ToString("T") + ".");

                try
                {
                    lock (Resources.Players.Gate)
                    {
                        if (Resources.Players.Count > 0)
                        {
                            List<Player> Players = Resources.Players.Values.ToList();

                            Parallel.ForEach(Players, (_Player) =>
                            {
                                if (_Player != null)
                                {
                                    Resources.Players.Save(_Player, Constants.Database);
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
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return;
                }

                Debug.WriteLine("[DATABASE] Finished at " + DateTime.Now.ToString("T") + ".");
            };

            this.LTimers.Add(1, Timer);
        }
        internal void DeadSockets()
        {
            Timer Timer = new Timer
            {
                Interval = 30000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                List<Device> DeadSockets = new List<Device>();

                Debug.WriteLine("[SOCKET] Executed at " + DateTime.Now.ToString("T") + ".");

                foreach (Device Device in Resources.Devices.Values.ToList())
                {
                    if (!Device.Connected())
                    {
                        DeadSockets.Add(Device);
                    }
                }

                Debug.WriteLine("[SOCKET] Added " + DeadSockets.Count + " devices to the list!");

                foreach (Device Device in DeadSockets)
                {
                    Resources.Gateway.Disconnect(Device.Token.Args);
                }

                Debug.WriteLine("[SOCKET] Finished at " + DateTime.Now.ToString("T") + ".");
            };

            this.LTimers.Add(2, Timer);
        }

        internal void Run()
        {
            foreach (Timer Timer in this.LTimers.Values)
            {
                Timer.Start();
            }
        }
    }
}