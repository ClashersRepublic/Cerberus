using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server.Alliance;

namespace BL.Servers.CR.Packets.Messages.Client.Alliance
{
    internal class Request_Create_Alliance : Message
    {
        public Request_Create_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Process()
        {
            ShowValues();
            new Response_Create_Alliance(this.Device).Send();
        }
    }
}
