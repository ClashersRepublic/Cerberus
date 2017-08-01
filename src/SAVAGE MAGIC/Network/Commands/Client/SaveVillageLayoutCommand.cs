using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
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
