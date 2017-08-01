using System;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.AvatarStreamEntries;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class HelpGameOpCommand: GameOpCommand
    {
        readonly string[] m_vArgs;

        public HelpGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 0;
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 1)
                {
                    var avatar = level.Avatar;
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetSenderId(avatar.Id);
                    mail.SetSenderAvatarId(avatar.Id);
                    mail.SetSenderName(avatar.GetAvatarName());
                    mail.SetIsNew(2);
                    mail.SetAllianceId(0);
                    mail.SetAllianceBadgeData(1526735450);
                    mail.SetAllianceName("UCS Server Commands Help");
                    mail.SetMessage(@"/help" +
                        "\n/attack" +
                        "\n/ban" +
                        "\n/kick" +
                        "\n/rename" +
                        "\n/setprivileges" +
                        "\n/shutdown" +
                        "\n/unban" +
                        "\n/visit" +
                        "\n/sysmsg" +
                        "\n/id" +
                        "\n/max" +
                        "\n/saveacc" +
                        "\n/saveall" +
                        "\n/becomeleader" +
                        "\n/status" +
                        "\n/reset");
                    mail.SetSenderLevel(avatar.GetAvatarLevel());
                    mail.SetSenderLeagueId(avatar.GetLeagueId());

                    var p = new AvatarStreamEntryMessage(level.Client);
                    p.SetAvatarStreamEntry(mail);
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
