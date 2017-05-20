using System;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Messages.Server.Battle;

namespace BL.Servers.CoC.Packets.Messages.Client.Battle
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
