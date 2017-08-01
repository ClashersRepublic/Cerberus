using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class ClientServerTickCommand : Command
    {
        public int Tick;
        public int Unknown1;

        public ClientServerTickCommand(PacketReader br)
        {
            Unknown1 = br.ReadInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}