using System;
using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class EditVillageLayoutCommand : Command
    {
        internal int X;
        internal int Y;
        internal int BuildingID;
        internal int Layout;

        public EditVillageLayoutCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            BuildingID = br.ReadInt32();
            Layout = br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
        }

    }
}
