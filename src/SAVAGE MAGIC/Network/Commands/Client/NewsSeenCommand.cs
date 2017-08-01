using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class NewsSeenCommand : Command
    {
        public byte[] packet;

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public NewsSeenCommand(PacketReader br)
        {
           br.ReadInt32();
           br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}