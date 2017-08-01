using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SaveVillageLayoutCommand : Command
    {
        public SaveVillageLayoutCommand(PacketReader br)
        {
            br.Read();
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}
