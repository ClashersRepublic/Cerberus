using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure.Slots.Items;
using Republic.Magic.Packets.Commands.Server;
using Republic.Magic.Packets.Messages.Server;

namespace Republic.Magic.Packets.Messages.Client.Clans
{
    internal class Edit_Alliance_Settings : Message
    {

        internal string Message = string.Empty;

        internal Clan Clan;

        public Edit_Alliance_Settings(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.Clan = Core.Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database, false);

            this.Clan.Description = this.Reader.ReadString();
            this.Reader.ReadInt32();
            this.Clan.Badge = this.Reader.ReadInt32();
            this.Clan.Type = (Hiring)this.Reader.ReadInt32();
            this.Clan.Required_Trophies = this.Reader.ReadInt32();
            this.Clan.War_Frequency = this.Reader.ReadInt32();
            this.Clan.Origin = this.Reader.ReadInt32();

            switch (this.Reader.ReadByte())
            {
                case 0:
                    this.Clan.War_History = false;
                    this.Clan.War_Amical = false;
                    break;
                case 1:
                    this.Clan.War_History = true;
                    this.Clan.War_Amical = false;
                    break;
                case 2:
                    this.Clan.War_History = false;
                    this.Clan.War_Amical = true;
                    break;
                case 3:
                    this.Clan.War_History = true;
                    this.Clan.War_Amical = true;
                    break;
            }
        }

        internal override void Process()
        {
            this.Clan.Chats.Add(
                new Entry
                {
                    Stream_Type = Alliance_Stream.EVENT,
                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = this.Clan.Members[this.Device.Player.Avatar.UserId].Role,
                    Event_ID = Events.UPDATE_SETTINGS,
                    Event_Player_ID = this.Device.Player.Avatar.UserId,
                    Event_Player_Name = this.Device.Player.Avatar.Name
                });

            new Server_Commands(this.Device) { Command = new Changed_Alliance_Setting(this.Device) { Clan = this.Clan }.Handle() }.Send();
        }
    }
}