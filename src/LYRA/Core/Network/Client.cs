using System;
using System.Net;
using System.Net.Sockets;

namespace BL.Networking.Lyra.Core.Network
{
    internal class Client
    {
        internal Socket ClientSocket, ServerSocket;
        internal readonly string Host = Constants.Host;

        public Client()
        {
            
        }

        internal Client(Socket s)
        {
            this.ClientSocket = s;
        }

        public void Enqueue()
        {
            Console.WriteLine($"Connecting to {this.Host}..");

            IPHostEntry HostInfo = Dns.GetHostEntry(this.Host);
            IPAddress Address = HostInfo.AddressList[0];

            IPEndPoint Endpoint = new IPEndPoint(Address, 9339);

            this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ServerSocket.Connect(Endpoint);

            new SendReceiveThread(this.ClientSocket, this.ServerSocket);
        }

        public void Dequeue()
        {
            this.ClientSocket.Disconnect(false);
            this.ServerSocket.Disconnect(false);

            this.ClientSocket = null;
            this.ServerSocket = null;
        }
    }
}
