using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Packets.Messages.Server.Battle;
using BL.Servers.CR.Packets.Messages.Server.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Servers.CR.Core.Events
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
                            Console.WriteLine("#" + DateTime.Now.ToString("d") + " ---- STATS ---- " + DateTime.Now.ToString("T") + " #");
                            Console.WriteLine("# ----------------------------------- #");
                            Console.WriteLine("# In-Memory Players  # " + Utils.Padding(Resources.Players.Count + " - " + Constants.MaxPlayers, 14) + " #");
                            Console.WriteLine("# In-Memory Waiting  # " + Utils.Padding(Resources.Battles.Waiting.Count.ToString(), 14) + " #");
                            Console.WriteLine("# In-Memory SAEA     # " + Utils.Padding(Resources.Gateway.ReadPool.Pool.Count + " - " + Resources.Gateway.WritePool.Pool.Count, 14) + " #");
                            Console.WriteLine("# ----------------------------------- #");
                            break;
                        }

                    case ConsoleKey.M:
                        {
                            if (Constants.Maintenance_Timer == null)
                            {
                                Console.WriteLine("Press Y to continue and N to cancel");
                                ConsoleKeyInfo Command2 = Console.ReadKey(false);

                                switch (Command2.Key)
                                {
                                    case ConsoleKey.Y:
                                        {
                                            Console.WriteLine("Please enter the duration of Maintenance (Enter value in int only).");
                                            Console.WriteLine("(Minute): ");

                                            if (Int32.TryParse(Console.ReadLine(), out int i))
                                            {
                                                Resources.Classes.Checker.Maintenance(i);
                                                Resources.Classes.Checker.LTimers[4].Start();
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
                                            Console.WriteLine("Press Y to continue and N to cancel");
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Server is already in Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                            }
                            break;
                        }

                    case ConsoleKey.D:
                        {
                            if (Constants.Maintenance_Timer != null)
                            {
                                Constants.Maintenance_Timer = null;

                                Resources.Classes.Checker.LTimers[4].Stop();
                                Resources.Classes.Checker.LTimers[5].Stop();
                                Resources.Classes.Checker.LTimers.Remove(4);
                                Resources.Classes.Checker.LTimers.Remove(5);

                                Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Exited Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                            }
                            else
                            {
                                Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Not in Maintanance Mode---- " + DateTime.Now.ToString("T") + " #");
                            }
                            break;
                        }

                    case ConsoleKey.E:
                        {
                            Environment.Exit(0);
                            break;
                        }

                    case ConsoleKey.H:
                        {
                            Console.WriteLine();
                            Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- HELPS ---- " + DateTime.Now.ToString("T") + " #");
                            Console.WriteLine("# ----------------------------------- #");
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
                                Resources.Gateway.Disconnect(_Device.Token.Args);
                            }
                            break;
                        }

                    case ConsoleKey.B:
                        {
                            foreach (var _Device in Resources.Devices.Values.ToList())
                            {
                                if (_Device.Player != null && _Device.PlayerState == Logic.Enums.State.IN_BATTLE)
                                {
                                    new Battle_End(_Device.Player.Device).Send();

                                    Resources.Battles.Remove(_Device.Player.BattleID);

                                    break;
                                }
                                break;
                            }
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
                        Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Entered Bleeding Edge Mode ---- " + DateTime.Now.ToString("T") + " #");
                        break;

                }
            }
        }
    }
}
