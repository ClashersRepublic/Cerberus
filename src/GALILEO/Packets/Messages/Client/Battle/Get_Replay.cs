using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Battle;

namespace BL.Servers.CoC.Packets.Messages.Client.Battle
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
