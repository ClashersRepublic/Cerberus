using BL.Servers.CR.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Packets.Messages.Server.Error
{
    internal class Out_Of_Sync : Message
    {
        internal Out_Of_Sync(Device Device) : base(Device)
        {
            this.Identifier = 24104;
        }
    }
}
