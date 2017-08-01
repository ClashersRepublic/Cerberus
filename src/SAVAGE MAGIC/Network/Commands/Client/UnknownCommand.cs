using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
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