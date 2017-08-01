using System;
using System.IO;
using System.Text;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.StreamEntries;
using Magic.ClashOfClans.Network.Messages.Server;
using  Magic.ClashOfClans.Network.Commands.Server;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class EditClanSettingsMessage : Message
    {
        private int m_vAllianceBadgeData;
        private string m_vAllianceDescription;
        private int m_vAllianceOrigin;
        private int m_vAllianceType;
        private int m_vRequiredScore;
        private int m_vWarFrequency;
        private byte m_vWarAndFriendlyStatus;

        public EditClanSettingsMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader (new MemoryStream(Data)))
            {
                m_vAllianceDescription = br.ReadString();
                br.ReadInt32();
                m_vAllianceBadgeData = br.ReadInt32();
                m_vAllianceType = br.ReadInt32();
                m_vRequiredScore = br.ReadInt32();
                m_vWarFrequency = br.ReadInt32();
                m_vAllianceOrigin = br.ReadInt32();
                m_vWarAndFriendlyStatus = br.ReadByte();
            }
        }

        public override void Process(Level level)
        {
            EditClanSettingsMessage clanSettingsMessage = this;
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
                if (alliance != null)
                {
                    if (clanSettingsMessage.m_vAllianceDescription.Length < 259 || clanSettingsMessage.m_vAllianceDescription.Length < 0)
                    {
                        if (clanSettingsMessage.m_vAllianceBadgeData < 1 || (long)clanSettingsMessage.m_vAllianceBadgeData < 10000000000L)
                        {
                            if (clanSettingsMessage.m_vAllianceType < 0 || clanSettingsMessage.m_vAllianceType < 10)
                            {
                                if (clanSettingsMessage.m_vRequiredScore < 0 || clanSettingsMessage.m_vRequiredScore < 4201)
                                {
                                    if (clanSettingsMessage.m_vWarFrequency < 0 || clanSettingsMessage.m_vWarFrequency < 10)
                                    {
                                        if (clanSettingsMessage.m_vAllianceOrigin < 0 || clanSettingsMessage.m_vAllianceOrigin < 42000000)
                                        {
                                            if ((int)clanSettingsMessage.m_vWarAndFriendlyStatus < 0 || (int)clanSettingsMessage.m_vWarAndFriendlyStatus < 5)
                                            {
                                                alliance.AllianceDescription = m_vAllianceDescription;
                                                alliance.AllianceBadgeData = m_vAllianceBadgeData;
                                                alliance.AllianceType = m_vAllianceType;
                                                alliance.RequiredScore = m_vRequiredScore;
                                                alliance.WarFrequency = m_vWarFrequency;
                                                alliance.AllianceOrigin = m_vAllianceOrigin;
                                                alliance.SetWarAndFriendlytStatus(m_vWarAndFriendlyStatus);
                                                Avatar avatar = level.Avatar;
                                                avatar.GetAllianceId();
                                                AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                                                eventStreamEntry.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                                eventStreamEntry.SetSender(avatar);
                                                eventStreamEntry.SetEventType(10);
                                                eventStreamEntry.SetAvatarId(avatar.Id);
                                                eventStreamEntry.SetAvatarName(avatar.GetAvatarName());
                                                eventStreamEntry.SetSenderId(avatar.Id);
                                                eventStreamEntry.SetSenderName(avatar.GetAvatarName());
                                                alliance.AddChatMessage(eventStreamEntry);
                                                AllianceSettingChangedCommand Command = new AllianceSettingChangedCommand();
                                                Command.SetAlliance(alliance);
                                                Command.SetPlayer(level);
                                                var availableServerCommandMessage = new AvailableServerCommandMessage(level.Client);
                                                availableServerCommandMessage.SetCommandId(6);
                                                availableServerCommandMessage.SetCommand(Command);
                                                availableServerCommandMessage.Send();
                                                foreach (AllianceMemberEntry allianceMember in alliance.AllianceMembers)
                                                {
                                                    Level player = ResourcesManager.GetPlayer(allianceMember.GetAvatarId(), false);
                                                    if (ResourcesManager.IsPlayerOnline(player))
                                                    {
                                                        var p = new AllianceStreamEntryMessage(player.Client);
                                                        AllianceEventStreamEntry eventStreamEntry1 = eventStreamEntry;
                                                        p.SetStreamEntry(eventStreamEntry1);
                                                        p.Send();
                                                    }
                                                }
                                                List<AllianceMemberEntry>.Enumerator enumerator = new List<AllianceMemberEntry>.Enumerator();
                                                DatabaseManager.Instance.Save(alliance);
                                                eventStreamEntry = (AllianceEventStreamEntry) null;
                                            }
                                            else
                                                ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                                        }
                                        else
                                            ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                                    }
                                    else
                                        ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                                }
                                else
                                    ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                            }
                            else
                                ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                        }
                        else
                            ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                    }
                    else
                        ResourcesManager.DisconnectClient(clanSettingsMessage.Client);
                }
                alliance = (Alliance) null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
