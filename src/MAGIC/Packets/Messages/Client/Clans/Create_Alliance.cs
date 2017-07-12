using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Commands.Server;
using CRepublic.Magic.Packets.Messages.Server;
using CRepublic.Magic.Packets.Messages.Server.Clans;
using CRepublic.Magic.Packets.Messages.Server.Errors;

namespace CRepublic.Magic.Packets.Messages.Client.Clans
{
    internal class Create_Alliance : Message
    {
        internal Clan Clan = Resources.Clans.New();
        public Create_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            this.Clan.Name = this.Reader.ReadString();
            this.Clan.Description = this.Reader.ReadString();
            this.Clan.Badge = this.Reader.ReadInt32();
            this.Clan.Type = (Hiring)this.Reader.ReadInt32();
            this.Clan.Required_Trophies = this.Reader.ReadInt32();
            this.Clan.War_Frequency = this.Reader.ReadInt32();
            this.Clan.Origin = this.Reader.ReadInt32();
            this.Clan.War_History = this.Reader.ReadByte() > 0;
            this.Clan.War_Amical = this.Reader.ReadByte() > 0;
        }

        internal override void Process()
        {
            int ResourceID = (CSV.Tables.Get(Gamefile.Resources).GetData((CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_RESOURCE") as Globals).TextValue) as Files.CSV_Logic.Resource).GetGlobalID();
            int Cost = (CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_COST") as Globals).NumberValue;

            if (this.Device.Player.Avatar.HasEnoughResources(ResourceID, Cost))
            {
                this.Device.Player.Avatar.Resources.Minus(ResourceID, Cost);
                this.Device.Player.Avatar.ClanId = Clan.Clan_ID;
                this.Device.Player.Avatar.Alliance_Level = Clan.Level;
                this.Device.Player.Avatar.Alliance_Name = Clan.Name;
                this.Device.Player.Avatar.Alliance_Role = (int)Role.Leader;
                this.Device.Player.Avatar.Badge_ID = Clan.Badge;

                this.Clan.Members.Add(this.Device.Player.Avatar);

                this.Clan.Chats.Add(new Entry
                {
                    Stream_Type = Alliance_Stream.EVENT,
                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = Role.Leader,
                    Event_ID = Events.JOIN_ALLIANCE,
                    Event_Player_ID = this.Device.Player.Avatar.UserId,
                    Event_Player_Name = this.Device.Player.Avatar.Name
                });

                this.Clan.Chats.Add(new Entry
                {
                    Stream_Type = Alliance_Stream.GIFT,
                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = Role.Leader,
                    Event_ID = Events.JOIN_ALLIANCE,
                    Event_Player_ID = this.Device.Player.Avatar.UserId,
                    Event_Player_Name = this.Device.Player.Avatar.Name
                });

                Resources.Clans.Save(this.Clan);

                new Alliance_Full_Entry(this.Device) { Clan = this.Clan }.Send();
                new Server_Commands(this.Device) { Command = new Joined_Alliance(this.Device) { Clan = this.Clan }.Handle() }.Send();
                new Server_Commands(this.Device) { Command = new Role_Update(this.Device) { Clan = this.Clan, Role = (int)Role.Leader }.Handle() }.Send();
            }
            else
            {
                new Out_Of_Sync(this.Device).Send();
                Resources.Clans.Delete(this.Clan);
                Loggers.Log("User tried to create alliance without enough [" + this.Device.Player.Avatar.UserId + ":" + GameUtils.GetHashtag(this.Device.Player.Avatar.UserId) + ']', true, Defcon.WARN);
            }
        }
    }
}
