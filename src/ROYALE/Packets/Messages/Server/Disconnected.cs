using System;
using System.Linq;
using System.Text;
using CRepublic.Royale.Core;
using CRepublic.Royale.Logic;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CRepublic.Royale.Packets.Messages.Server
{
    internal class Disconnected : Message
    {
        public Disconnected(Device Device) : base(Device)
        {
            this.Identifier = 25892;
        }

        internal override void Process()
        {
            this.Device.PlayerState = Logic.Enums.Client_State.DISCONNECTED;

            Server_Resources.Gateway.Disconnect(this.Device.Token.Args);
        }
    }
}