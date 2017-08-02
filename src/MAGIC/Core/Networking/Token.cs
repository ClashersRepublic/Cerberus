using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Core.Networking
{
    internal class Token
    {
        internal Device Device;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;

        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device = Device;

            this.Args = Args;
            this.Args.UserToken = this;

            this.Packet = new List<byte>(Constants.Buffer);
        }

        internal void Process()
        {
            byte[] Data = this.Packet.ToArray();
            this.Device.Process();
        }
    }
}