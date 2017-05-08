using System;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Npc_Data : Message
    {
        internal int Npc_ID = 0;
        internal Level Avatar;

        public Npc_Data(Device _Device) : base(_Device)
        {
            this.Identifier = 24133;
            this.Device.State = State.IN_BATTLE;     
        }

        internal override void Encode()
        {
            using (Objects Home = new Objects(Avatar = this.Device.Player, NPC.Levels[Npc_ID]))
            {
                this.Data.AddInt(0);
                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(this.Device.Player.Avatar.ToBytes);

                this.Data.AddInt(this.Npc_ID);
            }
        }
    }
}