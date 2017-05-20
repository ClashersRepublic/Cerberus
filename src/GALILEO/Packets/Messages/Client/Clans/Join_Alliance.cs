using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Commands.Server;
using BL.Servers.CoC.Packets.Messages.Server;
using BL.Servers.CoC.Packets.Messages.Server.Clans;

namespace BL.Servers.CoC.Packets.Messages.Client.Clans
{
    internal class Join_Alliance : Message
    { 
        internal long ClanID;

        public Join_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            this.ClanID = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            Clan Alliance = Resources.Clans.Get(this.ClanID, Constants.Database, false);

            if (Alliance != null)
            {
                Alliance.Members.Add(this.Device.Player.Avatar);
                this.Device.Player.Avatar.ClanId = Alliance.Clan_ID;
                this.Device.Player.Avatar.Alliance_Level = Alliance.Level;
                this.Device.Player.Avatar.Alliance_Name = Alliance.Name;
                this.Device.Player.Avatar.Alliance_Role = (int)Role.Member;
                this.Device.Player.Avatar.Badge_ID = Alliance.Badge;


                new Server_Commands(this.Device)
                {
                    Command = new Joined_Alliance(this.Device) { Clan = Alliance }.Handle()
                }.Send();

                new Alliance_All_Stream_Entry(this.Device).Send();

                Alliance.Chats.Add(new Entry
                {
                    Stream_Type = Alliance_Stream.EVENT,
                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = Role.Member,
                    Event_ID = Events.JOIN_ALLIANCE,
                    Event_Player_ID = this.Device.Player.Avatar.UserId,
                    Event_Player_Name = this.Device.Player.Avatar.Name
                });
            }
        }
    }
}
