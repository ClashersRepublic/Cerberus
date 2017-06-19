using Republic.Magic.Core;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Logic.Structure.Slots.Items;

namespace Republic.Magic.Packets.Messages.Client.Clans
{
    internal class Request_Join_Alliance : Message
    {
        internal long AllianceID;
        internal string Message;

        public Request_Join_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }
        internal override void Decode()
        {
            this.AllianceID = this.Reader.ReadInt64();
            this.Message = this.Reader.ReadString();
        }

        internal override void Process()
        {
            if (AllianceID > 0)
            {
                var clan = Resources.Clans.Get(AllianceID, Constants.Database, false);
                clan?.Chats.Add(
                    new Entry
                    {
                        Stream_Type = Alliance_Stream.INVITATION,
                        Sender_ID = this.Device.Player.Avatar.UserId,
                        Sender_Name = this.Device.Player.Avatar.Name,
                        Sender_Level = this.Device.Player.Avatar.Level,
                        Sender_League = this.Device.Player.Avatar.League,
                        Sender_Role = Role.Member,
                        Message = this.Message
                    });
            }
        }
    }
}
