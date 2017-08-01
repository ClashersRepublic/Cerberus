using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SpeedUpClearingCommand : Command
    {
        internal int m_vObstacleId;
        internal int m_vTick;

        public SpeedUpClearingCommand(PacketReader br)
        {
            m_vObstacleId = br.ReadInt32();
             m_vTick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}
