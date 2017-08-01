using System;
using System.IO;
using Savage.Magic.Core;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class SpeedUpTrainingCommand : Command
    {
        private readonly int m_vBuildingId;

        public SpeedUpTrainingCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
            int num = (int) br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}