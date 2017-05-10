using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace BL.Proxy.Lyra
{
    class Proxy
    {
        private static Thread AcceptThread; 
        public static List<Client> ClientPool = new List<Client>();
        public const int Backlog = 100;
        public const int Port = 9339;

        /// <summary>
        /// Starts the proxy
        /// </summary>
        public static void Start()
        {
            try
            {
                // Check dirs
                Helper.CheckDirectories();

                // ASCII art
                Console.ForegroundColor = Helper.ChooseRandomColor();
                Logger.CenterString(@"  _____                                 ____ ");
                Logger.CenterString(@" / ___/__  ______  ___  _____________  / / / ");
                Logger.CenterString(@" \__ \/ / / / __ \/ _ \/ ___/ ___/ _ \/ / /  ");
                Logger.CenterString(@"  _/ / /_/ / /_/ /  __/ /  / /__/  __/ / /   ");
                Logger.CenterString(@"/____/\__,_/ .___/\___/_/   \___/\___/_/_/   ");
                Logger.CenterString(@"          /_/____                            ");
                Logger.CenterString(@"            / __ \_________  _  ____  __      ");
                Logger.CenterString(@"           / /_/ / ___/ __ \| |/_/ / / /      ");
                Logger.CenterString(@"          / ____/ /  / /_/ />  </ /_/ /      ");
                Logger.CenterString(@"         /_/   /_/   \____/_/|_|\__, /       ");
                Logger.CenterString(@"                               /____/        ");
                Logger.CenterString(@"                                             ");
                Logger.CenterString(Helper.AssemblyVersion);
                Logger.CenterString("Coded by expl0itr");
                Logger.CenterString("Apache Version 2.0 License - © 2016");
                Logger.CenterString("https://github.com/expl0itr/BL.Proxy.Lyra/");
                Console.Write(Environment.NewLine);
                Console.ResetColor();

                // Show configuration values
                Console.Write(Environment.NewLine);
                Logger.CenterString("===============================");
                Logger.CenterString(Config.Game.ReadableName());
                Logger.CenterString(Config.Host);
                Logger.CenterString("Local IP: " + Helper.LocalNetworkIP);
                Logger.CenterString("===============================");
                Console.Write(Environment.NewLine);

                // Set latest public key
                Keys.SetPublicKey();
               
                // Bind a new socket to the local EP     
                IPEndPoint EP = new IPEndPoint(IPAddress.Any, Port);
                Socket ClientListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ClientListener.Bind(EP);
                ClientListener.Listen(Backlog);

                // Initialize the JSON Packets
                JSONPacketManager.LoadDefinitions();

                // Listen for connections
                AcceptThread = new Thread(() =>
                {
                    while (true)
                    {
                        Socket ClientSocket = ClientListener.Accept();
                        Client c = new Client(ClientSocket);
                        ClientPool.Add(c);
                        Logger.Log("A client connected (" + ClientSocket.GetIP() + ")! Enqueuing..");
                        c.Enqueue();
                    }
                });
                AcceptThread.Start();
                Logger.Log("Proxy started. Waiting for incoming connections..");
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to start the proxy (" + ex.GetType() + ")!");
            }
        }

        /// <summary>
        /// Stops the proxy
        /// </summary>
        public static void Stop()
        {
            for (int i = 0; i < ClientPool.Count; i++)
            {
                ClientPool[i].Dequeue();
            }
            ClientPool.Clear();
        }
    }
}
