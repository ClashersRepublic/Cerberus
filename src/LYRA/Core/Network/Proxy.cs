using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BL.Networking.Lyra.Crypto;

namespace BL.Networking.Lyra.Core.Network
{
    internal class Proxy
    {
        internal Socket ClientListener, ClientSocket;
        internal IPEndPoint Endpoint;
        internal Client Client;
        internal Thread AcceptThread;
        internal List<Client> Clients = new List<Client>();     

        internal Proxy()
        {
            Keys.SetKey();

            this.Endpoint = new IPEndPoint(IPAddress.Any, Constants.Port);

            this.ClientListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.ClientListener.Bind(Endpoint);
            this.ClientListener.Listen(Constants.Backlog);

            this.AcceptThread = new Thread(() =>
            {
                while (true)
                {
                    this.ClientSocket = this.ClientListener.Accept();
                    this.Client = new Client(this.ClientSocket);

                    this.Clients.Add(this.Client);

                    Console.WriteLine($"A new client has connected ({((IPEndPoint) this.ClientSocket.RemoteEndPoint).Address}) Enqueing..");

                    this.Client.Enqueue();
                }
            });

            this.AcceptThread.Start();

            Console.WriteLine("BarbarianLand Proxy has started on port 9339! Waiting for connections..");
        }

        internal void ClearQueue()
        {
            for (int i = 0; i < this.Clients.Count; i++)
            {
                this.Clients[i].Dequeue();
            }

            this.Clients.Clear();
        }
    }
}
