using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server
{
    internal class Cancel_Battle_OK : Message
    {
        public Cancel_Battle_OK(Device Device) : base(Device)
        {
            this.Identifier = 24125;
        }

        internal override void Encode()
        {
            this.Data.Add(1);
        }

        internal override void Process()
        {
            this.Device.PlayerState = Logic.Enums.Client_State.LOGGED;
        }
    }
}
