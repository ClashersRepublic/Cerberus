using System.IO;
using Savage.Magic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class SpeedUpHeroHealthCommand : Command
    {
        public SpeedUpHeroHealthCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
        }
    }
}