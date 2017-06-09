using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Messages.Server.Clans;

namespace BL.Servers.CoC.Packets.Commands.Client.Clan
{
    internal class Send_Mail : Command
    {
        internal string Message;
        internal int Tick;


        public Send_Mail(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            var AllianceID = this.Device.Player.Avatar.ClanId;

            if (AllianceID > 0)
            {
                var Clan = await Resources.Clans.Get(AllianceID, Constants.Database, false);
                if (Clan != null)
                {
                    var Mail = new Mail
                    {
                        Stream_Type = Logic.Enums.Avatar_Stream.CLAN_MAIL,
                        Sender_ID = this.Device.Player.Avatar.UserId,
                        Sender_Name = this.Device.Player.Avatar.Name,
                        Sender_Level = this.Device.Player.Avatar.Level,
                        Sender_League = this.Device.Player.Avatar.League,
                        Alliance_ID = AllianceID,
                        Message =  this.Message
                    };
                    foreach (Member Member in Clan.Members.Values)
                    {
                        Member.Player.Avatar.Inbox.Add(Mail);
                    }
                }
            }
        }
    }
}
