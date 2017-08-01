using System.IO;
using Savage.Magic;

namespace Savage.Magic.Network.Commands.Client
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