using System.IO;
using Savage.Magic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class DonateAllianceUnitCommand : Command
    {
        public uint PlayerId;
        public uint UnitType;
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        public DonateAllianceUnitCommand(PacketReader br)
        {
        }
    }
}
