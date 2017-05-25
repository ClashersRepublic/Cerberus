using System;
using System.Linq;
using System.Threading;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Packets.Messages.Server.Errors;

namespace BL.Servers.CoC.Core.Events
{
    internal class Parser
    {
        internal Thread Thread;
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
                    // Status
                    case ConsoleKey.S:
                    {
                        Console.WriteLine();
                        Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- STATS ---- " + DateTime.Now.ToString("T") + " #");
                        Console.WriteLine("# ----------------------------------- #");
                        Console.WriteLine("# Online Players    # " + Utils.Padding(Resources.Players.Count + " - " + Constants.MaxPlayers, 15) + " #");
                        Console.WriteLine("# Battles           # " + Utils.Padding(Resources.Battles.Seed.ToString(), 15) + " #");
                        Console.WriteLine("# In-Memory SAEA    # " + Utils.Padding(Resources.Gateway.ReadPool.Pool.Count + " - " + Resources.Gateway.WritePool.Pool.Count, 15) + " #");
                        Console.WriteLine("# ----------------------------------- #");
                        break;
                    }

                    // Emergency Close
                    case ConsoleKey.E:
                    {
                        //Environment.Exit(0);
                        break;
                    }

                    case ConsoleKey.H:
                    {
                        Console.WriteLine();
                        Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- HELPS ---- " +
                                          DateTime.Now.ToString("T") + " #");
                        Console.WriteLine("# ----------------------------------- #");
                        Console.WriteLine("# ----------------------------------- #");
                        break;
                    }

                    case ConsoleKey.R:
                    {
                        foreach(var _Device in Resources.Devices.Values.ToList())
                        {
                            if (_Device.Player != null)
                            {
                                new Out_Of_Sync(_Device).Send();
                            }
                            Resources.Gateway.Disconnect(_Device.Token.Args);
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

                    default:
                    {
                        Console.WriteLine();
                        Console.WriteLine("Press H for help");
                        break;
                    }
                }
            }
        }
    }
}