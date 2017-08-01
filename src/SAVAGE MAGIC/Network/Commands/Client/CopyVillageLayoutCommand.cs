using System.IO;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class CopyVillageLayoutCommand : Command
    {
        public int PasteLayoutID;
        public int CopiedLayoutID;
        public int Tick;

        public CopyVillageLayoutCommand(PacketReader br)
        {
            Tick = br.ReadInt32();
            CopiedLayoutID = br.ReadInt32();
            PasteLayoutID = br.ReadInt32();
        }
    }
}