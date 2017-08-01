using System;
using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.StreamEntries;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14306
    internal class PromoteAllianceMemberMessage : Message
    {
        public PromoteAllianceMemberMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public long m_vId;
        public int m_vRole;

        public override void Decode()
        {
            m_vId = Reader.ReadInt64WithEndian();
            m_vRole = Reader.ReadInt32WithEndian();
        }

        public override void Process(Level level)
        {
            var target = ResourcesManager.GetPlayer(m_vId);
            var player = level.Avatar;
            var alliance = ObjectManager.GetAlliance(player.GetAllianceId());
            if (player.GetAllianceRole() == 2 || player.GetAllianceRole() == 4)
            {
                if (player.GetAllianceId() == target.Avatar.GetAllianceId())
                {
                    int oldrole = target.Avatar.GetAllianceRole();
                    target.Avatar.SetAllianceRole(m_vRole);
                    if (m_vRole == 2)
                    {
                        player.SetAllianceRole(4);

                        AllianceEventStreamEntry demote = new AllianceEventStreamEntry();
                        demote.SetId(alliance.ChatMessages.Count + 1);
                        demote.SetSender(player);
                        demote.SetEventType(6);
                        demote.SetAvatarId(player.Id);
                        demote.SetAvatarName(player.GetAvatarName());

                        alliance.AddChatMessage(demote);

                        AllianceEventStreamEntry promote = new AllianceEventStreamEntry();
                        promote.SetId(alliance.ChatMessages.Count + 1);
                        promote.SetSender(target.Avatar);
                        promote.SetEventType(5);
                        promote.SetAvatarId(player.Id);
                        promote.SetAvatarName(player.GetAvatarName());

                        alliance.AddChatMessage(promote);

                        PromoteAllianceMemberOkMessage rup = new PromoteAllianceMemberOkMessage(Client);
                        PromoteAllianceMemberOkMessage rub = new PromoteAllianceMemberOkMessage(target.Client);

                        AllianceRoleUpdateCommand p = new AllianceRoleUpdateCommand();
                        AvailableServerCommandMessage pa = new AvailableServerCommandMessage(Client);

                        AllianceRoleUpdateCommand t = new AllianceRoleUpdateCommand();
                        AvailableServerCommandMessage ta = new AvailableServerCommandMessage(target.Client);

                        rup.SetID(level.Avatar.Id);
                        rup.SetRole(4);
                        rub.SetID(target.Avatar.Id);
                        rub.SetRole(2);

                        t.SetAlliance(alliance);
                        p.SetAlliance(alliance);
                        t.SetRole(2);
                        p.SetRole(4);
                        t.Tick(target);
                        p.Tick(level);

                        ta.SetCommandId(8);
                        pa.SetCommandId(8);
                        ta.SetCommand(t);
                        pa.SetCommand(p);
                        if (ResourcesManager.IsPlayerOnline(target))
                        {
                            ta.Send();
                            rub.Send();
                        }
                        rup.Send();
                        pa.Send();


                        // New function for send a message
                        foreach (AllianceMemberEntry op in alliance.AllianceMembers)
                        {
                            Level aplayer = ResourcesManager.GetPlayer(op.GetAvatarId());
                            if (aplayer.Client!= null)
                            {
                                AllianceStreamEntryMessage a = new AllianceStreamEntryMessage(aplayer.Client);
                                AllianceStreamEntryMessage b = new AllianceStreamEntryMessage(aplayer.Client);

                                a.SetStreamEntry(demote);
                                b.SetStreamEntry(promote);

                                a.Send();
                                b.Send();
                            }

                        }
                    }
                    else
                    {
                        AllianceRoleUpdateCommand t = new AllianceRoleUpdateCommand();
                        AvailableServerCommandMessage ta = new AvailableServerCommandMessage(target.Client);
                        PromoteAllianceMemberOkMessage ru = new PromoteAllianceMemberOkMessage(target.Client);
                        AllianceEventStreamEntry stream = new AllianceEventStreamEntry();

                        stream.SetId(alliance.ChatMessages.Count + 1);
                        stream.SetSender(target.Avatar);
                        stream.SetAvatarId(player.Id);
                        stream.SetAvatarName(player.GetAvatarName());
                        if (m_vRole > oldrole)
                            stream.SetEventType(5);
                        else
                            stream.SetEventType(6);

                        t.SetAlliance(alliance);
                        t.SetRole(m_vRole);
                        t.Tick(target);

                        ta.SetCommandId(8);
                        ta.SetCommand(t);

                        ru.SetID(target.Avatar.Id);
                        ru.SetRole(m_vRole);

                        alliance.AddChatMessage(stream);

                        if (ResourcesManager.IsPlayerOnline(target))
                        {
                            ta.Send();
                            ru.Send();
                        }
                        // New function for send a message
                        foreach (AllianceMemberEntry op in alliance.AllianceMembers)
                        {
                            Level aplayer = ResourcesManager.GetPlayer(op.GetAvatarId());
                            if (aplayer.Client!= null)
                            {
                                AllianceStreamEntryMessage b = new AllianceStreamEntryMessage(aplayer.Client);
                                b.SetStreamEntry(stream);
                                //PacketManager.Send(b);
                                b.Send();
                            }
                        }
                    }
                }
            }
        }
    }
}
