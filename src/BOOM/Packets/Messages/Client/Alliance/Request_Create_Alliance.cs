using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.BB.Core.Network;
using BL.Servers.BB.Extensions.Binary;
using BL.Servers.BB.Logic;
using BL.Servers.BB.Packets.Messages.Server.Alliance;

namespace BL.Servers.BB.Packets.Messages.Client.Alliance
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
