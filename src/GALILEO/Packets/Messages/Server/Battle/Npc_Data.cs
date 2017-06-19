using Republic.Magic.Extensions.List;
using Republic.Magic.Files;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;

namespace Republic.Magic.Packets.Messages.Server.Battle
{
    internal class Npc_Data : Message
    {
        internal int Npc_ID = 0;
        internal Level Avatar;

        public Npc_Data(Device _Device) : base(_Device)
        {
            this.Identifier = 24133;
        }

        internal override void Encode()
        {
            using (Objects Home = new Objects(Avatar = this.Device.Player, NPC.Levels[Npc_ID]))
            {
                this.Data.AddInt(0);
                this.Data.AddRange(Home.ToBytes);
                this.Data.AddRange(this.Device.Player.Avatar.ToBytes);

                this.Data.AddInt(this.Npc_ID);
                this.Data.AddByte(0);
            }
        }

        internal override void Process()
        {
            this.Device.State = State.IN_NPC_BATTLE;
        }
    }
}