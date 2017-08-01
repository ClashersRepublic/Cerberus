using System.IO;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class CancelUpgradeUnitCommand : Command
    {
        public uint BuildingId { get; set; }

        public uint Unknown1 { get; set; }

        public CancelUpgradeUnitCommand(PacketReader br)
        {
        }
    }
}