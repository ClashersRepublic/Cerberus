using System;
using System.IO;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.AvatarStreamEntries;
using Magic.ClashOfClans.Logic.StreamEntries;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.ClashOfClans.Network.Commands.Server;
using System.Threading.Tasks;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class KickAllianceMemberCommand : Command
    {
        public KickAllianceMemberCommand(PacketReader br)
        {
            m_vAvatarId = br.ReadInt64WithEndian();
            br.ReadByte();
            m_vMessage = br.ReadScString();
            br.ReadInt32WithEndian();
        }

        public override void Execute(Level level)
        {
            var targetAccount = ResourcesManager.GetPlayer(m_vAvatarId);
            if (targetAccount != null)
            {
                var targetAvatar = targetAccount.Avatar;
                var targetAllianceId = targetAvatar.GetAllianceId();
                var requesterAvatar = level.Avatar;
                var requesterAllianceId = requesterAvatar.GetAllianceId();
                if (requesterAllianceId > 0 && targetAllianceId == requesterAllianceId)
                {
                    var alliance = ObjectManager.GetAlliance(requesterAllianceId);
                    lock (alliance)
                    {
                        var requesterMember = alliance.GetAllianceMember(requesterAvatar.Id);
                        var targetMember = alliance.GetAllianceMember(m_vAvatarId);
                        if (targetMember.HasLowerRoleThan(requesterMember.GetRole()))
                        {
                            targetAvatar.SetAllianceId(0);
                            alliance.RemoveMember(m_vAvatarId);
                            if (ResourcesManager.IsPlayerOnline(targetAccount))
                            {
                                var leaveAllianceCommand = new LeavedAllianceCommand();
                                leaveAllianceCommand.SetAlliance(alliance);
                                leaveAllianceCommand.SetReason(2); //Kick
                                var availableServerCommandMessage = new AvailableServerCommandMessage(targetAccount.Client);
                                availableServerCommandMessage.SetCommandId(2);
                                availableServerCommandMessage.SetCommand(leaveAllianceCommand);
                                availableServerCommandMessage.Send();

                                var kickOutStreamEntry = new AllianceKickOutStreamEntry();
                                kickOutStreamEntry.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                kickOutStreamEntry.SetAvatar(requesterAvatar);
                                kickOutStreamEntry.SetIsNew(0);
                                kickOutStreamEntry.SetAllianceId(alliance.AllianceId);
                                kickOutStreamEntry.SetAllianceBadgeData(alliance.AllianceBadgeData);
                                kickOutStreamEntry.SetAllianceName(alliance.AllianceName);
                                kickOutStreamEntry.SetMessage(m_vMessage);

                                var p = new AvatarStreamEntryMessage(targetAccount.Client);
                                p.SetAvatarStreamEntry(kickOutStreamEntry);
                                p.Send();
                            }

                            var eventStreamEntry = new AllianceEventStreamEntry();
                            eventStreamEntry.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            eventStreamEntry.SetSender(targetAvatar);
                            eventStreamEntry.SetEventType(1);
                            eventStreamEntry.SetAvatarId(requesterAvatar.Id);
                            eventStreamEntry.SetAvatarName(requesterAvatar.GetAvatarName());
                            alliance.AddChatMessage(eventStreamEntry);

                            Parallel.ForEach((alliance.AllianceMembers), op =>
                            {
                                var alliancemembers = ResourcesManager.GetPlayer(op.GetAvatarId());
                                if (alliancemembers.Client != null)
                                {
                                    var p = new AllianceStreamEntryMessage(alliancemembers.Client);
                                    p.SetStreamEntry(eventStreamEntry);
                                    p.Send();
                                }
                            });
                        }
                    }
                }
            }
        }

        readonly long m_vAvatarId;
        readonly string m_vMessage;
    }
}
