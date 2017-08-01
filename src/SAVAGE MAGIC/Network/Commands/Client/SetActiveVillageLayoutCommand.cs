using System;
using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
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
