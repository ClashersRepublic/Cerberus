using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using BL.Networking.Lyra.Core.Network.Packets;

namespace BL.Networking.Lyra.Core.Network
{
    internal class SendReceiveThread
    {
        internal Packet ClientPacket, ServerPacket;

        internal Thread ClientThread, ServerThread;
        internal Socket ClientSocket, ServerSocket;

        internal byte[] Client_Header = new byte[Constants.Header_Size];
        internal byte[] Server_Header = new byte[Constants.Header_Size];

        internal byte[] Client_Buf, Server_Buf;

        internal SendReceiveThread(Socket _ClientSocket, Socket _ServerSocket)
        {
            this.ClientSocket = _ClientSocket;
            this.ServerSocket = _ServerSocket;

            this.ClientThread = new Thread(() =>
            {
                while (this.ClientSocket.Receive(this.Client_Header, 0, this.Client_Header.Length, SocketFlags.None) != 0)
                {
                    var Temp = this.Client_Header.Skip(2).Take(3).ToArray();
                    var Packet_Length = ((0x00 << 24) | (Temp[0] << 16) | (Temp[1] << 8) | Temp[2]);

                    this.Client_Buf = new byte[Packet_Length + Constants.Header_Size];

                    for (int i = 0; i < Constants.Header_Size; i++)
                        this.Client_Buf[i] = this.Client_Header[i];

                    this.ClientSocket.Receive(this.Client_Buf, Constants.Header_Size, Packet_Length, SocketFlags.None);

                    this.ClientPacket = new Packet(this.Client_Buf, PacketDestination.CLIENT);
                    this.ClientPacket.Export();

                    Console.WriteLine($"{this.ClientPacket.PacketID} -> SERVER ({this.ClientPacket.DecryptedPayload.Length} bytes)");

                    this.ServerSocket.Send(this.ClientPacket.Rebuilt);
                }
            });

            this.ServerThread = new Thread(() =>
            {
                while (this.ServerSocket.Receive(this.Server_Header, 0, this.Server_Header.Length, SocketFlags.None) != 0)
                {
                    var Temp = this.Server_Header.Skip(2).Take(3).ToArray();
                    var Packet_Length = ((0x00 << 24) | (Temp[0] << 16) | (Temp[1] << 8) | Temp[2]);

                    this.Server_Buf = new byte[Packet_Length + Constants.Header_Size];

                    for (int i = 0; i < Constants.Header_Size; i++)
                        this.Server_Buf[i] = this.Server_Header[i];

                    this.ServerSocket.Receive(this.Server_Buf, Constants.Header_Size, Packet_Length, SocketFlags.None);

                    this.ServerPacket = new Packet(this.Client_Buf, PacketDestination.CLIENT);
                    this.ServerPacket.Export();

                    Console.WriteLine($"{this.ServerPacket.PacketID} <- CLIENT ({this.ServerPacket.DecryptedPayload.Length} bytes)");

                    this.ClientSocket.Send(this.ServerPacket.Rebuilt);
                }
            });

            this.ClientThread.Start();
            this.ServerThread.Start();
        }
    }
}
