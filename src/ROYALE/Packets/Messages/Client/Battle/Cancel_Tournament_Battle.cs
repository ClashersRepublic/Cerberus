using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRepublic.Royale.Core.Network;

namespace CRepublic.Royale.Packets.Messages.Client.Battle
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
