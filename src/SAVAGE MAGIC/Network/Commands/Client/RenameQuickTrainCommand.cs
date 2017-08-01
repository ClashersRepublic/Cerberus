using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class RenameQuickTrainCommand : Command
    {
        public int SlotID;
        public string SlotName;
        public int Tick;

        public RenameQuickTrainCommand(PacketReader br)
        {
            SlotID = br.ReadInt32();
            SlotName = br.ReadString();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}