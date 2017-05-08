using System.Threading.Tasks;
using BL.Servers.BB.Logic.Enums;

namespace BL.Servers.BB.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Timers;
    using BL.Servers.BB.Logic;
    using BL.Servers.BB.Extensions;


    internal class Timers
    {
        internal readonly List<Timer> LTimers = new List<Timer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Timers"/> class.
        /// </summary>
        internal Timers()
        {
            this.Save();
            this.DeadSockets();
            this.Collect();
            this.Run();
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
#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : Save executed at " + DateTime.Now.ToString("T") + ".",
                    true);
#endif
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
                                    _Player.Tick();
                                    Resources.Players.Save(_Player);
                                }
                            });
                        }
                    }
                    /* lock (Resources.Clans.Gate)
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
                     }*/
                }
                catch (Exception ex)
                {
                    Loggers.Log(
                        Utils.Padding(ex.GetType().Name, 15) + " : " + ex.Message + ".[: Failed at " +
                        DateTime.Now.ToString("T") + ']' + Environment.NewLine + ex.StackTrace, true, Defcon.ERROR);
                    return;
                }
#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : Save finished at " + DateTime.Now.ToString("T") + ".",
                    true);

#endif
            };

            this.LTimers.Add(Timer);
        }

        internal void Collect()
        {
            Timer Timer = new Timer
            {
                Interval = 10000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                try
                {
                    GC.Collect(GC.MaxGeneration);
                    GC.WaitForPendingFinalizers();
                }
                catch 
                {
                //
                }
            };

            this.LTimers.Add(Timer);
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
#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : DeadSocket executed at " + DateTime.Now.ToString("T") +
                    ".", true);
#endif
                foreach (Device Device in Resources.Devices.Values.ToList())
                {
                    if (!Device.Connected())
                    {
                        DeadSockets.Add(Device);
                    }
                }

#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : Added " + DeadSockets.Count +
                    " devices to DeadSockets list.", true);

#endif
                foreach (Device Device in DeadSockets)
                {
                    Resources.Gateway.Disconnect(Device.Token.Args);
                }


#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : DeadSocket finished at " + DateTime.Now.ToString("T") +
                    ".", true);

#endif
            };

            this.LTimers.Add(Timer);
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        internal void Run()
        {
            foreach (Timer Timer in this.LTimers)
            {
                Timer.Start();
            }
        }
    }
}