using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.Servers.CR.Core.Network;

namespace BL.Servers.CR.Packets.Messages.Client.Battle
{
    internal class Cancel_Tournament_Battle : Message
    {
        internal Cancel_Tournament_Battle(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Process()
        {
            //new Cancel_Battle_OK(this.Device).Send();
        }
    }
}
