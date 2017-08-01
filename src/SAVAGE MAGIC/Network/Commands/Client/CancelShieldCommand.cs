using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class CancelShieldCommand : Command
    {
        public int Tick;

        public CancelShieldCommand(PacketReader br)
        {
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            level.Avatar.SetShieldTime(0);
        }
    }      
}
