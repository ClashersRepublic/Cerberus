using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Battle
{
    internal class Matchmaking_Info : Message
    {
        internal Matchmaking_Info(Device Device) : base(Device)
        {
            this.Identifier = 24107;
        }

        internal override void Encode()
        {
            this.Data.AddInt(200);
        }
    }
}
