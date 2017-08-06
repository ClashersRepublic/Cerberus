using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;
using CRepublic.Magic.Packets.Messages.Server.Errors;
using Timer = System.Timers.Timer;

namespace CRepublic.Magic.Core
{
    internal class Timers
    {
        internal readonly Dictionary<int, Timer> LTimers = new Dictionary<int, Timer>();

        internal Timers()
        {
            this.Save();
            this.KeepAlive();
            this.Random();
            this.Run();
        }

        internal void Maintenance(int durations)
        {
            foreach (var _Device in Players.Levels.Values.ToList())
            {
                if (_Device.Device != null)
                {
                    new Server_Shutdown(_Device.Device).Send();
                }
            }

            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
                AutoReset = false,
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {   
                foreach (var _Device in Devices._Devices.Values.ToList())
                {
                    Devices.Remove(_Device.SocketHandle);
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
                    await Task.WhenAll(Players.Save(), Resources.Clans.Save(), Resources.Battles.Save()).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Exceptions.Log(ex,
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
                foreach (Device Device in Devices._Devices.Values.ToList())
                {
                    //if (!Device.Connected())
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
                    Devices.Remove(Device.SocketHandle);
                }


#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : DeadSocket finished at " + DateTime.Now.ToString("T") +
                    ".", true);

#endif
            };

            this.LTimers.Add(2, Timer);
        }

        internal void KeepAlive()
        {
            Timer Timer = new Timer
            {
                Interval = 60000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                var numDisc = 0;
#if DEBUG
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + " : KeepAlive executed at " + DateTime.Now.ToString("T") +
                    ".", true);
#endif
                foreach (Device Device in Devices._Devices.Values.ToList())
                {
                    if (DateTime.Now > Device.NextKeepAlive)
                    {
                        Devices.Remove(Device.SocketHandle);
                        numDisc++;
                    }
                }
#if DEBUG
                if (numDisc > 0)
                Loggers.Log(
                    Utils.Padding(this.GetType().Name, 6) + $" : KeepAlive dropped {numDisc} clients due to keep alive timeouts at " + DateTime.Now.ToString("T") +
                    ".", true);
#endif
                Loggers.Log("#" + DateTime.Now.ToString("d") + " ---- Pools ---- " + DateTime.Now.ToString("T") + " #", true);
                Loggers.Log($"SocketAsyncEventArgs: created -> {Gateway.NumberOfArgsCreated} in-use -> {Gateway.NumberOfArgsInUse} available -> {Gateway.NumberOfArgs}.", true);
                Loggers.Log($"Buffers: created -> {Gateway.NumberOfBuffersCreated} in-use -> {Gateway.NumberOfBuffersInUse} available -> {Gateway.NumberOfBuffers}.", true);
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