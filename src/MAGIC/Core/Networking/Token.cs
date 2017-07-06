using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Core.Networking
{
    internal class Token
    {
        internal Device Device;
        internal SocketAsyncEventArgs Args;
        internal List<byte> Packet;

        //internal byte[] Buffer; // Not used.

        //internal int Offset; // Not used.

        //internal bool Aborting; // Not used.

        internal Token(SocketAsyncEventArgs Args, Device Device)
        {
            this.Device = Device;
            this.Device.Token = this;

            this.Args = Args;
            this.Args.UserToken = this;

            //this.Buffer = new byte[Constants.ReceiveBuffer];
            this.Packet = new List<byte>(Constants.ReceiveBuffer);
        }

        internal void Process()
        {
            byte[] Data = this.Packet.ToArray();
            this.Device.Process(Data);
        }

        [Obsolete]
        internal void Reset()
        {
            this.Offset = 0;
            this.Packet.Clear();
        }
    }
}