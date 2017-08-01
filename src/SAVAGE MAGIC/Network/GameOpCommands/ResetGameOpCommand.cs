using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class ResetGameOpCommand : GameOpCommand
    {
        public ResetGameOpCommand(string[] args)
        {
            RequiredPrivileges = 0;
        }

        public override void Execute(Level level)
        {
            level.SetHome(ObjectManager.m_vHomeDefault);
            new OwnHomeDataMessage(level.Client, level).Send();
        }
    }
}
