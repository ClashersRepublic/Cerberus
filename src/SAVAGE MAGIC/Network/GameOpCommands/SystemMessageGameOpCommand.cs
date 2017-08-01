using System;
using System.Linq;
using Savage.Magic.Core;

using Savage.Magic.Logic;
using Savage.Magic.Logic.AvatarStreamEntries;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.GameOpCommands
{
    internal class SystemMessageGameOpCommand : GameOpCommand
    {
        readonly string[] m_vArgs;

        public SystemMessageGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 1;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 1)
                {
                    var message = string.Join(" ", m_vArgs.Skip(1));
                    var avatar = level.Avatar;
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetSenderId(avatar.Id);
                    mail.SetSenderAvatarId(avatar.Id);
                    mail.SetSenderName(avatar.GetAvatarName());
                    mail.SetIsNew(2);
                    mail.SetAllianceId(0);
                    mail.SetAllianceBadgeData(1526735450);
                    mail.SetAllianceName("Administrator");
                    mail.SetMessage(message);
                    mail.SetSenderLevel(avatar.GetAvatarLevel());
                    mail.SetSenderLeagueId(avatar.GetLeagueId());

                    foreach (var onlinePlayer in ResourcesManager.OnlinePlayers)
                    {
                        var p = new AvatarStreamEntryMessage(onlinePlayer.Client);
                        p.SetAvatarStreamEntry(mail);
                        p.Send();
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
