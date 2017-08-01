using System;
using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
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
