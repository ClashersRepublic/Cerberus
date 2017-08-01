using System;
using System.IO;
using System.Linq;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.StreamEntries;
using Magic.ClashOfClans.Network.Commands.Server;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14308
    internal class LeaveAllianceMessage : Message
    {
        public static bool done;

        public LeaveAllianceMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Process(Level level)
        {
            var avatar = level.Avatar;
            var alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());

            lock (alliance)
            {
                if (avatar.GetAllianceRole() == 2 && alliance.AllianceMembers.Count > 1)
                {
                    var members = alliance.AllianceMembers;
                    foreach (AllianceMemberEntry player in members.Where(player => player.GetRole() >= 3))
                    {
                        player.SetRole(2);

                        if (ResourcesManager.IsPlayerOnline(ResourcesManager.GetPlayer(player.GetAvatarId())))
                        {
                            var c = new AllianceRoleUpdateCommand();
                            c.SetAlliance(alliance);
                            c.SetRole(2);
                            c.Tick(level);

                            var d = new AvailableServerCommandMessage(ResourcesManager.GetPlayer(player.GetAvatarId()).Client);
                            d.SetCommandId(8);
                            d.SetCommand(c);
                            d.Send();
                        }
                        done = true;
                        break;
                    }

                    if (!done)
                    {
                        var count = alliance.AllianceMembers.Count;
                        var rnd = new Random();
                        var id = rnd.Next(1, count);
                        while (id != level.Avatar.Id)
                            id = rnd.Next(1, count);
                        var loop = 0;
                        foreach (AllianceMemberEntry player in members)
                        {
                            loop++;
                            if (loop == id)
                            {
                                player.SetRole(2);
                                if (ResourcesManager.IsPlayerOnline(ResourcesManager.GetPlayer(player.GetAvatarId())))
                                {
                                    var e = new AllianceRoleUpdateCommand();
                                    e.SetAlliance(alliance);
                                    e.SetRole(2);
                                    e.Tick(level);

                                    var f = new AvailableServerCommandMessage(ResourcesManager.GetPlayer(player.GetAvatarId()).Client);
                                    f.SetCommandId(8);
                                    f.SetCommand(e);
                                    f.Send();
                                }
                                break;
                            }
                        }
                    }
                }

                var a = new LeavedAllianceCommand();
                a.SetAlliance(alliance);
                a.SetReason(1);

                var b = new AvailableServerCommandMessage(Client);
                b.SetCommandId(2);
                b.SetCommand(a);
                b.Send();

                alliance.RemoveMember(avatar.Id);
                avatar.SetAllianceId(0);

                if (alliance.AllianceMembers.Count > 0)
                {
                    var eventStreamEntry = new AllianceEventStreamEntry();
                    eventStreamEntry.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    eventStreamEntry.SetSender(avatar);
                    eventStreamEntry.SetEventType(4);
                    eventStreamEntry.SetAvatarId(avatar.Id);
                    eventStreamEntry.SetAvatarName(avatar.GetAvatarName());
                    alliance.AddChatMessage(eventStreamEntry);
                    foreach (Level onlinePlayer in ResourcesManager.OnlinePlayers)
                    {
                        if (onlinePlayer.Avatar.GetAllianceId() == alliance.AllianceId)
                        {
                            AllianceStreamEntryMessage p = new AllianceStreamEntryMessage(onlinePlayer.Client);
                            p.SetStreamEntry(eventStreamEntry);
                            p.Send();
                        }
                    }
                }
                else
                {
                    DatabaseManager.Instance.RemoveAlliance(alliance);
                }
                new LeaveAllianceOkMessage(Client, alliance).Send();
            }
        }
    }
}
