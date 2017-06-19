using System;
namespace CRepublic.Royale.Packets.Messages.Client.Battle
{
    using CRepublic.Royale.Core.Network;
    using CRepublic.Royale.Extensions.Binary;
    using CRepublic.Royale.Logic;
    using CRepublic.Royale.Packets.Messages.Server.Battle;
    
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
