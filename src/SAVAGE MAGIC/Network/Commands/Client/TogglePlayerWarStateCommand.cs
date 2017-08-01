using System.IO;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.ClashOfClans.Network.Commands.Client
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