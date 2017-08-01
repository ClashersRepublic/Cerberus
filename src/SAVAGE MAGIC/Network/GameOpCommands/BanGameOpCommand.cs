using System;
using Savage.Magic.Core;

using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.GameOpCommands
{
    internal class BanGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public BanGameOpCommand(string[] args)
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
                            if (l.AccountPrivileges< level.AccountPrivileges)
                            {
                                l.AccountStatus = 99;
                                l.AccountPrivileges = 0;
                                if (ResourcesManager.IsPlayerOnline(l))
                                {
                                    new OutOfSyncMessage(l.Client).Send();
                                    
                                }
                            }
                            else
                            {
                            }
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
