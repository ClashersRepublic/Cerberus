using Savage.Magic.Core;

using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.GameOpCommands
{
    internal class ShutdownServerGameOpCommand : GameOpCommand
    {
        string[] m_vArgs;

        public ShutdownServerGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 4;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                foreach (var onlinePlayer in ResourcesManager.OnlinePlayers)
                {
                    var p = new ShutdownStartedMessage(onlinePlayer.Client);
                    p.SetCode(5);
                    p.Send();
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
