using System;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Battle;

namespace Republic.Magic.Packets.Messages.Client.Battle
{
    internal class Get_Replay : Message
    {
        internal long Replay_ID;

        public Get_Replay(Device Device, Reader Reader) : base(Device, Reader)
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
