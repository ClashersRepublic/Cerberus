using Savage.Magic.Core;

using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.GameOpCommands
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
