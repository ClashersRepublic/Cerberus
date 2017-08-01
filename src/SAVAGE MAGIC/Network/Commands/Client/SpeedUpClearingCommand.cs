using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
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
