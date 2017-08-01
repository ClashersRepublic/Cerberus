using System;
using Savage.Magic.Core;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.GameOpCommands
{
    internal class UnbanGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public UnbanGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 2;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 2)
                {
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (l != null)
                        {
                            l.AccountStatus = 0;
                        }
                        else
                        {
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }
    }
}
