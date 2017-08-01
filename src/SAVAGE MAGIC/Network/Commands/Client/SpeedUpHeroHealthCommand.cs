using System.IO;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Commands.Client
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