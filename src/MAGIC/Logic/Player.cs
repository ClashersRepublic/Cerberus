using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.API;
using BL.Servers.CoC.Logic.Structure.Slots;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic
{
    internal class Player
    {
        [JsonIgnore]
        internal long UserId
        {
            get => (((long)this.UserHighId << 32) | (this.UserLowId & 0xFFFFFFFFL));
            set
            {
                this.UserHighId = Convert.ToInt32(value >> 32);
                this.UserLowId = (int)value;
            }
        }

        [JsonIgnore]
        internal long ClanId
        {
            get => (((long)this.ClanHighID << 32) | (this.ClanLowID & 0xFFFFFFFFL));
            set
            {
                this.ClanHighID = Convert.ToInt32(value >> 32);
                this.ClanLowID = (int)value;
            }
        }

        [JsonProperty("acc_hi")] internal int UserHighId;
        [JsonProperty("acc_lo")] internal int UserLowId;

        [JsonProperty("clan_hi")] internal int ClanHighID;
        [JsonProperty("clan_lo")] internal int ClanLowID;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;

        [JsonProperty("name")] internal string Name = "NoNameYet";
        [JsonProperty("IpAddress")] internal string IpAddress;
        [JsonProperty("region")] internal string Region;

        [JsonProperty("lvl")] internal int Level = 1;
        [JsonProperty("xp")] internal int Experience;

        [JsonProperty("wins")] internal int Wins;
        [JsonProperty("loses")] internal int Loses;
        [JsonProperty("games")] internal int Games;
        [JsonProperty("win_streak")] internal int Streak;
        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;

        [JsonProperty("shield")] internal int Shield;
        [JsonProperty("guard")] internal int Guard;
        [JsonProperty("trophies")] internal int Trophies;
        [JsonProperty("legend_troph")] internal int Legendary_Trophies;
        [JsonProperty("league_type")] internal int League;


        [JsonProperty("war_state")] internal bool WarState = true;
        [JsonProperty("name_state")] internal byte NameState;

        [JsonProperty("rank")] internal Rank Rank = Rank.Player;

        [JsonProperty("town_hall_level")] internal int TownHall_Level;
        [JsonProperty("castle_lvl")] internal int Castle_Level = -1;
        [JsonProperty("castle_total")] internal int Castle_Total;
        [JsonProperty("castle_used")] internal int Castle_Used;
        [JsonProperty("castle_total_sp")] internal int Castle_Total_SP;
        [JsonProperty("castle_used_sp")] internal int Castle_Used_SP;
        [JsonProperty("castle_resource")] internal Resources Castle_Resources;

        [JsonProperty("bookmarks")] internal List<long> Bookmarks = new List<long>();
        [JsonProperty("tutorials")] internal List<int> Tutorials = new List<int>();
        [JsonProperty("account_locked")] internal bool Locked = false;


        [JsonProperty("units")] internal Units Units;
        [JsonProperty("spells")] internal Units Spells;
        [JsonProperty("alliance_units")] internal Units Castle_Units;
        [JsonProperty("alliance_spells")] internal Units Castle_Spells;

        [JsonProperty("resources")] internal Resources Resources;
        [JsonProperty("resources_cap")] internal Resources Resources_Cap;
        [JsonProperty("npcs")] internal Npcs Npcs;

        [JsonProperty("last_tick")] internal DateTime LastTick = DateTime.UtcNow;
        [JsonProperty("update_date")] internal DateTime Update = DateTime.UtcNow;
        [JsonProperty("creation_date")] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty("ban_date")] internal DateTime BanTime = DateTime.UtcNow;

        [JsonProperty("facebook")] internal Structure.API.Facebook Facebook;
        [JsonProperty("google")] internal Structure.API.Google Google;
        [JsonProperty("gamecenter")] internal Structure.API.Gamecenter Gamecenter;

        internal bool Banned => this.BanTime > DateTime.UtcNow;
        internal Player()
        {

            this.Facebook = new Structure.API.Facebook(this);
            this.Google = new Structure.API.Google(this);
            this.Gamecenter = new Structure.API.Gamecenter(this);

            this.Castle_Resources = new Resources(this);
            this.Resources = new Resources(this);
            this.Resources_Cap = new Resources(this);
            this.Npcs = new Npcs();

            this.Units = new Units(this);
            this.Spells = new Units(this);
            this.Castle_Units = new Units(this);
            this.Castle_Spells = new Units(this);
        }

        internal Player(long UserId)
        {
            this.UserId = UserId;

            this.Facebook = new Structure.API.Facebook(this);
            this.Google = new Structure.API.Google(this);
            this.Gamecenter = new Structure.API.Gamecenter(this);

            this.Castle_Resources = new Resources(this, false);
            this.Resources = new Resources(this, true);
            this.Resources_Cap = new Resources(this, false);
            this.Npcs = new Npcs();

            this.Units = new Units(this);
            this.Spells = new Units(this);
            this.Castle_Units = new Units(this);
            this.Castle_Spells = new Units(this);
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(this.UserId);
                _Packet.AddLong(this.UserId);

                _Packet.AddBool(false);
                if (this.ClanId > 0)
                {
                }

                _Packet.AddInt(this.Legendary_Trophies);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddInt(this.League);
                _Packet.AddInt(this.Castle_Level);
                _Packet.AddInt(this.Castle_Total);
                _Packet.AddInt(this.Castle_Used);
                _Packet.AddInt(this.Castle_Total_SP);
                _Packet.AddInt(this.Castle_Used_SP);

                _Packet.AddInt(this.TownHall_Level);

#if DEBUG
                _Packet.AddString($"{this.Name} #{this.UserId}:{GameUtils.GetHashtag(this.UserId)}");
#else
                _Packet.AddString(this.Name);
#endif
                _Packet.AddString(!string.IsNullOrEmpty(this.Facebook.Identifier) ? this.Facebook.Identifier : null);

                _Packet.AddInt(this.Level);
                _Packet.AddInt(this.Experience);
                _Packet.AddInt(this.Resources.Gems);
                _Packet.AddInt(this.Resources.Gems);

                _Packet.AddInt(0); // 1200
                _Packet.AddInt(0); // 60

                _Packet.AddInt(this.Trophies);

                _Packet.AddInt(this.Wins);
                _Packet.AddInt(this.Loses);
                _Packet.AddInt(0); // Def Wins
                _Packet.AddInt(0); // Def Loses
                
                _Packet.AddInt(this.Castle_Resources.Get(Resource.WarGold));
                _Packet.AddInt(this.Castle_Resources.Get(Resource.WarElixir));
                _Packet.AddInt(this.Castle_Resources.Get(Resource.WarDarkElixir));

                _Packet.AddInt(0);

                _Packet.AddBool(true);
                _Packet.AddInt(220);
                _Packet.AddInt(1828055880);

                _Packet.AddBool(this.NameState > 0);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(this.WarState ? 1 : 0);
                _Packet.AddInt(0); // Total Attack with shield
                _Packet.AddInt(0);

                _Packet.AddBool(this.NameState > 1);

                _Packet.AddInt(this.Resources_Cap.Count);
                foreach (Slot _Resource in this.Resources_Cap)
                {
                    _Packet.AddInt(_Resource.Data);
                    _Packet.AddInt(_Resource.Count);
                }

                _Packet.AddRange(this.Resources.ToBytes);

                _Packet.AddInt(this.Units.Count);
                foreach (Slot _Unit in this.Units)
                {
                    _Packet.AddInt(_Unit.Data);
                    _Packet.AddInt(_Unit.Count);
                }

                _Packet.AddInt(this.Spells.Count);
                foreach (Slot _Spell in this.Spells)
                {
                    _Packet.AddInt(_Spell.Data);
                    _Packet.AddInt(_Spell.Count);
                }

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddInt(this.Castle_Units.Count + this.Castle_Spells.Count);

                foreach (Slot _Unit in this.Castle_Units)
                {
                    _Packet.AddInt(_Unit.Data);
                    _Packet.AddInt(_Unit.Count);
                }

                foreach (Slot _Spell in this.Castle_Spells)
                {
                    _Packet.AddInt(_Spell.Data);
                    _Packet.AddInt(_Spell.Count);
                }


                _Packet.AddInt(this.Tutorials.Count);
                foreach (var Tutorial in this.Tutorials)
                {
                    _Packet.AddInt(Tutorial);
                }

                _Packet.AddInt(0);//Achievements
                _Packet.AddInt(0);//Achievements Progress

                _Packet.AddRange(this.Npcs.ToBytes);

                _Packet.AddInt(0);//Var

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                return _Packet.ToArray();
            }
            
        }
        internal void Add_Unit(int Data, int Count)
        {
            int _Index = this.Units.FindIndex(U => U.Data == Data);

            if (_Index > -1)
            {
                this.Units[_Index].Count += Count;
            }
            else
            {
                this.Units.Add(new Slot(Data, Count));
            }
        }
        public void CommodityCountChangeHelper(int commodityType, Data data, int count)
        {
            if (data.Type == 2)
            {
                if (commodityType == 0)
                {
                    int resourceCount = this.Resources.Get(data.GetGlobalID());
                    int newResourceValue = Math.Max(resourceCount + count, 0);
                    if (count >= 1)
                    {
                        int resourceCap = this.Resources_Cap.Get(data.GetGlobalID());
                        if (resourceCount < resourceCap)
                        {
                            if (newResourceValue > resourceCap)
                            {
                                newResourceValue = resourceCap;
                            }
                        }
                    }
                    this.Resources.Set(data.GetGlobalID(), newResourceValue);
                }
            }
        }

        public bool HasEnoughResources(int Globalid, int buildCost) => this.Resources.Get(Globalid) >= buildCost;

    }
}
