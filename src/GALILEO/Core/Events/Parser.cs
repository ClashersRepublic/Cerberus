using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic.Structure;
using CRepublic.Magic.Packets.Messages.Server.Errors;
using ThreadState = System.Diagnostics.ThreadState;

namespace CRepublic.Magic.Core.Events
{
    internal class Parser
    {
        internal Thread Thread;
        internal Boolean BleeedingEdge;

        internal Parser()
        {
            this.Thread = new Thread(this.Parse)
            {
                Priority = ThreadPriority.Lowest,
                Name = this.GetType().Name
            };
            this.Thread.Start();
        }

        internal void Parse()
        {
            while (true)
            {
                    ConsoleKeyInfo Command = Console.ReadKey(false);

                switch (Command.Key)
                {
                    default:
                    {
                        Console.WriteLine();
                        Console.WriteLine("Press H for help");
                        break;
                    }

                    // Status
                    case ConsoleKey.S:
                    {
                        Console.WriteLine();
                        Console.WriteLine("#" + DateTime.Now.ToString("d") + " ---- STATS ---- " +
                                          DateTime.Now.ToString("T") + " #");
                        Console.WriteLine("# ----------------------------------- #");
                        Console.WriteLine("# In-Memory Players    # " +
                                          Utils.Padding(Resources.Players.Count.ToString(), 15) + " #");
                        Console.WriteLine("# In-Memory Battles           # " +
                                          Utils.Padding(Resources.Battles.Seed.ToString(), 15) + " #");
                        Console.WriteLine("# In-Memory SAEA    # " +
                                          Utils.Padding(Resources.Gateway.ReadPool.Pool.Count + " - " +  Resources.Gateway.WritePool.Pool.Count, 25) + " #");
                        Console.WriteLine("# ----------------------------------- #");
                        break;
                    }

                    case ConsoleKey.M:
                    {
                        if (Resources.Classes.Timers.LTimers.Count <= 3)
                        {
                            Console.WriteLine("Press Y to continue and N to cancle");
                            ConsoleKeyInfo Command2 = Console.ReadKey(false);

                            switch (Command2.Key)
                            {
                                case ConsoleKey.Y:
                                {
                                    Console.WriteLine("Please enter the duration of Maintenance (Enter value in int only).");
                                    Console.Write("(Minute): ");
                                    if (Int32.TryParse(Console.ReadLine(), out int i))
                                    {
                                        Resources.Classes.Timers.Maintenance(i);
                                        Resources.Classes.Timers.LTimers[4].Start();
                                    }
                                    else
                                        Console.WriteLine("Value is invalid, Request cancelled");
                                    break;
                                }

                                case ConsoleKey.N:
                                {
                                    Console.WriteLine("Request cancelled");
                                    break;
                                }

                                default:
                                {
                                    Console.WriteLine("Press Y to continue and N to cancle");
                                    break;
                                }
                            }
                        }
                        else
                        {

                            Console.WriteLine("# " + DateTime.Now.ToString("d") +
                                              " ---- Server is already in Maintanance Mode---- " + DateTime.Now.ToString("T") +
                                              " #");
                            }
                        break;
                    }

                    case ConsoleKey.D:
                    {
                        if (Constants.Maintenance != null)
                        {
                            Constants.Maintenance = null;
                            Resources.Classes.Timers.LTimers[4].Stop();
                            Resources.Classes.Timers.LTimers[5].Stop();
                            Resources.Classes.Timers.LTimers.Remove(4);
                            Resources.Classes.Timers.LTimers.Remove(5);
                            Console.WriteLine("# " + DateTime.Now.ToString("d") +
                                              " ---- Exited from Maintanance Mode---- " + DateTime.Now.ToString("T") +
                                              " #");
                        }
                        else
                        {
                            Console.WriteLine("# " + DateTime.Now.ToString("d") +
                                              " ---- Not in Maintanance Mode---- " + DateTime.Now.ToString("T") +
                                              " #");
                            }
                        break;
                    }

                    // Emergency Close
                    case ConsoleKey.E:
                    {
                        Environment.Exit(0);
                        break;
                    }

                    case ConsoleKey.H:
                    {
                        Console.WriteLine();
                        Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- HELPS ---- " +
                                          DateTime.Now.ToString("T") + " #");
                        Console.WriteLine("# ----------------------------------- #");
                            Console.WriteLine(((IEnumerable)Process.GetCurrentProcess().Threads)
                                    .OfType<ProcessThread>()
                                    .Count(t => t.ThreadState == ThreadState.Running));
                        Console.WriteLine("# ----------------------------------- #");
                        break;
                    }

                    case ConsoleKey.R:
                    {
                        foreach (var _Device in Resources.Devices.Values.ToList())
                        {
                            if (_Device.Player != null)
                            {
                                new Out_Of_Sync(_Device).Send();
                            }
                            //Resources.Gateway.Disconnect(_Device.Token.Args);
                        }
                        break;
                    }

                    case ConsoleKey.T:
                    {
                        break;
                    }

                    case ConsoleKey.C:
                    {
                        Console.Clear();
                        break;
                    }

                    case ConsoleKey.F10:
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Entered Bleeding Edge Mode ---- " +
                                          DateTime.Now.ToString("T") + " #");
                        break;

                }
            }
        }
    }
}