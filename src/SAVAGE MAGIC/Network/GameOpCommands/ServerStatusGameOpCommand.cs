using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.AvatarStreamEntries;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.GameOpCommands
{
    internal class ServerStatusGameOpCommand   : GameOpCommand
    {
        readonly string[] m_vArgs;
        private readonly PerformanceCounter _cpuCounter;

        public ServerStatusGameOpCommand(string[] args)
        {
            m_vArgs = args;
            RequiredPrivileges = 0;
            var curProcess = Process.GetCurrentProcess();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public override void Execute(Level level)
        {
            if (level.AccountPrivileges>= RequiredPrivileges)
            {
                if (m_vArgs.Length >= 1)
                {
                    _cpuCounter.NextValue(); //Always 0
                    var avatar = level.Avatar;
                    var mail = new AllianceMailStreamEntry();
                    mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    mail.SetSenderId(avatar.Id);
                    mail.SetSenderAvatarId(avatar.Id);
                    mail.SetSenderName(avatar.GetAvatarName());
                    mail.SetIsNew(2);
                    mail.SetAllianceId(0);
                    mail.SetAllianceBadgeData(1526735450);
                    mail.SetAllianceName("UCS Server Information");
					mail.SetMessage(@"Online Players: " + ResourcesManager.OnlinePlayers.Count +
						"\nIn Memory Players: " + ResourcesManager.GetInMemoryLevels().Count +
						"\nConnected Players: " + ResourcesManager.GetConnectedClients().Count +
						//"\nUCS Ram: " + (Process.GetCurrentProcess().WorkingSet64 / 1048576) + "MB/" + //Unknown yet how to get properly
						"\nServer Ram: " + PerformanceInfo.GetPhysicalAvailableMemoryInMiB() + "MB/" + Performances.GetTotalMemory() + "MB" +
						"\nServer CPU " + _cpuCounter.NextValue() + "%"  //Match Taskmanager
						);

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
