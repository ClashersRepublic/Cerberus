using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server.Alliance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Packets.Messages.Client.Alliance
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
