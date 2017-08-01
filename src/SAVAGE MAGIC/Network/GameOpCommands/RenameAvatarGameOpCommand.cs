using System;
using Savage.Magic.Core;

using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.GameOpCommands
{
    internal class RenameAvatarGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public RenameAvatarGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 1;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 3)
                {
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (l != null)
                        {
                            l.Avatar.SetName(m_vArgs[2]);
                            if (ResourcesManager.IsPlayerOnline(l))
                            {
                                var p = new AvatarNameChangeOkMessage(l.Client);
                                //p.SetAvatarName(m_vArgs[2]);
                                p.Send();
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (Exception ex)
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
