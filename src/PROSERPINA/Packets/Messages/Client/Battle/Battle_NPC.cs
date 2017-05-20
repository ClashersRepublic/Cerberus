using System;
namespace BL.Servers.CR.Packets.Messages.Client.Battle
{
    using BL.Servers.CR.Core.Network;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Packets.Messages.Server.Battle;
    
    internal class Battle_NPC : Message
    {
        public Battle_NPC(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            new Sector_NPC(this.Device).Send();
        }
    }
}
