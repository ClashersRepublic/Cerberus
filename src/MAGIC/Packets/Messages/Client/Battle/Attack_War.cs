using System;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Messages.Server.Battle;

namespace CRepublic.Magic.Packets.Messages.Client.Battle
{
    internal class Attack_War : Message
    {
        internal int Npc_ID;

        public Attack_War(Device device) : base(device)
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
