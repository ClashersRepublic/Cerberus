using System;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Battle;

namespace CRepublic.Magic.Packets.Messages.Client.Battle
{
    internal class Get_Replay : Message
    {
        internal long Replay_ID;

        public Get_Replay(Device device) : base(device)
        {
            // Get_Replay.
        }

        internal override void Decode()
        {
            this.Replay_ID = this.Reader.ReadInt64();
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            new Replay_Data(this.Device) { Battle_ID = this.Replay_ID }.Send();
        }
    }
}
