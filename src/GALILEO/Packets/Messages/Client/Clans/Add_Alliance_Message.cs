using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure.Slots.Items;

namespace CRepublic.Magic.Packets.Messages.Client.Clans
{
    internal class Add_Alliance_Message : Message
    {
        internal string Message = string.Empty;

        public Add_Alliance_Message(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
        }

        internal override void Process()
        {
            Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database).Chats.Add(
                new Entry
                {
                    Stream_Type = Logic.Enums.Alliance_Stream.CHAT,
                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database).Members[this.Device.Player.Avatar.UserId].Role,
                    Message = this.Message
                });
        }
    }
}
