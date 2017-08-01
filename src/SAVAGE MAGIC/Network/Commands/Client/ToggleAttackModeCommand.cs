using System.IO;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class ToggleAttackModeCommand : Command
    {
        public uint BuildingId { get; set; }

        public byte Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public ToggleAttackModeCommand(PacketReader br)
        {
        }
    }
}