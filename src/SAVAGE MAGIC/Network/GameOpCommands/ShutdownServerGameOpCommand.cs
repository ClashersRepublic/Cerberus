using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
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
