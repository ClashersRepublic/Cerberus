using System;
using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SetActiveVillageLayoutCommand : Command
    {
        private int Layout;
        public SetActiveVillageLayoutCommand(PacketReader br)
        {
            Layout = br.ReadInt32();
        }
        public override void Execute(Level level)
        {
            level.Avatar.SetActiveLayout(Layout);
        }
    }
}
