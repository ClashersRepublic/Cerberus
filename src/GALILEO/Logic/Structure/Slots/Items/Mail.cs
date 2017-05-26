using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic.Enums;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class Mail
    {
        [JsonProperty("type")] internal Avatar_Stream Stream_Type;
        [JsonProperty("high_id")] internal int Message_HighID;
        [JsonProperty("low_id")] internal int Message_LowID;

        [JsonProperty("sender_id")] internal long Sender_ID;
        [JsonProperty("sender_name")] internal string Sender_Name;
        [JsonProperty("sender_league")] internal int Sender_League;
        [JsonProperty("sender_lvl")] internal int Sender_Level;
        [JsonProperty("date")] internal DateTime Sent = DateTime.UtcNow;

        [JsonProperty("alliance_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal long Alliance_ID;

        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Message = string.Empty;

        [JsonProperty("battle_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal long Battle_ID;
        [JsonIgnore]
        internal int GetTime => (int)DateTime.UtcNow.Subtract(this.Sent).TotalSeconds;

        internal Mail()
        {
            // Mail.
        }

        internal byte[] ToBytes
        {
            get
            {
                Battle Battle = null;
                if (this.Stream_Type == Avatar_Stream.ATTACK || this.Stream_Type == Avatar_Stream.DEFENSE)
                    Battle = Core.Resources.Battles.Get(Battle_ID, Constants.Database, false);

                List<byte> _Packet = new List<byte>();
                _Packet.AddInt((int)this.Stream_Type);
                switch (this.Stream_Type)
                {
                    case Avatar_Stream.ATTACK:
                        _Packet.AddLong(this.Battle_ID);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Battle.Defender.UserId);
                        _Packet.AddString(Battle.Defender.Name);
                        _Packet.AddInt(Battle.Defender.Level);
                        _Packet.AddInt(Battle.Defender.League);
                        break;
                    case Avatar_Stream.DEFENSE:
                        _Packet.AddLong(this.Battle_ID);
                        _Packet.AddBool(true);
                        _Packet.AddLong(Battle.Attacker.UserId);
                        _Packet.AddString(Battle.Attacker.Name);
                        _Packet.AddInt(Battle.Attacker.Level);
                        _Packet.AddInt(Battle.Attacker.League);
                        break;
                    default:
                        _Packet.AddInt(this.Message_HighID);
                        _Packet.AddInt(this.Message_LowID);
                        _Packet.AddBool(true);
                        _Packet.AddLong(this.Sender_ID);
                        _Packet.AddString(this.Sender_Name);
                        _Packet.AddInt(this.Sender_Level);
                        _Packet.AddInt(this.Sender_League);
                        break;
                }
                _Packet.AddInt(this.GetTime);
                _Packet.AddByte(0);

                switch (this.Stream_Type)
                {
                    case Avatar_Stream.ATTACK:
                    case Avatar_Stream.DEFENSE:
                        _Packet.AddString(Battle.Replay_Info.Json);
                        _Packet.AddInt(0);
                        _Packet.AddBool(true);
                        _Packet.AddInt(9);
                        _Packet.AddInt(24);
                        _Packet.AddInt(0);

                        _Packet.AddBool(true);
                        _Packet.AddLong(this.Battle_ID);
                        _Packet.AddInt(int.MaxValue);
                        break;

                    case Avatar_Stream.REMOVED_CLAN:
                        var clan1 = Core.Resources.Clans.Get(this.Alliance_ID, Constants.Database, false);
                        _Packet.AddString(this.Message);
                        _Packet.AddLong(this.Alliance_ID);
                        _Packet.AddString(clan1.Name);
                        _Packet.AddInt(clan1.Badge);
                        _Packet.AddBool(true);
                        _Packet.AddLong(this.Sender_ID);
                        break;

                    case Avatar_Stream.CLAN_MAIL:
                        var clan2 = Core.Resources.Clans.Get(this.Alliance_ID, Constants.Database, false);
                        _Packet.AddString(this.Message);
                        _Packet.AddBool(true);
                        _Packet.AddLong(this.Sender_ID);
                        _Packet.AddLong(this.Alliance_ID);
                        _Packet.AddString(clan2.Name);
                        _Packet.AddInt(clan2.Badge);
                        break;
                    case Avatar_Stream.INVITATION:
                        var clan3 = Core.Resources.Clans.Get(this.Alliance_ID, Constants.Database, false);
                        _Packet.AddLong(this.Alliance_ID);
                        _Packet.AddString(clan3.Name);
                        _Packet.AddInt(clan3.Badge);
                        _Packet.AddBool(true);
                        _Packet.AddLong(this.Sender_ID);
                        _Packet.AddInt(11);
                        _Packet.AddByte(0);
                        break;
                }
                return _Packet.ToArray();
            }
        }
    }
}
