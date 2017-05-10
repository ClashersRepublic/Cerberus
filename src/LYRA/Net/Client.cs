using System;
using System.Net;
using System.Net.Sockets;

namespace BL.Proxy.Lyra
{
    internal class Client
    {
        public Socket ClientSocket, ServerSocket;
        private readonly string Host = Config.Host;

        /// <summary>
        /// Client constructor
        /// </summary>
        public Client(Socket s)
        {
            ClientSocket = s;
        }

        public void Enqueue()
        {
            try
            {
                // Connect to the official supercell server
                Logger.Log("Connecting to " + Host + "..");
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 9339);
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Connect(remoteEndPoint);

                // Start async recv/send procedure
                Logger.Log("Starting threads..");
                Console.Write(Environment.NewLine);
                new ReceiveSendThreads(ClientSocket, ServerSocket).Run();
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to enqueue client " + ClientSocket.GetIP() + "!");
                Program.WaitAndClose();
            }
        }

        /// <summary>
        /// Dequeues the client
        /// </summary>
        public void Dequeue()
        {
            ClientSocket.Disconnect(false);
            ServerSocket.Disconnect(false);
            ClientSocket = null;
            ServerSocket = null;
        }    
    }
}
