using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Republic.Magic.Core.Database;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure;
using Republic.Magic.Packets.Messages.Server.Errors;
using Battle = Republic.Magic.Logic.Battle;
using Clan = Republic.Magic.Logic.Clan;
using Timer = System.Timers.Timer;

namespace Republic.Magic.Core
{
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
                if (_Device.Client != null)
                {
                    new Server_Shutdown(_Device.Client).Send();
                }
            }

            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
                AutoReset = false,
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {   
                foreach (var _Device in Resources.Devices.Values.ToList())
                {
                    Resources.Gateway.Disconnect(_Device.Token.Args);
                }

                Constants.Maintenance = new Maintenance_Timer();
                Constants.Maintenance.StartTimer(DateTime.Now, (int)TimeSpan.FromMinutes(durations).TotalSeconds);
                Timer Timer2 = new Timer
                {
                    Interval = (int)TimeSpan.FromMinutes(durations).TotalMilliseconds,
                    AutoReset = false
                };

                Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Entered Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                Console.WriteLine("# ----------------------------------- #");
                Console.WriteLine("# Maintanance Duration    # " + Utils.Padding(durations.ToString()) + " #");
                Console.WriteLine("# Maintanance End Time    # " + Utils.Padding(Constants.Maintenance.GetEndTime.ToString("T")) + " #");
                Console.WriteLine("# ----------------------------------- #");
                Timer2.Start();

                Timer2.Elapsed += (_Sender2, _Args2) =>
                {
                    Constants.Maintenance = null;
                    this.LTimers.Remove(4);
                    this.LTimers.Remove(5);
                    Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Exited from Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                };
                this.LTimers.Add(5, Timer2);

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
                Resources.Random = new Random(DateTime.Now.ToString("T").GetHashCode());
            };
            this.LTimers.Add(3, Timer);
        }

        internal void Save()
        {
            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(3).TotalMilliseconds,
                AutoReset = true
            };

            Timer.Elapsed += async (_Sender, _Args) =>
            {
#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : Save executed at " + DateTime.Now.ToString("T") + ".",
                    true);
#endif
                try
                {

                    if (Resources.Players.Count > 0)
                    {
                        await Resources.Players.Save(Constants.Database);
                    }
                    if (Resources.Clans.Count > 0)
                    {
                        await Resources.Clans.Save(Constants.Database);
                    }

                    if (Resources.Battles.Count > 0)
                    {
                        await Resources.Battles.Save(Constants.Database);
                    }
                }
                catch (Exception ex)
                {
                    Resources.Exceptions.Catch(ex,
                        "[: Failed at " + DateTime.Now.ToString("T") + ']' + Environment.NewLine + ex.StackTrace);
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

            this.LTimers.Add(1 ,Timer);
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
                Loggers.Log(Utils.Padding(this.GetType().Name, 6) + " : DeadSocket executed at " + DateTime.Now.ToString("T") + ".", true);
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