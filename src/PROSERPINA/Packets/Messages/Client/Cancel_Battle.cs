using System;
using BL.Servers.CR.Core;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Packets.Messages.Client
{
    internal class Cancel_Battle : Message
    {
        public Cancel_Battle(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.Device.PlayerState = Logic.Enums.Client_State.LOGGED;
        }

        internal override void Process()
        {
            Server_Resources.Battles.Dequeue(this.Device.Player);

            new Cancel_Battle_OK(Device).Send();
        }
    }
}
