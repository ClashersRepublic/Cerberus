using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class SetPrivilegesGameOpCommand : GameOpCommand
    {
        #region Private Fields

        readonly string[] m_vArgs;

        #endregion Private Fields

        #region Public Constructors

        public SetPrivilegesGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 4;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 3)
                {
                    try
                    {
                        var id = Convert.ToInt64(m_vArgs[1]);
                        var accountPrivileges = Convert.ToByte(m_vArgs[2]);
                        var l = ResourcesManager.GetPlayer(id);
                        if (accountPrivileges < level.AccountPrivileges)
                        {
                            if (l != null)
                            {
                                l.AccountPrivileges = accountPrivileges;
                            }
                            else
                            {
                                //Debugger.WriteLine("SetPrivileges failed: id " + id + " not found");
                            }
                        }
                        else
                        {
                            //Debugger.WriteLine("SetPrivileges failed: target privileges too high");
                        }
                    }
                    catch 
                    {
                        ////Debugger.WriteLine("SetPrivileges failed with error: " + ex);
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.Client);
            }
        }

        #endregion Public Methods
    }
}