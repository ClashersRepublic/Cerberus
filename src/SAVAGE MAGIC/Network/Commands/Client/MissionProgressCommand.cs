using System.IO;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Commands.Client
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