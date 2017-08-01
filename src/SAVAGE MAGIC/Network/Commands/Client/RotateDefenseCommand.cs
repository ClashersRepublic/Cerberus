using System;
using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class RotateDefenseCommand : Command
    {
        public int BuildingID { get; set; }

        public RotateDefenseCommand(PacketReader br)
        {
            BuildingID = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }
    }
}
