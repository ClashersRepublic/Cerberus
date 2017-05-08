using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server
{
    internal class Avatar_Stream : Message
    {
        public Avatar_Stream(Device client) : base(client)
        {
            this.Identifier = 24411;
        }

        internal override void Encode()
        {
            this.Data.AddInt(0);
        }
    }
}
