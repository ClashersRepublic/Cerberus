using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class PlaceHeroCommand : Command
    {
        public int X;
        public int Y;
        public int Tick;
        public int HeroID;

        public PlaceHeroCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            Tick = br.ReadInt32();
            HeroID = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}