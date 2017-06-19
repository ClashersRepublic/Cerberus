using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server.Alliance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Packets.Messages.Client.Alliance
{
    internal class Request_Alliance_Data : Message
    {
        internal long ClanID;

        public Request_Alliance_Data(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.ClanID = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            new Alliance_Data(this.Device, this.ClanID).Send();
        }
    }
}
