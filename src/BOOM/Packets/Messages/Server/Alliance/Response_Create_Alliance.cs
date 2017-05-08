using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.BB.Extensions.List;
using BL.Servers.BB.Logic;

namespace BL.Servers.BB.Packets.Messages.Server.Alliance
{
    internal class Response_Create_Alliance : Message
    {
        internal Response_Create_Alliance(Device Device) : base(Device)
        {
            this.Identifier = 24356;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0); //Time left until able to create alliance
        }
    }
}
