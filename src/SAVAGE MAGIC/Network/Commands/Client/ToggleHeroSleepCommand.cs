using System.IO;
using Savage.Magic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class ToggleHeroSleepCommand : Command
    {
        public int BuildingId;
        public byte FlagSleep;
        public uint Tick;

        public ToggleHeroSleepCommand(PacketReader br)
        {
            BuildingId = br.ReadInt32(); 
            FlagSleep = br.ReadByte();
            Tick = br.ReadUInt32();
        }
    }
}