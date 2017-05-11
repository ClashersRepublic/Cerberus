using System;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Battle;

namespace BL.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Search_Opponent : Command
    {
        public Search_Opponent(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            new Pc_Battle_Data(this.Device) {  Enemy = this.Device.Player}.Send();
        }
    }
}
