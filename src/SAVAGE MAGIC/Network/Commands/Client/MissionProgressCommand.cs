using System.IO;
using Savage.Magic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class MissionProgressCommand : Command
    {
        public uint MissionID;
        public uint Tick;

        public MissionProgressCommand(PacketReader br)
        {
            MissionID = br.ReadUInt32();
            Tick = br.ReadUInt32();
        }
    }
}