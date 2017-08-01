using System.IO;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Savage.Magic.Network.Commands.Client
{
    internal class TogglePlayerWarStateCommand : Command
    {
        public TogglePlayerWarStateCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            //TODO
        }
    }
}