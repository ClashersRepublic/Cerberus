using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server
{
    internal class Avatar_Stream : Message
    {
        internal Player Player = null;

        public Avatar_Stream(Device client) : base(client)
        {
            this.Identifier = 24411;
        }

        public Avatar_Stream(Device client, Player player) : base(client)
        {
            this.Identifier = 24411;
            this.Player = player;
        }

        internal override void Encode()
        {
            this.Data.AddRange(Player != null ? this.Player.Inbox.ToBytes : this.Device.Player.Avatar.Inbox.ToBytes);
        }
    }
}
