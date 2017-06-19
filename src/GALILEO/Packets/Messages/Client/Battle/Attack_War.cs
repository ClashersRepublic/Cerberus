using System;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Packets.Messages.Server.Battle;

namespace Republic.Magic.Packets.Messages.Client.Battle
{
    internal class Attack_War : Message
    {
        internal int Npc_ID;

        public Attack_War(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Attack_War.
        }

        internal override void Decode()
        {
            Console.WriteLine(BitConverter.ToString(this.Reader.ReadFully()));
        }

        internal override void Process()
        {
            new Pc_Battle_Data(this.Device) { Enemy = this.Device.Player, BattleMode = Battle_Mode.NEXT_BUTTON_DISABLE}.Send();
        }
    }
}
