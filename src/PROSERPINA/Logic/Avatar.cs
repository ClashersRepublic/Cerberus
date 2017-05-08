using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Files;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Logic.Slots;
using BL.Servers.CR.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BL.Servers.CR.Logic
{
    internal class Avatar
    {
        [JsonIgnore]
        internal Device Device;
        [JsonIgnore]
        internal long BattleID;

        #region Long Ids

        internal long UserId
        {
            get => (((long) this.UserHighId << 32) | (this.UserLowId & 0xFFFFFFFFL));
            set
            {
                this.UserHighId = Convert.ToInt32(value >> 32);
                this.UserLowId = (int) value;
            }
        }

        internal long ClanId
        {
            get => (((long) this.ClanHighID << 32) | (this.ClanLowID & 0xFFFFFFFFL));
            set
            {
                this.ClanHighID = Convert.ToInt32(value >> 32);
                this.ClanLowID = (int) value;
            }
        }

        #endregion

        [JsonProperty("acc_hi")] internal int UserHighId;
        [JsonProperty("acc_lo")] internal int UserLowId;

        [JsonProperty("clan_hi")] internal int ClanHighID;
        [JsonProperty("clan_lo")] internal int ClanLowID;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;

        [JsonProperty("name")] internal string Username = String.Empty;
        [JsonProperty("IpAddress")] internal string IpAddress;
        [JsonProperty("region")] internal string Region;

        [JsonProperty("lvl")] internal int Level = 1;
        [JsonProperty("xp")] internal int Experience;
        [JsonProperty("tutorials")] internal int Tutorial = 0x06;
        [JsonProperty("changes")] internal int Changes;

        [JsonProperty("trophies")] internal int Trophies = 6500;

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

        internal byte[] Components
        {
            get
            {
                int TimeStamp = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(UserId);

                _Packet.AddVInt(0);

                _Packet.AddVInt(0);

                _Packet.AddVInt(1698340);
                _Packet.AddVInt(1727920);

                _Packet.AddVInt(TimeStamp);

                #region WIP

                //_Packet.Add(0);

                //_Packet.Add(1);
                //{
                //    _Packet.AddVInt(8);

                //    foreach (Card _Card in this.Decks.GetRange(0, 8))
                //    {
                //        _Packet.AddVInt(_Card.GlobalID);
                //    }
                //}

                //_Packet.Add(255);

                //_Packet.AddRange(this.Decks.ToBytes());

                //_Packet.AddVInt(this.Decks.Count - 8);

                //foreach (Card _Card in this.Decks.Skip(8))
                //{
                //                    _Packet.AddVInt(_Card.Type);
                //_Packet.AddVInt(_Card.ID);
                //_Packet.AddVInt(_Card.Level);
                //_Packet.AddVInt(0);
                //_Packet.AddVInt(_Card.Count);
                //_Packet.AddVInt(0);
                //_Packet.AddVInt(0);
                //_Packet.AddVInt(_Card.New);
                //}

                //_Packet.Add(0);
                //_Packet.Add(4);

                #endregion

                _Packet.AddHexa(
                    "00 03 08 80 EA E5 18 81 EA E5 18 8D EA E5 18 81 FC D9 1A 80 FC D9 1A 83 EA E5 18 8E EA E5 18 90 EA E5 18 08 80 EA E5 18 81 EA E5 18 8D EA E5 18 81 FC D9 1A 80 FC D9 1A 83 EA E5 18 00 00 08 80 EA E5 18 81 EA E5 18 8D EA E5 18 81 FC D9 1A 80 FC D9 1A 83 EA E5 18 00 00"
                        .Replace(" ", ""));

                _Packet.AddHexa(
                    "FF 01 01 00 01 01 00 00 02 01 00 00 00 00 00 0E 01 00 01 00 00 00 82 01 01 00 00 00 00 00 81 01 00 00 01 00 00 00 04 00 00 01 00 00 00 0F 00 9D 86 DE 17 01 01 00 02 11 00 9F 86 DE 17"
                        .Replace(" ", ""));

                _Packet.AddHexa(
                    "01 01 00 02 00 00 00 00 7F 7F 95 87 8B 90 0B 01 00 05 22 00 00 00 15 4D 75 73 6B 65 74 65 65 72 20 53 74 61 63 6B 20 4F 66 66 65 72 01 90 9C B9 8F 0B 90 E2 C3 8F 0B 90 9C B9 8F 0B 00 00 00 00 00 00 00 00 00 00 00 15 4D 75 73 6B 65 74 65 65 72 20 53 74 61 63 6B 20 4F 66 66 65 72 00 00 02 CD 7B 22 54 69 74 6C 65 22 3A 22 4D 75 73 6B 65 74 65 65 72 20 4F 66 66 65 72 21 22 2C 22 53 75 62 74 69 74 6C 65 22 3A 22 54 68 69 73 20 6C 69 6D 69 74 65 64 20 6F 66 66 65 72 20 69 73 20 61 20 4F 4E 45 20 74 69 6D 65 20 70 75 72 63 68 61 73 65 21 22 2C 22 53 68 6F 70 4F 66 66 65 72 73 22 3A 5B 7B 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 54 79 70 65 22 3A 22 73 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 32 2C 22 53 70 65 6C 6C 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 7D 5D 2C 22 54 69 74 6C 65 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 2C 22 49 6D 61 67 65 22 3A 7B 22 50 61 74 68 22 3A 22 5C 2F 62 65 62 35 34 39 64 37 35 38 33 34 37 62 38 36 66 38 32 38 65 38 36 36 33 64 65 64 33 33 35 66 5F 73 68 6F 70 5F 6D 75 73 6B 65 74 65 65 72 5F 73 74 61 63 6B 5F 30 31 2E 70 6E 67 22 2C 22 43 68 65 63 6B 73 75 6D 22 3A 22 62 65 62 35 34 39 64 37 35 38 33 34 37 62 38 36 66 38 32 38 65 38 36 36 33 64 65 64 33 33 35 66 22 2C 22 46 69 6C 65 22 3A 22 73 68 6F 70 5F 6D 75 73 6B 65 74 65 65 72 5F 73 74 61 63 6B 5F 30 31 2E 70 6E 67 22 7D 2C 22 43 6F 73 74 54 79 70 65 22 3A 22 67 65 6D 73 22 2C 22 43 6F 73 74 22 3A 35 30 2C 22 54 65 6D 70 6C 61 74 65 22 3A 22 73 68 6F 70 5F 69 74 65 6D 32 5F 6E 6F 5F 74 65 78 74 22 7D 2C 7B 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 54 79 70 65 22 3A 22 73 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 30 30 2C 22 53 70 65 6C 6C 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 7D 5D 2C 22 54 69 74 6C 65 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 2C 22 49 6D 61 67 65 22 3A 7B 22 50 61 74 68 22 3A 22 5C 2F 61 37 35 65 31 62 64 35 36 38 32 38 61 31 64 61 62 31 30 38 66 62 61 31 66 38 66 36 36 31 65 64 5F 73 68 6F 70 5F 6D 75 73 6B 65 74 65 65 72 5F 73 74 61 63 6B 5F 30 32 2E 70 6E 67 22 2C 22 43 68 65 63 6B 73 75 6D 22 3A 22 61 37 35 65 31 62 64 35 36 38 32 38 61 31 64 61 62 31 30 38 66 62 61 31 66 38 66 36 36 31 65 64 22 2C 22 46 69 6C 65 22 3A 22 73 68 6F 70 5F 6D 75 73 6B 65 74 65 65 72 5F 73 74 61 63 6B 5F 30 32 2E 70 6E 67 22 7D 2C 22 43 6F 73 74 54 79 70 65 22 3A 22 67 65 6D 73 22 2C 22 43 6F 73 74 22 3A 33 38 30 2C 22 54 65 6D 70 6C 61 74 65 22 3A 22 73 68 6F 70 5F 69 74 65 6D 32 5F 6E 6F 5F 74 65 78 74 22 7D 5D 7D 23 00 00 00 18 4D 75 73 6B 65 74 65 65 72 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 02 90 9C B9 8F 0B 90 E2 C3 8F 0B 90 9C B9 8F 0B 00 00 00 00 00 00 00 00 00 00 00 18 4D 75 73 6B 65 74 65 65 72 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 00 00 03 7D 7B 22 47 61 6D 65 4D 6F 64 65 22 3A 22 43 61 72 64 52 65 6C 65 61 73 65 44 72 61 66 74 22 2C 22 54 69 74 6C 65 22 3A 22 4D 75 73 6B 65 74 65 65 72 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 22 2C 22 4A 6F 69 6E 43 6F 73 74 22 3A 31 30 2C 22 4A 6F 69 6E 43 6F 73 74 52 65 73 6F 75 72 63 65 22 3A 22 47 65 6D 73 22 2C 22 4D 61 78 4C 6F 73 73 65 73 22 3A 33 2C 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 47 6F 6C 64 22 3A 31 33 30 2C 22 43 61 72 64 73 22 3A 32 7D 2C 7B 22 47 6F 6C 64 22 3A 31 38 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 33 2C 22 53 70 65 6C 6C 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 7D 2C 22 43 61 72 64 73 22 3A 33 7D 2C 7B 22 47 6F 6C 64 22 3A 32 34 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 35 2C 22 53 70 65 6C 6C 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 7D 2C 22 43 61 72 64 73 22 3A 35 7D 2C 7B 22 47 6F 6C 64 22 3A 33 31 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 38 2C 22 53 70 65 6C 6C 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 7D 2C 22 43 61 72 64 73 22 3A 38 7D 5D 2C 22 49 63 6F 6E 45 78 70 6F 72 74 4E 61 6D 65 22 3A 22 69 63 6F 6E 5F 74 6F 75 72 6E 61 6D 65 6E 74 5F 63 61 72 64 5F 72 65 6C 65 61 73 65 22 2C 22 57 69 6E 49 63 6F 6E 45 78 70 6F 72 74 4E 61 6D 65 22 3A 22 74 6F 75 72 6E 61 6D 65 6E 74 5F 6F 70 65 6E 5F 77 69 6E 73 5F 62 61 64 67 65 5F 64 72 61 66 74 22 2C 22 41 72 65 6E 61 22 3A 22 41 6C 6C 22 2C 22 4D 69 6C 65 73 74 6F 6E 65 48 69 67 68 6C 69 67 68 74 49 6E 55 49 22 3A 33 2C 22 53 75 62 74 69 74 6C 65 22 3A 22 57 69 6E 20 4D 75 73 6B 65 74 65 65 72 73 21 22 2C 22 44 65 73 63 72 69 70 74 69 6F 6E 22 3A 22 57 69 6E 20 4D 75 73 6B 65 74 65 65 72 73 20 74 68 72 6F 75 67 68 20 6F 6E 65 20 74 69 6D 65 20 72 65 77 61 72 64 73 20 61 6E 64 20 63 6F 6D 70 6C 65 74 65 20 74 68 69 73 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 20 61 74 20 33 20 77 69 6E 73 21 22 2C 22 53 74 61 72 74 4E 6F 74 69 66 69 63 61 74 69 6F 6E 22 3A 22 50 6C 61 79 20 32 34 68 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 20 61 6E 64 20 63 68 65 63 6B 20 74 68 65 20 53 68 6F 70 20 66 6F 72 20 63 61 72 64 20 73 74 61 63 6B 73 21 22 2C 22 45 6E 64 4E 6F 74 69 66 69 63 61 74 69 6F 6E 22 3A 22 54 77 6F 20 68 6F 75 72 73 20 6C 65 66 74 20 74 6F 20 70 6C 61 79 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 20 61 6E 64 20 67 65 74 20 74 68 65 20 63 61 72 64 20 73 74 61 63 6B 73 20 66 72 6F 6D 20 74 68 65 20 73 68 6F 70 21 22 2C 22 43 61 72 64 54 68 65 6D 65 22 3A 22 4D 75 73 6B 65 74 65 65 72 22 2C 22 46 72 65 65 50 61 73 73 22 3A 30 7D 24 00 00 00 21 48 65 61 6C 20 43 61 72 64 20 52 65 6C 65 61 73 65 20 43 68 61 6C 6C 65 6E 67 65 20 46 69 6E 61 6C 02 B0 A1 97 90 0B B0 F3 B6 90 0B B0 89 ED 8F 0B 00 00 00 00 00 00 00 00 00 00 00 21 48 65 61 6C 20 43 61 72 64 20 52 65 6C 65 61 73 65 20 43 68 61 6C 6C 65 6E 67 65 20 46 69 6E 61 6C 00 00 05 0F 7B 22 47 61 6D 65 4D 6F 64 65 22 3A 22 43 61 72 64 52 65 6C 65 61 73 65 44 72 61 66 74 22 2C 22 54 69 74 6C 65 22 3A 22 48 65 61 6C 20 44 72 61 66 74 20 43 68 61 6C 6C 65 6E 67 65 22 2C 22 46 72 65 65 50 61 73 73 22 3A 31 2C 22 4A 6F 69 6E 43 6F 73 74 22 3A 31 30 30 2C 22 4A 6F 69 6E 43 6F 73 74 52 65 73 6F 75 72 63 65 22 3A 22 47 65 6D 73 22 2C 22 4D 61 78 4C 6F 73 73 65 73 22 3A 33 2C 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 47 6F 6C 64 22 3A 37 30 30 2C 22 43 61 72 64 73 22 3A 31 30 7D 2C 7B 22 47 6F 6C 64 22 3A 39 35 30 2C 22 43 61 72 64 73 22 3A 31 35 7D 2C 7B 22 47 6F 6C 64 22 3A 31 32 35 30 2C 22 43 61 72 64 73 22 3A 32 35 7D 2C 7B 22 47 6F 6C 64 22 3A 31 36 30 30 2C 22 43 61 72 64 73 22 3A 34 32 7D 2C 7B 22 47 6F 6C 64 22 3A 32 30 30 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 47 6F 6C 64 22 2C 22 41 6D 6F 75 6E 74 22 3A 32 35 30 30 7D 2C 22 43 61 72 64 73 22 3A 36 35 7D 2C 7B 22 47 6F 6C 64 22 3A 32 35 30 30 2C 22 43 61 72 64 73 22 3A 39 32 7D 2C 7B 22 47 6F 6C 64 22 3A 33 31 30 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 30 2C 22 53 70 65 6C 6C 22 3A 22 48 65 61 6C 22 7D 2C 22 43 61 72 64 73 22 3A 31 32 35 7D 2C 7B 22 47 6F 6C 64 22 3A 33 38 30 30 2C 22 43 61 72 64 73 22 3A 31 36 35 7D 2C 7B 22 47 6F 6C 64 22 3A 34 36 35 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 43 68 65 73 74 22 3A 22 47 69 61 6E 74 5F 3C 63 75 72 72 65 6E 74 5F 61 72 65 6E 61 3E 22 2C 22 54 79 70 65 22 3A 22 43 68 65 73 74 22 7D 2C 22 43 61 72 64 73 22 3A 32 31 30 7D 2C 7B 22 47 6F 6C 64 22 3A 37 31 30 30 2C 22 43 61 72 64 73 22 3A 32 36 35 7D 2C 7B 22 47 6F 6C 64 22 3A 38 37 35 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 47 6F 6C 64 22 2C 22 41 6D 6F 75 6E 74 22 3A 32 35 30 30 30 7D 2C 22 43 61 72 64 73 22 3A 33 33 35 7D 2C 7B 22 47 6F 6C 64 22 3A 31 36 33 30 2C 22 43 61 72 64 73 22 3A 34 33 30 7D 2C 7B 22 47 6F 6C 64 22 3A 31 31 30 30 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 30 30 2C 22 53 70 65 6C 6C 22 3A 22 48 65 61 6C 22 7D 2C 22 43 61 72 64 73 22 3A 35 35 30 7D 5D 2C 22 49 63 6F 6E 45 78 70 6F 72 74 4E 61 6D 65 22 3A 22 69 63 6F 6E 5F 74 6F 75 72 6E 61 6D 65 6E 74 5F 63 61 72 64 5F 72 65 6C 65 61 73 65 22 2C 22 43 61 72 64 54 68 65 6D 65 22 3A 22 48 65 61 6C 22 2C 22 57 69 6E 49 63 6F 6E 45 78 70 6F 72 74 4E 61 6D 65 22 3A 22 74 6F 75 72 6E 61 6D 65 6E 74 5F 6F 70 65 6E 5F 77 69 6E 73 5F 62 61 64 67 65 5F 64 72 61 66 74 22 2C 22 41 72 65 6E 61 22 3A 22 41 6C 6C 22 2C 22 53 75 62 74 69 74 6C 65 22 3A 22 55 6E 6C 6F 63 6B 20 61 20 4E 65 77 20 43 61 72 64 21 22 2C 22 44 65 73 63 72 69 70 74 69 6F 6E 22 3A 22 50 69 63 6B 20 34 20 63 61 72 64 73 20 61 6E 64 20 72 65 63 65 69 76 65 20 34 20 66 72 6F 6D 20 79 6F 75 72 20 6F 70 70 6F 6E 65 6E 74 3B 20 6F 6E 65 20 6F 66 20 79 6F 75 20 77 69 6C 6C 20 67 65 74 20 74 6F 20 70 6C 61 79 20 77 69 74 68 20 74 68 65 20 48 65 61 6C 20 73 70 65 6C 6C 21 20 43 6F 6C 6C 65 63 74 20 6F 6E 65 20 74 69 6D 65 20 72 65 77 61 72 64 73 20 61 73 20 79 6F 75 20 70 72 6F 67 72 65 73 73 20 61 6E 64 20 75 6E 6C 6F 63 6B 20 74 68 69 73 20 6E 65 77 20 63 61 72 64 20 65 61 72 6C 79 21 22 2C 22 53 74 61 72 74 4E 6F 74 69 66 69 63 61 74 69 6F 6E 22 3A 22 48 65 61 6C 20 44 72 61 66 74 20 43 68 61 6C 6C 65 6E 67 65 20 68 61 73 20 62 65 67 75 6E 21 20 55 6E 6C 6F 63 6B 20 61 20 6E 65 77 20 63 61 72 64 20 65 61 72 6C 79 21 22 2C 22 45 6E 64 4E 6F 74 69 66 69 63 61 74 69 6F 6E 22 3A 22 48 65 61 6C 20 44 72 61 66 74 20 43 68 61 6C 6C 65 6E 67 65 20 65 6E 64 73 20 69 6E 20 74 77 6F 20 68 6F 75 72 73 21 20 4C 61 73 74 20 63 68 61 6E 63 65 20 74 6F 20 75 6E 6C 6F 63 6B 20 74 68 69 73 20 6E 65 77 20 63 61 72 64 20 65 61 72 6C 79 21 22 2C 22 4D 69 6C 65 73 74 6F 6E 65 48 69 67 68 6C 69 67 68 74 49 6E 55 49 22 3A 31 32 7D 25 00 00 00 15 50 72 69 6E 63 65 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 02 B0 95 82 90 0B B0 DB 8C 90 0B B0 95 82 90 0B 00 00 00 00 00 00 00 00 00 00 00 15 50 72 69 6E 63 65 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 00 00 03 6C 7B 22 47 61 6D 65 4D 6F 64 65 22 3A 22 43 61 72 64 52 65 6C 65 61 73 65 44 72 61 66 74 22 2C 22 54 69 74 6C 65 22 3A 22 50 72 69 6E 63 65 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 22 2C 22 4A 6F 69 6E 43 6F 73 74 22 3A 31 30 2C 22 4A 6F 69 6E 43 6F 73 74 52 65 73 6F 75 72 63 65 22 3A 22 47 65 6D 73 22 2C 22 4D 61 78 4C 6F 73 73 65 73 22 3A 33 2C 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 47 6F 6C 64 22 3A 31 33 30 2C 22 43 61 72 64 73 22 3A 32 7D 2C 7B 22 47 6F 6C 64 22 3A 31 38 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 2C 22 53 70 65 6C 6C 22 3A 22 50 72 69 6E 63 65 22 7D 2C 22 43 61 72 64 73 22 3A 33 7D 2C 7B 22 47 6F 6C 64 22 3A 32 34 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 2C 22 53 70 65 6C 6C 22 3A 22 50 72 69 6E 63 65 22 7D 2C 22 43 61 72 64 73 22 3A 35 7D 2C 7B 22 47 6F 6C 64 22 3A 33 31 30 2C 22 4D 69 6C 65 73 74 6F 6E 65 22 3A 7B 22 54 79 70 65 22 3A 22 53 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 31 2C 22 53 70 65 6C 6C 22 3A 22 50 72 69 6E 63 65 22 7D 2C 22 43 61 72 64 73 22 3A 38 7D 5D 2C 22 49 63 6F 6E 45 78 70 6F 72 74 4E 61 6D 65 22 3A 22 69 63 6F 6E 5F 74 6F 75 72 6E 61 6D 65 6E 74 5F 63 61 72 64 5F 72 65 6C 65 61 73 65 22 2C 22 57 69 6E 49 63 6F 6E 45 78 70 6F 72 74 4E 61 6D 65 22 3A 22 74 6F 75 72 6E 61 6D 65 6E 74 5F 6F 70 65 6E 5F 77 69 6E 73 5F 62 61 64 67 65 5F 64 72 61 66 74 22 2C 22 41 72 65 6E 61 22 3A 22 41 6C 6C 22 2C 22 4D 69 6C 65 73 74 6F 6E 65 48 69 67 68 6C 69 67 68 74 49 6E 55 49 22 3A 33 2C 22 53 75 62 74 69 74 6C 65 22 3A 22 57 69 6E 20 50 72 69 6E 63 65 21 22 2C 22 44 65 73 63 72 69 70 74 69 6F 6E 22 3A 22 57 69 6E 20 50 72 69 6E 63 65 20 63 61 72 64 73 20 74 68 72 6F 75 67 68 20 6F 6E 65 20 74 69 6D 65 20 72 65 77 61 72 64 73 20 61 6E 64 20 63 6F 6D 70 6C 65 74 65 20 74 68 69 73 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 20 61 74 20 33 20 77 69 6E 73 21 22 2C 22 53 74 61 72 74 4E 6F 74 69 66 69 63 61 74 69 6F 6E 22 3A 22 50 6C 61 79 20 32 34 68 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 20 61 6E 64 20 63 68 65 63 6B 20 74 68 65 20 53 68 6F 70 20 66 6F 72 20 63 61 72 64 20 73 74 61 63 6B 73 21 22 2C 22 45 6E 64 4E 6F 74 69 66 69 63 61 74 69 6F 6E 22 3A 22 54 77 6F 20 68 6F 75 72 73 20 6C 65 66 74 20 74 6F 20 70 6C 61 79 20 43 61 72 64 20 43 68 61 6C 6C 65 6E 67 65 20 61 6E 64 20 67 65 74 20 74 68 65 20 63 61 72 64 20 73 74 61 63 6B 73 20 66 72 6F 6D 20 74 68 65 20 73 68 6F 70 21 22 2C 22 43 61 72 64 54 68 65 6D 65 22 3A 22 50 72 69 6E 63 65 22 2C 22 46 72 65 65 50 61 73 73 22 3A 30 7D 26 00 00 00 12 50 72 69 6E 63 65 20 53 74 61 63 6B 20 4F 66 66 65 72 01 B0 95 82 90 0B B0 DB 8C 90 0B B0 95 82 90 0B 00 00 00 00 00 00 00 00 00 00 00 12 50 72 69 6E 63 65 20 53 74 61 63 6B 20 4F 66 66 65 72 00 00 02 5C 7B 22 54 69 74 6C 65 22 3A 22 50 72 69 6E 63 65 20 4F 66 66 65 72 21 22 2C 22 53 75 62 74 69 74 6C 65 22 3A 22 54 68 69 73 20 6C 69 6D 69 74 65 64 20 6F 66 66 65 72 20 69 73 20 61 20 4F 4E 45 20 74 69 6D 65 20 70 75 72 63 68 61 73 65 21 22 2C 22 53 68 6F 70 4F 66 66 65 72 73 22 3A 5B 7B 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 54 79 70 65 22 3A 22 73 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 33 2C 22 53 70 65 6C 6C 22 3A 22 50 72 69 6E 63 65 22 7D 5D 2C 22 49 6D 61 67 65 22 3A 7B 22 50 61 74 68 22 3A 22 5C 2F 65 30 66 35 65 32 66 65 33 33 30 62 66 65 35 35 62 38 62 38 63 64 34 30 64 61 62 62 36 31 31 62 22 2C 22 43 68 65 63 6B 73 75 6D 22 3A 22 65 30 66 35 65 32 66 65 33 33 30 62 66 65 35 35 62 38 62 38 63 64 34 30 64 61 62 62 36 31 31 62 22 2C 22 46 69 6C 65 22 3A 22 73 68 6F 70 5F 70 72 69 6E 63 65 5F 73 74 61 63 6B 5F 30 31 2E 70 6E 67 22 7D 2C 22 43 6F 73 74 54 79 70 65 22 3A 22 67 65 6D 73 22 2C 22 43 6F 73 74 22 3A 32 30 30 2C 22 54 65 6D 70 6C 61 74 65 22 3A 22 73 68 6F 70 5F 69 74 65 6D 32 5F 6E 6F 5F 74 65 78 74 22 7D 2C 7B 22 52 65 77 61 72 64 73 22 3A 5B 7B 22 54 79 70 65 22 3A 22 73 70 65 6C 6C 22 2C 22 41 6D 6F 75 6E 74 22 3A 36 2C 22 53 70 65 6C 6C 22 3A 22 50 72 69 6E 63 65 22 7D 5D 2C 22 49 6D 61 67 65 22 3A 7B 22 50 61 74 68 22 3A 22 5C 2F 35 35 32 64 35 38 34 64 39 30 30 35 64 62 34 62 66 38 37 37 35 36 37 62 35 36 30 65 33 66 62 63 22 2C 22 43 68 65 63 6B 73 75 6D 22 3A 22 35 35 32 64 35 38 34 64 39 30 30 35 64 62 34 62 66 38 37 37 35 36 37 62 35 36 30 65 33 66 62 63 22 2C 22 46 69 6C 65 22 3A 22 73 68 6F 70 5F 70 72 69 6E 63 65 5F 73 74 61 63 6B 5F 30 32 2E 70 6E 67 22 7D 2C 22 43 6F 73 74 54 79 70 65 22 3A 22 67 65 6D 73 22 2C 22 43 6F 73 74 22 3A 33 38 30 2C 22 54 65 6D 70 6C 61 74 65 22 3A 22 73 68 6F 70 5F 69 74 65 6D 32 5F 6E 6F 5F 74 65 78 74 22 7D 5D 7D 00 00 00 00 00 00 7F 00 00 00 05 22 01 23 01 24 01 25 01 26 05 00 00 00 48 7B 22 49 44 22 3A 22 43 41 52 44 5F 52 45 4C 45 41 53 45 22 2C 22 50 61 72 61 6D 73 22 3A 7B 22 41 73 73 61 73 73 69 6E 22 3A 22 32 30 31 37 30 33 32 34 22 2C 22 48 65 61 6C 22 3A 22 32 30 31 37 30 35 30 31 22 7D 7D 00 04 01 13 03 00 09 06 00 00 00 04 13 03 00 0B 06 01 00 00 00 80 94 23 80 94 23 93 E9 8C 90 0B 00 00 7F 01 13 07 01 02 00 7F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B8 E3 D1 01 84 F7 D2 01 B9 C6 95 90 0B B8 E3 D1 01 84 F7 D2 01 B9 C6 95 90 0B 00 00 00 7F 0F 00 00 00 00 00 00 00 02 01 36 00 DD CE A7 DB 0E 05 05 B0 B9 B6 01 B0 B9 B6 01 BF 97 94 90 0B 03 01 82 01 02 00 00 00 00 00 00 1A 01 00 01 82 01 02 00 00 00 00 00 00 1C 00 01 01 82 01 02 00 00 00 00 00 00 1A 07 02 00 00 00 7F 00 00 7F 00 00 7F 00 00 00 00 00 00 00 00 00 00 00 09 00 00 00 00 F8 01 01 00 00 01 00 00 00 02 00 00 01 00 00 00 0E 00 00 01 00 00 00 82 01 00 00 01 00 00 00 81 01 00 00 01 00 00 00 04 00 00 01 00 00 00 01 AE EA E5 18 01 AE EA E5 18 00 00 00 00 01 B0 F3 B6 90 0B 00 00 01 01 8A E6 BF 33 00 00 00 00 00 00 00"
                        .Replace(" ", ""));

                return _Packet.ToArray();
            }
        }

        internal byte[] Profile
        {
            get
            {
                int TimeStamp = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                List<byte> _Packet = new List<byte>();

                _Packet.AddVInt(this.UserHighId);

                _Packet.AddVInt(this.UserLowId);

                _Packet.AddVInt(this.UserHighId);

                _Packet.AddVInt(this.UserLowId);

                _Packet.AddVInt(this.UserHighId); 

                _Packet.AddVInt(this.UserLowId);

                _Packet.AddString(this.Username); // Name

                _Packet.AddVInt(this.Changes); // Changes

                _Packet.AddVInt(21); // Arena

                _Packet.AddVInt(Trophies); // Trophies

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Legend Trophies

                _Packet.AddVInt(0); // Current Trophies => Season Higheset

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Leaderboard NR => Best Season

                _Packet.AddVInt(0); // Trophies => Best Season

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(30); // Unknown

                _Packet.AddVInt(0); // Leaderboard NR => Previous Season

                _Packet.AddVInt(0); // Trophies => Previous Season

                _Packet.AddVInt(0); // Highest Trophies

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Unknown

                _Packet.AddHexa(
                    "07 08 05 01 9B 02 05 05 9B 02 05 1D 88 88 D5 44 05 0D 05 05 0E 06 05 03 02 05 10 01 05 02 06 00 06 3C 07 08 3C 08 08 3C 09 08 3C 04 01 3C 05 01 3C 06 01 00 03 05 0B 1E 05 08 08 05 1B 01 08 1A 00 0A 1A 01 0C 1A 0D 0A 1A 03 01 1C 01 01 1C 00 01 1A 0E 04 1A 10 03 00"
                        .Replace(" ", ""));

                _Packet.AddVInt(999999999); // Gems

                _Packet.AddVInt(999999999); // Gems

                _Packet.AddVInt(Experience); // Experience

                _Packet.AddVInt(Level); // Level

                _Packet.Add(0);

                if (this.ClanId == 0)
                {
                    _Packet.Add(9);

                    _Packet.AddVInt(0);
                    _Packet.AddVInt(1);
                    _Packet.AddString("Kill Yourself");
                    _Packet.AddVInt(100);
                    _Packet.AddVInt(2);
                }
                else
                {
                    _Packet.Add(0);
                }

                _Packet.AddVInt(0); // Games played

                _Packet.AddVInt(0); // Matches Played => Tournament

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Wins

                _Packet.AddVInt(0); // Loses

                _Packet.AddVInt(0); // Streak

                _Packet.AddVInt(Tutorial); // Tutorial

                _Packet.AddVInt(0); // Tournament

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Unknown

                return _Packet.ToArray();
            }
        }
    }
}
