using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.StreamEntries;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14321
    internal class TakeDecisionJoinRequestMessage : Message
    {
        public TakeDecisionJoinRequestMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public long MessageId { get; set; }

        public int Choice { get; set; }

        public override void Decode()
        {
            MessageId = Reader.ReadInt64();
            Choice = Reader.ReadByte();
        }

        public override void Process(Level level)
        {

            Alliance a = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            StreamEntry message = a.ChatMessages.Find(c => c.GetId() == MessageId);
            Level requester = ResourcesManager.GetPlayer(message.GetSenderId());
            if (Choice == 1)
            {
                if (!a.IsAllianceFull)
                {
                    requester.Avatar.SetAllianceId(a.AllianceId);

                    AllianceMemberEntry member = new AllianceMemberEntry(requester.Avatar.Id);
                    member.SetRole(1);

                    // For some reason this thing adds the same member twice.
                    a.AddAllianceMember(member);

                    StreamEntry e = a.ChatMessages.Find(c => c.GetId() == MessageId);
                    e.SetJudgeName(level.Avatar.GetAvatarName());
                    e.SetState(2);

                    AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                    eventStreamEntry.SetId(a.ChatMessages.Count + 1);
                    eventStreamEntry.SetSender(requester.Avatar);
                    eventStreamEntry.SetAvatarName(level.Avatar.GetAvatarName());
                    eventStreamEntry.SetAvatarId(level.Avatar.Id);
                    eventStreamEntry.SetEventType(2);

                    a.AddChatMessage(eventStreamEntry);

                    // New function for send a message
                    foreach (AllianceMemberEntry op in a.AllianceMembers)
                    {
                        Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                        if (player.Client!= null)
                        {
                            AllianceStreamEntryMessage c = new AllianceStreamEntryMessage(player.Client);
                            AllianceStreamEntryMessage p = new AllianceStreamEntryMessage(player.Client);
                            p.SetStreamEntry(eventStreamEntry);
                            c.SetStreamEntry(e);
                            p.Send();
                            c.Send();
                        }
                    }
                    if (ResourcesManager.IsPlayerOnline(requester))
                    {
                        JoinedAllianceCommand joinAllianceCommand = new JoinedAllianceCommand();
                        joinAllianceCommand.SetAlliance(a);

                        AvailableServerCommandMessage availableServerCommandMessage = new AvailableServerCommandMessage(requester.Client);
                        availableServerCommandMessage.SetCommandId(1);
                        availableServerCommandMessage.SetCommand(joinAllianceCommand);

                        AllianceRoleUpdateCommand d = new AllianceRoleUpdateCommand();
                        d.SetAlliance(a);
                        d.SetRole(4);
                        d.Tick(level);

                        AvailableServerCommandMessage c = new AvailableServerCommandMessage(Client);
                        c.SetCommandId(8);
                        c.SetCommand(d);

                        new AnswerJoinRequestAllianceMessage(Client).Send();
                        availableServerCommandMessage.Send();
                        c.Send();
                        new AllianceStreamMessage(requester.Client, a).Send();
                    }
                }
            }
            else
            {
                StreamEntry e = a.ChatMessages.Find(c => c.GetId() == MessageId);
                e.SetJudgeName(level.Avatar.GetAvatarName());
                e.SetState(3);

                // New function for send a message
                foreach (AllianceMemberEntry op in a.AllianceMembers)
                {
                    Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (player.Client!= null)
                    {
                        AllianceStreamEntryMessage c = new AllianceStreamEntryMessage(player.Client);
                        c.SetStreamEntry(e);
                        //PacketManager.Send(c);
                        c.Send();
                    }
                }
            }
        }

    }
}