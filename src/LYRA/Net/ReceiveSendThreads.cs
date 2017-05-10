using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BL.Proxy.Lyra
{
    class ReceiveSendThreads
    {
        // Constants
        private const int HEADER_SIZE = 7;
     
        // Threads
        private Thread ClientThread, ServerThread;

        // Sockets
        private Socket ClientSocket, ServerSocket;

        // PacketHeaders
        private byte[] ClientHeader = new byte[HEADER_SIZE];
        private byte[] ServerHeader = new byte[HEADER_SIZE];

        // PacketBufs
        private byte[] ClientBuf, ServerBuf;

        public static bool Running = false;

        /// <summary>
        /// ReceiveSendThread constructor
        /// </summary>
        public ReceiveSendThreads(Socket ClientSocket, Socket ServerSocket)
        {
            this.ClientSocket = ClientSocket;
            this.ServerSocket = ServerSocket;
        }

        /// <summary>
        /// Starts both threads
        /// </summary>
        public void Run()
        {
            ClientThread = new Thread(() =>
            {
                while (ClientSocket.Receive(ClientHeader, 0, ClientHeader.Length, SocketFlags.None) != 0)
                {
                    // Parse Packet Length
                    var tmp = ClientHeader.Skip(2).Take(3).ToArray();
                    var PacketLength = ((0x00 << 24) | (tmp[0] << 16) | (tmp[1] << 8) | tmp[2]);

                    // Initialize Client Buffer
                    ClientBuf = new byte[PacketLength + HEADER_SIZE];

                    // Apply header
                    for (int i = 0; i < HEADER_SIZE; i++)
                        ClientBuf[i] = ClientHeader[i];

                    // Fill Client Buffer
                    ClientSocket.Receive(ClientBuf, HEADER_SIZE, PacketLength, SocketFlags.None);

                    // Parse & Export Packet
                    Packet ClientPacket = new Packet(ClientBuf, PacketDestination.FROM_CLIENT);
                    ClientPacket.Export();

                    // Log Packet
                    Logger.Log(ClientPacket.ID + " | " + ClientPacket.DecryptedPayload.Length + " bytes", LogType.PACKET);

                    JSONPacketManager.HandlePacket(ClientPacket);

                    // Resend
                    ServerSocket.Send(ClientPacket.Rebuilt);
                }
            });

            ServerThread = new Thread(() =>
            {
                while (ServerSocket.Receive(ServerHeader, 0, ServerHeader.Length, SocketFlags.None) != 0)
                {
                    // Parse Packet Length
                    var tmp = ServerHeader.Skip(2).Take(3).ToArray();
                    var PacketLength = ((0x00 << 24) | (tmp[0] << 16) | (tmp[1] << 8) | tmp[2]);

                    // Initialize Server Buffer
                    ServerBuf = new byte[PacketLength + HEADER_SIZE];

                    // Apply header
                    for (int i = 0; i < HEADER_SIZE; i++)
                        ServerBuf[i] = ServerHeader[i];

                    // Fill Server Buffer
                    ServerSocket.Receive(ServerBuf, HEADER_SIZE, PacketLength, SocketFlags.None);

                    // Parse & Export Packet
                    Packet ServerPacket = new Packet(ServerBuf, PacketDestination.FROM_SERVER);
                    ServerPacket.Export();

                    // Log Packet
                    Logger.Log(ServerPacket.ID + " | " + ServerPacket.DecryptedPayload.Length + " bytes", LogType.PACKET);

                    JSONPacketManager.HandlePacket(ServerPacket);

                    // Resend
                    ClientSocket.Send(ServerPacket.Rebuilt);
                }
            });

            ClientThread.Start();
            ServerThread.Start();
            Running = true;
        }
    }
}
