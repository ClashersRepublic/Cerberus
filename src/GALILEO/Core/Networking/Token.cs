using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Republic.Magic.Extensions;
using Republic.Magic.Logic;

namespace Republic.Magic.Core.Networking
{
    internal class Token
    {
        internal Device Device;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;

        internal byte[] Buffer;

        internal int Offset;

        internal bool Aborting;

        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device = Device;
            this.Device.Token = this;

            this.Args = Args;
            this.Args.UserToken = this;

            this.Buffer = new byte[Constants.ReceiveBuffer];
            this.Packet = new List<byte>(Constants.ReceiveBuffer);
        }

        internal void Process()
        {
            byte[] Data = this.Packet.ToArray();
            this.Device.Process(Data);
        }

        internal void Reset()
        {
            this.Offset = 0;
            this.Packet.Clear();
        }
    }
}