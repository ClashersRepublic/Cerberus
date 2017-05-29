using System;
using System.Linq;
using System.Text;
using BL.Servers.CR.Core;
using BL.Servers.CR.Logic;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Disconnected : Message
    {
        public Disconnected(Device Device) : base(Device)
        {
            this.Identifier = 25892;
        }

        internal override void Process()
        {
            Resources.Gateway.Disconnect(this.Device.Token.Args);
        }
    }
}