using CRepublic.Magic.Core;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure.Slots.Items;

namespace CRepublic.Magic.Packets.Commands.Client.Clan
{
    internal class Request_Alliance_Troops : Command
    {
        internal bool Have_Message;
        internal string Message = string.Empty;

        public Request_Alliance_Troops(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32(); // Ticks

            this.Have_Message = this.Reader.ReadBoolean();

            if (this.Have_Message) this.Message = this.Reader.ReadString();
        }

        internal override  void Process()
        {
            var Clan = Resources.Clans.Get(this.Device.Player.Avatar.ClanId, Constants.Database);
            foreach (var Old_Entry in Clan.Chats.Slots.FindAll(M => M.Sender_ID == this.Device.Player.Avatar.UserId && M.Stream_Type == Alliance_Stream.TROOP_REQUEST))
            {
                Clan.Chats.Remove(Old_Entry);
            }

            Clan.Chats.Add(
                new Entry
                {
                    Stream_Type = Alliance_Stream.TROOP_REQUEST,

                    Sender_ID = this.Device.Player.Avatar.UserId,
                    Sender_Name = this.Device.Player.Avatar.Name,
                    Sender_Level = this.Device.Player.Avatar.Level,
                    Sender_League = this.Device.Player.Avatar.League,
                    Sender_Role = Clan.Members[this.Device.Player.Avatar.UserId].Role,
                    Have_Message = this.Have_Message,
                    Message = this.Message,
                    Max_Troops = (CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(1000014) as Files.CSV_Logic.Buildings).HousingSpace[this.Device.Player.Avatar.Castle_Level],
                    Max_Spells = (CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(1000014) as Files.CSV_Logic.Buildings).HousingSpaceAlt[this.Device.Player.Avatar.Castle_Level],
                    Used_Space_Troops = this.Device.Player.Avatar.Castle_Used,
                    Used_Space_Spells = this.Device.Player.Avatar.Castle_Used_SP,
                    Units = this.Device.Player.Avatar.Castle_Units.Clone(),
                    Spells = this.Device.Player.Avatar.Castle_Spells.Clone(),

                });
        }
    }
}