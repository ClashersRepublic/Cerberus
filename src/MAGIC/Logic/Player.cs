using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure.Slots;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using CRepublic.Magic.Packets.Messages.Server.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using Npcs = CRepublic.Magic.Logic.Structure.Slots.Npcs;
using Resource = CRepublic.Magic.Logic.Enums.Resource;

namespace CRepublic.Magic.Logic
{
    internal class Player : ICloneable
    {
        [JsonIgnore] internal long Battle_ID_V2;
        [JsonIgnore] internal long Battle_ID;
        [JsonIgnore] internal int Amical_ID;
        [JsonIgnore] internal int ObstacleClearCount;


        [JsonIgnore]
        internal DateTime LoginTime = DateTime.UtcNow;

        [JsonIgnore]
        internal long UserId
        {
            get { return (((long)this.UserHighId << 32) | (this.UserLowId & 0xFFFFFFFFL)); }
            set
            {
                this.UserHighId = Convert.ToInt32(value >> 32);
                this.UserLowId = (int) value;
            }
        }

        [JsonIgnore]
        internal long ClanId
        {
            get { return (((long)this.ClanHighID << 32) | (this.ClanLowID & 0xFFFFFFFFL)); }
            set
            {
                this.ClanHighID = Convert.ToInt32(value >> 32);
                this.ClanLowID = (int) value;
            }
        }

        internal void Refresh()
        {
            DataTable table = CSV.Tables.Get(Gamefile.Leagues);
            int i = 0;
            bool found = false;
            while (!found && i < table.Datas.Count)
            {
                var league = (Leagues) table.Datas[i];
                if (this.Trophies <= league.BucketPlacementRangeHigh[league.BucketPlacementRangeHigh.Length - 1] && this.Trophies >= league.BucketPlacementRangeLow[0])
                {
                    found = true;
                    this.League = i;
                }
                i++;
            }
        }

        [JsonProperty("avatar_id_high")] internal int UserHighId;
        [JsonProperty("avatar_id_low")] internal int UserLowId;

        [JsonProperty("alliance_id_high")] internal int ClanHighID;
        [JsonProperty("alliance_id_low")] internal int ClanLowID;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;

        [JsonProperty("name")] internal string Name = "NoNameYet";
        [JsonProperty("IpAddress")] internal string IpAddress;
        [JsonProperty("region")] internal string Region;
        [JsonProperty("alliance_name")] internal string Alliance_Name;

        [JsonProperty("xp_level")] internal int Level = 1;
        [JsonProperty("xp")] internal int Experience;

        [JsonProperty("wins")] internal int Wons;
        [JsonProperty("loses")] internal int Loses;
        [JsonProperty("games")] internal int Games;
        [JsonProperty("win_streak")] internal int Streak;
        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("received")] internal int Received;

        [JsonProperty("shield")] internal int Shield;
        [JsonProperty("guard")] internal int Guard;
        [JsonProperty("score")] internal int Trophies = Core.Resources.Random.Next(4000, 4050);
        [JsonProperty("duel_score")] internal int Builder_Trophies = Core.Resources.Random.Next(0, 200);
        [JsonProperty("legend_troph")] internal int Legendary_Trophies;
        [JsonProperty("league_type")] internal int League;


        [JsonProperty("war_state")] internal bool WarState = true;
        [JsonProperty("name_state")] internal byte NameState;

        [JsonProperty("rank")] internal Rank Rank = Rank.PLAYER;

        [JsonProperty("town_hall_level")] internal int TownHall_Level;
        [JsonProperty("th_v2_lvl")] internal int Builder_TownHall_Level;
        [JsonProperty("castle_lvl")] internal int Castle_Level = -1;
        [JsonProperty("castle_total")] internal int Castle_Total;
        [JsonProperty("castle_used")] internal int Castle_Used;
        [JsonProperty("castle_total_sp")] internal int Castle_Total_SP;
        [JsonProperty("castle_used_sp")] internal int Castle_Used_SP;
        [JsonProperty("castle_resource")] internal Resources Castle_Resources;

        [JsonProperty("bookmarks")] internal List<long> Bookmarks = new List<long>();
        [JsonProperty("tutorials")] internal List<int> Tutorials = new List<int>();
        [JsonProperty("last_search_enemy_id")] internal List<long> Last_Attack_Enemy_ID = new List<long>();

        [JsonProperty("account_locked")] internal bool Locked = false;

        [JsonProperty("badge_id")] internal int Badge_ID = -1;
        [JsonProperty("wall_group_id")] internal int Wall_Group_ID = -1;
        [JsonProperty("alliance_role")] internal int Alliance_Role = -1;
        [JsonProperty("alliance_level")] internal int Alliance_Level = -1;

        [JsonProperty("units")] internal Units Units;
        [JsonProperty("units2")] internal Units Units2;
        [JsonProperty("spells")] internal Units Spells;
        [JsonProperty("alliance_units")] internal Castle_Units Castle_Units;
        [JsonProperty("alliance_spells")] internal Castle_Units Castle_Spells;


        [JsonProperty("unit_upgrades")] internal Upgrades Unit_Upgrades;
        [JsonProperty("spell_upgrades")] internal Upgrades Spell_Upgrades;
        [JsonProperty("hero_upgrade")] internal Upgrades Heroes_Upgrades;

        [JsonProperty("hero_states")] internal Slots Heroes_States;
        [JsonProperty("hero_health")] internal Slots Heroes_Health;
        [JsonProperty("hero_modes")] internal Slots Heroes_Modes;

        [JsonProperty("resources")] internal Resources Resources;
        [JsonProperty("resources_cap")] internal Resources Resources_Cap;
        [JsonProperty("npcs")] internal Npcs Npcs;
        [JsonProperty("variable")] internal Structure.Slots.Variables Variables;
        [JsonProperty("debug_mode")] internal Modes Modes;

        [JsonProperty("login_count")] internal int Login_Count;

        [JsonProperty("play_time")]
        internal TimeSpan PlayTime => (DateTime.UtcNow - this.Created);

        [JsonProperty("last_tick")] internal DateTime LastTick = DateTime.UtcNow;
        [JsonProperty("last_save")] internal DateTime LastSave = DateTime.UtcNow;
        [JsonProperty("creation_date")] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty("ban_date")] internal DateTime BanTime = DateTime.UtcNow;

        [JsonProperty("facebook")] internal Structure.API.Facebook Facebook;
        [JsonProperty("google")] internal Structure.API.Google Google;
        [JsonProperty("gamecenter")] internal Structure.API.Gamecenter Gamecenter;
        [JsonProperty("inbox")] internal Inbox Inbox;

        internal bool Banned => this.BanTime > DateTime.UtcNow;

        internal Player()
        {

            this.Facebook = new Structure.API.Facebook(this);
            this.Google = new Structure.API.Google(this);
            this.Gamecenter = new Structure.API.Gamecenter(this);
            this.Inbox = new Inbox(this);

            this.Castle_Resources = new Resources(this);
            this.Resources = new Resources(this);
            this.Resources_Cap = new Resources(this);
            this.Npcs = new Npcs();
            this.Variables = new Structure.Slots.Variables(this);
            this.Modes = new Modes(this);

            this.Units = new Units(this);
            this.Units2 = new Units(this);
            this.Spells = new Units(this);
            this.Castle_Units = new Castle_Units(this);
            this.Castle_Spells = new Castle_Units(this);

            this.Unit_Upgrades = new Upgrades(this);
            this.Spell_Upgrades = new Upgrades(this);
            this.Heroes_Upgrades = new Upgrades(this);

            this.Heroes_Health = new Slots();
            this.Heroes_Modes = new Slots();
            this.Heroes_States = new Slots();
        }

        internal Player(long UserId)
        {
            this.UserId = UserId;

            this.Facebook = new Structure.API.Facebook(this);
            this.Google = new Structure.API.Google(this);
            this.Gamecenter = new Structure.API.Gamecenter(this);
            this.Inbox = new Inbox(this);

            this.Castle_Resources = new Resources(this, false);
            this.Resources = new Resources(this, true);
            this.Resources_Cap = new Resources(this, false);
            this.Npcs = new Npcs();
            this.Variables = new Structure.Slots.Variables(this, true);
            this.Modes = new Modes(this, true);

            this.Units = new Units(this);
            this.Units2 = new Units(this);
            this.Spells = new Units(this);
            this.Castle_Units = new Castle_Units(this);
            this.Castle_Spells = new Castle_Units(this);

            this.Unit_Upgrades = new Upgrades(this);
            this.Spell_Upgrades = new Upgrades(this);
            this.Heroes_Upgrades = new Upgrades(this);

            this.Heroes_Health = new Slots();
            this.Heroes_Modes = new Slots();
            this.Heroes_States = new Slots();
        }

        internal byte[] ToBytes
        {
            get
            {
                //this.Refresh();

                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(this.UserId);
                _Packet.AddLong(this.UserId);

                _Packet.AddBool(this.ClanId > 0);
                if (this.ClanId > 0)
                {
                    Clan clan = Core.Resources.Clans.Get(ClanId, Constants.Database);

                    if (clan != null)
                    {
                        if (!string.IsNullOrEmpty(clan.Name))
                        {
                            _Packet.AddLong(this.ClanId);
                            _Packet.AddString(clan.Name);
                            _Packet.AddInt(clan.Badge); // Badge
                            _Packet.AddInt((int) clan.Members[UserId].Role); // Role
                            _Packet.AddInt(clan.Level); // Level

                            _Packet.AddBool(false); // Alliance War
                            {
                                // _Packet.AddLong(1); // War ID
                            }
                        }
                        else
                        {
                            foreach (var userid in clan?.Members.Keys)
                            {
                                var player = Core.Resources.Players.Get(userid, Constants.Database, false);
                                player.Avatar.ClanId = 0;
                                player.Avatar.Alliance_Role = -1;
                                player.Avatar.Alliance_Level = -1;
                                player.Avatar.Alliance_Name = string.Empty;
                                player.Avatar.Badge_ID = -1;

                            }
                            Core.Resources.Clans.Delete(clan);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Clan is null");
                        this.ClanId = 0;
                        this.Alliance_Role = -1;
                        this.Alliance_Level = -1;
                        this.Alliance_Name = string.Empty;
                        this.Badge_ID = -1;
                        new Out_Of_Sync(Core.Resources.Players.Get(this.UserId, Constants.Database).Client).Send();
                    }
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


                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddInt(10000);
                _Packet.AddInt(9999);
                _Packet.AddInt(8888);
                _Packet.AddInt(7777);
                _Packet.AddInt(6666);
                _Packet.AddInt(5555); //Builder base Versus Battle Win
                _Packet.AddInt(4444); //5
                _Packet.AddInt(3333); //8

                _Packet.AddInt(this.League);
                _Packet.AddInt(this.Castle_Level);
                _Packet.AddInt(this.Castle_Total);
                _Packet.AddInt(this.Castle_Used);
                _Packet.AddInt(this.Castle_Total_SP);
                _Packet.AddInt(this.Castle_Used_SP);

                _Packet.AddInt(this.TownHall_Level);
                _Packet.AddInt(this.Builder_TownHall_Level);

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
                _Packet.AddInt(this.Builder_Trophies);

                _Packet.AddInt(this.Wons);
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

                _Packet.AddByte(this.NameState); //Name changed count

                _Packet.AddInt(this.NameState > 1 ? 1 : 0); //Name Changed

                _Packet.AddInt(6900); //6900
                _Packet.AddInt(0);
                _Packet.AddInt(this.WarState ? 1 : 0);

                _Packet.AddInt(0);
                _Packet.AddInt(0); // Total Attack with shield

                _Packet.AddBool(false); //0

                _Packet.AddDataSlots(this.Resources_Cap);

                _Packet.AddRange(this.Resources.ToBytes);

                _Packet.AddDataSlots(this.Units);
                _Packet.AddDataSlots(this.Spells);
                _Packet.AddDataSlots(this.Unit_Upgrades);
                _Packet.AddDataSlots(this.Spell_Upgrades);

                _Packet.AddDataSlots(this.Heroes_Upgrades);
                _Packet.AddDataSlots(this.Heroes_Health);
                _Packet.AddDataSlots(this.Heroes_States);

                _Packet.AddInt(this.Castle_Units.Count + this.Castle_Spells.Count);

                foreach (Alliance_Unit _Unit in this.Castle_Units)
                {
                    _Packet.AddInt(_Unit.Data);
                    _Packet.AddInt(_Unit.Count);
                    _Packet.AddInt(_Unit.Level);
                }

                foreach (Alliance_Unit _Spell in this.Castle_Spells)
                {
                    _Packet.AddInt(_Spell.Data);
                    _Packet.AddInt(_Spell.Count);
                    _Packet.AddInt(_Spell.Level);
                }


                _Packet.AddInt(this.Tutorials.Count);
                foreach (var Tutorial in this.Tutorials)
                {
                    _Packet.AddInt(Tutorial);
                }

                _Packet.AddInt(0); //Achievements
                _Packet.AddInt(0); //Achievements Progress

                _Packet.AddRange(this.Npcs.ToBytes);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);

                _Packet.AddDataSlots(this.Variables);

                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                _Packet.AddDataSlots(this.Units2);
                _Packet.AddInt(0);
                _Packet.AddInt(0);
                return _Packet.ToArray();
            }
        }

        internal static int GetDataIndex(List<Slot> dsl, Data d) => dsl.FindIndex(ds => ds.Data == d.Id);

        internal static int GetDataIndex(Units dsl, Data d) => dsl.FindIndex(ds => ds.Data == d.Id);

        internal int Get_Unit_Count_V2(Combat_Item cd)
        {
            var result = 0;
            var index = GetDataIndex(this.Units2, cd);
            if (index != -1)
                result = this.Units2[index].Count;
            return result;
        }

        internal void Set_Unit_Count_V2(Combat_Item cd, int count)
        {

            var index = GetDataIndex(this.Units2, cd);
            if (index != -1)
                this.Units2[index].Count = count;
            else
            {
                var ds = new Slot(cd.GetGlobalID(), count);
                this.Units2.Add(ds);
            }
        }

        internal void Add_Unit2(int Data, int Count)
        {
            int _Index = this.Units2.FindIndex(U => U.Data == Data);

            if (_Index > -1)
            {
                this.Units2[_Index].Count += Count;
            }
            else
            {
                this.Units2.Add(new Slot(Data, Count));
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

        public void Add_Spells(int Data, int Count)
        {
            int _Index = this.Spells.FindIndex(S => S.Data == Data);

            if (_Index > -1)
            {
                this.Spells[_Index].Count += Count;
            }
            else
            {
                this.Spells.Add(new Slot(Data, Count));
            }
        }

        public void AddCastleTroop(int Data, int Count, int level)
        {
            int _Index = this.Castle_Units.FindIndex(t => t.Data == Data && t.Level == level);
            if (_Index > -1)
            {
                this.Castle_Units[_Index].Count += Count;
            }
            else
            {
                Alliance_Unit ds = new Alliance_Unit(0, Data, Count, level);
                this.Castle_Units.Add(ds);
            }
        }

        public void AddCastleSpell(int Data, int Count, int level)
        {
            int _Index = this.Castle_Spells.FindIndex(t => t.Data == Data && t.Level == level);
            if (_Index > -1)
            {
                this.Castle_Spells[_Index].Count += Count;
            }
            else
            {
                Alliance_Unit ds = new Alliance_Unit(0, Data, Count, level);
                this.Castle_Spells.Add(ds);
            }
        }

        public int GetUnitUpgradeLevel(Combat_Item cd)
        {
            int result = 0;
            switch (cd.GetCombatItemType())
            {
                case 2:
                {
                    int index = GetDataIndex(this.Heroes_Upgrades, cd);
                    if (index != -1)
                    {
                        result = this.Heroes_Upgrades[index].Count;
                    }
                    break;
                }
                case 1:
                {
                    int index = GetDataIndex(this.Spell_Upgrades, cd);
                    if (index != -1)
                    {
                        result = this.Spell_Upgrades[index].Count;
                    }
                    break;
                }

                default:
                {
                    int index = GetDataIndex(this.Unit_Upgrades, cd);
                    if (index != -1)
                    {
                        result = this.Unit_Upgrades[index].Count;
                    }
                    break;
                }
            }
            return result;
        }
        public void SetUnitUpgradeLevel(Combat_Item cd, int level)
        {
            switch (cd.GetCombatItemType())
            {
                case 2:
                {
                    int index = GetDataIndex(this.Heroes_Upgrades, cd);
                    if (index != -1)
                    {
                        this.Heroes_Upgrades[index].Count = level;
                    }
                    else
                    {
                        Slot ds = new Slot(cd.GetGlobalID(), level);
                        this.Heroes_Upgrades.Add(ds);
                    }
                    break;
                }
                case 1:
                {
                    int index = GetDataIndex(this.Spell_Upgrades, cd);
                    if (index != -1)
                    {
                        this.Spell_Upgrades[index].Count = level;
                    }
                    else
                    {
                        Slot ds = new Slot(cd.GetGlobalID(), level);
                        this.Spell_Upgrades.Add(ds);
                    }
                    break;
                }
                default:
                {
                    int index = GetDataIndex(this.Unit_Upgrades, cd);
                    if (index != -1)
                    {
                        this.Unit_Upgrades[index].Count = level;
                    }
                    else
                    {
                        Slot ds = new Slot(cd.GetGlobalID(), level);
                        this.Unit_Upgrades.Add(ds);
                    }
                    break;
                }
            }
        }

        public void SetHeroHealth(Heroes hd, int health)
        {
            int index = GetDataIndex(this.Heroes_Health, hd);
            if (index == -1)
            {
                Slot ds = new Slot(hd.GetGlobalID(), health);
                this.Heroes_Health.Add(ds);
            }
            else
            {
                this.Heroes_Health[index].Count = health;
            }
        }

        public void SetHeroState(Heroes hd, int state)
        {
            int index = GetDataIndex(this.Heroes_States, hd);
            if (index == -1)
            {
                Slot ds = new Slot(hd.GetGlobalID(), state);
                this.Heroes_States.Add(ds);
            }
            else
            {
                this.Heroes_States[index].Count = state;
            }
        }

        public bool HasEnoughResources(Resource resource, int buildCost) => this.Resources.Get(resource) >= buildCost;

        public bool HasEnoughResources(int globalId, int buildCost) => this.Resources.Get(globalId) >= buildCost;

        internal Player Clone()
        {
            return this.MemberwiseClone() as Player;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}