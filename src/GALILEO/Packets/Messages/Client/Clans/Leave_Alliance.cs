using Republic.Magic.Core;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure.Slots.Items;
using Republic.Magic.Packets.Commands.Server;
using Republic.Magic.Packets.Messages.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Republic.Magic.Packets.Messages.Client.Clans
{
    internal class Leave_Alliance : Message
    {
        internal bool done;
        public Leave_Alliance(Device device, Reader reader) : base(device, reader)
        {

        }
        internal override void Process()
        {
            Player User = this.Device.Player.Avatar;
            Clan Alliance = Resources.Clans.Get(User.ClanId, Constants.Database, false);

            if (Alliance != null)
            {
                if (Alliance.Members[User.UserId].Role == Role.Leader && Alliance.Members.Count > 1)
                {
                    foreach (Member player in Alliance.Members.Values.Where(player => player.Role >= Role.Elder))
                    {
                        player.Role = Role.Leader;

                        if (player.Player.Client != null)
                        {
                            new Server_Commands(player.Player.Client) { Command = new Role_Update(player.Player.Client) { Clan = Alliance, Role = (int)Role.Leader }.Handle() }.Send();
                        }
                        done = true;
                        break;
                    }

                    if (!done)
                    {
                        List<long> UserID = Alliance.Members.Keys.SkipWhile(P => P == User.UserId).ToList();

                        while (UserID.Count > 1)
                        {
                            int index = Resources.Random.Next(0, UserID.Count);
                            UserID.RemoveAt(index);
                        }
                        var lucky = Alliance.Members[UserID[0]];
                        lucky.Role = Role.Leader;
                        if (lucky.Player.Client != null)
                        {
                            new Server_Commands(lucky.Player.Client) { Command = new Role_Update(lucky.Player.Client) { Clan = Alliance, Role = (int)Role.Leader }.Handle() }.Send();
                        }
                    }
                }

                Alliance.Members.Remove(this.Device.Player.Avatar);
                this.Device.Player.Avatar.ClanId = 0;
                this.Device.Player.Avatar.Alliance_Level = -1;
                this.Device.Player.Avatar.Alliance_Name = string.Empty;
                this.Device.Player.Avatar.Alliance_Role = -1;
                this.Device.Player.Avatar.Badge_ID = -1;

                new Server_Commands(this.Device) { Command = new Leaved_Alliance(this.Device) { AllianceID = Alliance.Clan_ID, Reason = 1 }.Handle() }.Send();

                if (Alliance.Members.Count == 0)
                {
                    Resources.Clans.Delete(Alliance);
                }
                else
                {
                    Alliance.Chats.Add(new Entry
                    {
                        Stream_Type = Alliance_Stream.EVENT,
                        Sender_ID = this.Device.Player.Avatar.UserId,
                        Sender_Name = this.Device.Player.Avatar.Name,
                        Sender_Level = this.Device.Player.Avatar.Level,
                        Sender_League = this.Device.Player.Avatar.League,
                        Sender_Role = Role.Member,
                        Event_ID = Events.LEAVE_ALLIANCE,
                        Event_Player_ID = this.Device.Player.Avatar.UserId,
                        Event_Player_Name = this.Device.Player.Avatar.Name
                    });
                }


            }
        }
    }
}
