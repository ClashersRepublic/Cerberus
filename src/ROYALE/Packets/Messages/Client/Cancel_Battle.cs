using System;
using CRepublic.Royale.Core;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;

namespace CRepublic.Royale.Packets.Messages.Client
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
