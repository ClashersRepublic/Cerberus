using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class UnknownCommand : Command
    {
        public static int Tick;
        public static int Unknown1;

        public UnknownCommand(PacketReader br)
        {
        }

        public override void Execute(Level level)
        {
        }
    }
}