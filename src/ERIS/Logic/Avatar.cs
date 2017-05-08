using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.BB.Extensions.List;
using BL.Servers.BB.Files;
using BL.Servers.BB.Files.CSV_Logic;
using BL.Servers.BB.Logic.Enums;
using BL.Servers.BB.Logic.Slots;
using BL.Servers.BB.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BL.Servers.BB.Logic
{
    internal class Avatar
    {
        internal long UserId
        {
            get
            {
                return (((long)this.UserHighId << 32) | (this.UserLowId & 0xFFFFFFFFL));
            }
            set
            {
                this.UserHighId = Convert.ToInt32(value >> 32);
                this.UserLowId = (int)value;
            }
        }

        [JsonProperty("acc_hi")] internal int UserHighId;
        [JsonProperty("acc_lo")] internal int UserLowId;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;


        [JsonProperty("name")] internal string Username = "Commander";
        [JsonProperty("IpAddress")] internal string IpAddress;
        [JsonProperty("region")] internal string Region;

        [JsonProperty("lvl")] internal int Level = 1;
        [JsonProperty("xp")] internal int Experience;
        [JsonProperty("tutorials")] internal List<int> Tutorials = new List<int>();
        //[JsonProperty("tutorial")] internal int Tutorial = 0;

        [JsonProperty("HQ_lvl")] internal int HQ_Level = 0;

        [JsonProperty("resources")] internal Resources Resources;
        [JsonProperty("resources_cap")] internal Resources Resources_Cap;


        [JsonProperty("account_locked")] internal bool Locked = false;

        [JsonProperty("last_tick")] internal DateTime LastTick = DateTime.UtcNow;
        [JsonProperty("update_date")] internal DateTime Update = DateTime.UtcNow;
        [JsonProperty("creation_date")] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty("ban_date")] internal DateTime BanTime = DateTime.UtcNow;


        internal bool Banned => this.BanTime > DateTime.UtcNow;

        internal Avatar()
        {

            this.Resources = new Resources(this);
            this.Resources_Cap = new Resources(this);
        }

        internal Avatar(long UserId)
        {
            this.UserId = UserId;    

            this.Resources = new Resources(this, true);
            this.Resources_Cap = new Resources(this, false);
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(this.UserId);
                _Packet.AddLong(this.UserId);

                _Packet.AddInt(0);
                _Packet.AddBool(false);

                _Packet.AddString(this.Username);
                _Packet.AddString(null);

                _Packet.AddInt(this.Level);
                _Packet.AddInt(this.Experience); //Exp

                _Packet.AddInt(this.Resources.Gems);
                _Packet.AddInt(this.Resources.Gems);
                _Packet.AddInt(0); //Shield?
                _Packet.AddInt(0); //Shield?
                _Packet.AddInt(100); //Trophies

                _Packet.AddInt(0);
                _Packet.AddByte(0); //Huh supercell?
                _Packet.AddInt(0);
                _Packet.AddInt(0); //Resource Cap
          
                _Packet.AddRange(this.Resources.ToBytes);

                _Packet.AddInt(1); //Units
                _Packet.AddInt(CSV.Tables.Get(Gamefile.Characters).GetData("Heavy").GetGlobalID());
                _Packet.AddInt(2);

                // UK
                _Packet.AddInt(0);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddInt(0);//Building id is in this

                _Packet.AddInt(0); // UK

                _Packet.AddInt(0); //28000000 stuff // UK

                _Packet.AddInt(0); // UK

                /*_Packet.AddInt(this.Tutorials.Count);
                foreach (var Tutorial in this.Tutorials)
                {
                    _Packet.AddInt(Tutorial);
                }*/
                _Packet.AddInt(11);

                for (int i = 0; i < 11; i++)
                    _Packet.AddInt(21000000 + i); //Tutorial

                _Packet.AddInt(0); // UK

                _Packet.AddInt(0); //23000001 stuff // UK

                _Packet.AddInt(0); // UK
                _Packet.AddInt(0); // UK
                _Packet.AddInt(0); // UK
                _Packet.AddInt(0); // UK
                _Packet.AddInt(0); // UK
                _Packet.AddInt(0); // NPC List
                _Packet.AddRange(
                    "01000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000000000000000000000000000000000000000007FFFFFFFF0000000000FFFFFFFF00000000FFFFFFFF0000000000000000000000010000000D5449445F5455544F5249414C3200000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000003FFFFFFFF0000000000FFFFFFFF00000000FFFFFFFF0000000000000000000000020000000D5449445F5455544F5249414C3300000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000003FFFFFFFF0000000000FFFFFFFF00000000FFFFFFFF00000000000000000000000000000000000000000000000000000000000000000000000000000006FFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFF0000000000FFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000000000FFFFFFFF00000000040000000000000000FFFFFFFFFFFFFFFF00000000000000000000000000FFFFFFFFEA5B9A1B58F383350000000000005460000000024D5900000005506572616B"
                        .HexaToBytes());

                return _Packet.ToArray();
            }
        }
    }
}
