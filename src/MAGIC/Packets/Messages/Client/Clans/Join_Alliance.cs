using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Commands.Server;
using CRepublic.Magic.Packets.Messages.Server;
using CRepublic.Magic.Packets.Messages.Server.Clans;

namespace CRepublic.Magic.Packets.Messages.Client.Clans
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

                new Alliance_Full_Entry(this.Device).Send();
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
