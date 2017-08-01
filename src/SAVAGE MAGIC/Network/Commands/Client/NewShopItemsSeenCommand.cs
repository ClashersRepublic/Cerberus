using System.IO;
using Savage.Magic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class NewShopItemsSeenCommand : Command
    {
        public uint NewShopItemNumber;
        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        public NewShopItemsSeenCommand(PacketReader br)
        {
           br.ReadInt32();
           br.ReadInt32();
           br.ReadInt32();
           br.ReadInt32();
        }
    }
}