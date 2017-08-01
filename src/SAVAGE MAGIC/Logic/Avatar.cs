using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic.DataSlots;
using static System.Configuration.ConfigurationManager;
using static System.Convert;

namespace Magic.ClashOfClans.Logic
{
    internal class Avatar : BaseAvatar
    {
        // Long
        private long _allianceId;
        private long _homeId;
        private long _id;

        // Int
        public int ReportedTimes;

        private int _highId;
        private int _lowId;
        private int _expLvl;
        private int _gems;
        private int _expPoint;
        private int _league;
        private int _shieldTime;
        private int _trophy;
        private int _troopsDonated;
        private int _troopsReceived;
        private int _activeLayout;
        // Byte
        private byte _nameChangeLeft;
        private byte _nameChoosenByUser;

        // String
        private string _name;
        private string _token;
        private string _region;

        // Boolean
        private bool _isAndroid;

        //DateTime
        private DateTime _accCreateDate;

        public enum UserState : int
        {
            Home = 0,
            Searching = 1,
            PVP = 2,
            PVE = 3,
            CHA = 4,
            Editmode = 5,
        }

        public int GetShieldTime => _shieldTime;

        public struct AttackInfo
        {
            public Level Defender;
            public Level Attacker;

            public int Lost;
            public int Reward;

            public List<DataSlot> UsedTroop;
        }

        public Avatar()
        {
            Achievements = new List<DataSlot>();
            AchievementsUnlocked = new List<DataSlot>();
            AllianceUnits = new List<TroopDataSlot>();
            NpcStars = new List<DataSlot>();
            NpcLootedGold = new List<DataSlot>();
            NpcLootedElixir = new List<DataSlot>();
            BookmarkedClan = new List<BookmarkSlot>();
            DonationSlot = new List<DonationSlot>();
            QuickTrain1 = new List<DataSlot>();
            QuickTrain2 = new List<DataSlot>();
            QuickTrain3 = new List<DataSlot>();

            AttackingInfo = new Dictionary<long, AttackInfo>(); //Should use AttackID,Since it not yet implement.I use userID
        }

        public Avatar(long id, string token) : this()
        {
            LastUpdate = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            EndShieldTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            _id = id;
            _homeId = id;
            _token = token;

            _highId = (int)(id >> 32);
            _lowId = (int)(id & 0xFFFFFFFFL);

            _nameChoosenByUser = 0;
            _nameChangeLeft = 2;

            _expLvl = ToInt32(AppSettings["startingLevel"]);
            _allianceId = 0;
            _expPoint = 0;
            _gems = ToInt32(AppSettings["startingGems"]);

            _trophy = AppSettings["startingTrophies"] == "random" ? s_rnd.Next(1500, 4800) : Convert.ToInt32(AppSettings["startingTrophies"]);

            TutorialStepsCount = 0x0A;
            _name = "NoNameYet";

            SetResourceCount(CsvManager.DataTables.GetResourceByName("Gold"), ToInt32(AppSettings["startingGold"]));
            SetResourceCount(CsvManager.DataTables.GetResourceByName("Elixir"), ToInt32(AppSettings["startingElixir"]));
            SetResourceCount(CsvManager.DataTables.GetResourceByName("DarkElixir"), ToInt32(AppSettings["startingDarkElixir"]));
            SetResourceCount(CsvManager.DataTables.GetResourceByName("Diamonds"), ToInt32(AppSettings["startingGems"]));
        }

        public List<DataSlot> Achievements { get; set; }
        public List<DataSlot> AchievementsUnlocked { get; set; }
        public List<TroopDataSlot> AllianceUnits { get; set; }
        public int EndShieldTime { get; set; }
        public int LastUpdate { get; set; }
        public UserState State { get; set; }

        public List<DataSlot> NpcLootedElixir { get; set; }
        public List<DataSlot> NpcLootedGold { get; set; }
        public List<DataSlot> NpcStars { get; set; }
        public List<BookmarkSlot> BookmarkedClan { get; set; }
        public List<DonationSlot> DonationSlot { get; set; }
        public Dictionary<long, AttackInfo> AttackingInfo { get; set; }
        public List<DataSlot> QuickTrain1 { get; set; }
        public List<DataSlot> QuickTrain2 { get; set; }
        public List<DataSlot> QuickTrain3 { get; set; }

        public uint TutorialStepsCount { get; set; }
        public uint Region { get; set; }

        public int RemainingShieldTime
        {
            get
            {
                var rest = EndShieldTime - (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                return rest > 0 ? rest : 0;
            }
        }

        private void UpdateLeague()
        {
            var table = CsvManager.DataTables.GetTable(12);
            var i = 0;
            bool found = false;
            while (!found)
            {
                var league = (LeagueData)table.GetItemAt(i);
                if (_trophy <= league.BucketPlacementRangeHigh[league.BucketPlacementRangeHigh.Count - 1] &&
                    _trophy >= league.BucketPlacementRangeLow[0])
                {
                    found = true;
                    SetLeagueId(i);
                }
                i++;
            }
        }

        public void AddDiamonds(int diamondCount)
        {
            _gems += diamondCount;
        }

        public void AddExperience(int exp)
        {
            _expPoint += exp;
            var experienceCap =
                ((ExperienceLevelData)CsvManager.DataTables.GetTable(10).GetDataByName(_expLvl.ToString()))
                    .ExpPoints;
            if (_expPoint >= experienceCap)
            {
                if (CsvManager.DataTables.GetTable(10).GetItemCount() > _expLvl + 1)
                {
                    _expLvl += 1;
                    _expPoint = _expPoint - experienceCap;
                }
                else
                    _expPoint = 0;
            }
        }

        private static readonly Random s_rnd = new Random();

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt64(_id);
            data.AddInt64(_homeId);

            if (_allianceId != 0)
            {
                var alliance = ObjectManager.GetAlliance(_allianceId);
                var inClan = alliance != null && alliance.GetAllianceMember(_id) != null;
                if (inClan)
                {
                    data.Add(1);
                    data.AddInt64(_allianceId);

                    data.AddString(alliance.AllianceName);
                    data.AddInt32(alliance.AllianceBadgeData);
                    data.AddInt32(alliance.GetAllianceMember(_id).GetRole());
                    data.AddInt32(alliance.AllianceLevel);
                }
                else
                {
                    _allianceId = 0;
                }
            }
            data.Add(0);

            if (_league == 22)
            {
                data.AddInt32(_trophy / 12);
                data.AddInt32(1);
                var month = DateTime.Now.Month;
                data.AddInt32(month);
                data.AddInt32(DateTime.Now.Year);
                data.AddInt32(s_rnd.Next(1, 10));
                data.AddInt32(_trophy);
                data.AddInt32(1);
                if (month == 1)
                {
                    data.AddInt32(12);
                    data.AddInt32(DateTime.Now.Year - 1);
                }
                else
                {
                    int pmonth = month - 1;
                    data.AddInt32(pmonth);
                    data.AddInt32(DateTime.Now.Year);
                }
                data.AddInt32(s_rnd.Next(1, 10));
                data.AddInt32(_trophy / 2);
            }
            else
            {
                data.AddInt32(0); //1
                data.AddInt32(0); //2
                data.AddInt32(0); //3
                data.AddInt32(0); //4
                data.AddInt32(0); //5
                data.AddInt32(0); //6
                data.AddInt32(0); //7
                data.AddInt32(0); //8
                data.AddInt32(0); //9
                data.AddInt32(0); //10
                data.AddInt32(0); //11
            }

            data.AddInt32(_league);
            data.AddInt32(GetAllianceCastleLevel());
            data.AddInt32(GetAllianceCastleTotalCapacity());
            data.AddInt32(GetAllianceCastleUsedCapacity());
            data.AddInt32(0);
            data.AddInt32(-1);
            data.AddInt32(GetTownHallLevel());
            data.AddString(_name);
            data.AddInt32(-1);
            data.AddInt32(_expLvl);
            data.AddInt32(_expPoint);
            data.AddInt32(_gems);
            data.AddInt32(_gems);
            data.AddInt32(1200);
            data.AddInt32(60);
            data.AddInt32(_trophy);
            data.AddInt32(100); // Attack Wins
            data.AddInt32(1);
            data.AddInt32(100); // Attack Loses
            data.AddInt32(0);

            data.AddInt32(2800000); // Castle Gold
            data.AddInt32(2800000); // Castle Elexir
            data.AddInt32(14400); // Castle Dark Elexir
            data.AddInt32(0);
            data.Add(1);
            data.AddInt64(946720861000);

            data.Add(_nameChoosenByUser);

            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(1);

            data.AddInt32(0);
            data.AddInt32(0);
            data.Add(0);
            data.AddDataSlots(GetResourceCaps());
            data.AddDataSlots(GetResources());
            data.AddDataSlots(GetUnits());
            data.AddDataSlots(GetSpells());
            data.AddDataSlots(m_vUnitUpgradeLevel);
            data.AddDataSlots(m_vSpellUpgradeLevel);
            data.AddDataSlots(m_vHeroUpgradeLevel);
            data.AddDataSlots(m_vHeroHealth);
            data.AddDataSlots(m_vHeroState);

            data.AddRange(BitConverter.GetBytes(AllianceUnits.Count).Reverse());
            foreach (var u in AllianceUnits)
            {
                data.AddRange(BitConverter.GetBytes(u.Data.GetGlobalID()).Reverse());
                data.AddRange(BitConverter.GetBytes(u.Value).Reverse());
                data.AddRange(BitConverter.GetBytes(0).Reverse());
            }

            data.AddRange(BitConverter.GetBytes(TutorialStepsCount).Reverse());
            for (uint i = 0; i < TutorialStepsCount; i++)
                data.AddRange(BitConverter.GetBytes(0x01406F40 + i).Reverse());

            data.AddRange(BitConverter.GetBytes(Achievements.Count).Reverse());
            foreach (var a in Achievements)
                data.AddRange(BitConverter.GetBytes(a.Data.GetGlobalID()).Reverse());

            data.AddRange(BitConverter.GetBytes(Achievements.Count).Reverse());
            foreach (var a in Achievements)
            {
                data.AddRange(BitConverter.GetBytes(a.Data.GetGlobalID()).Reverse());
                data.AddRange(BitConverter.GetBytes(0).Reverse());
            }

            data.AddRange(BitConverter.GetBytes(ObjectManager.NpcLevels.Count).Reverse());
            for (var i = 17000000; i < 17000050; i++)
            {
                data.AddRange(BitConverter.GetBytes(i).Reverse());
                data.AddRange(BitConverter.GetBytes(s_rnd.Next(3, 3)).Reverse());
            }

            data.AddDataSlots(NpcLootedGold);
            data.AddDataSlots(NpcLootedElixir);
            data.AddDataSlots(new List<DataSlot>());
            data.AddDataSlots(new List<DataSlot>());
            data.AddDataSlots(new List<DataSlot>());
            data.AddDataSlots(new List<DataSlot>());

            data.AddInt32(QuickTrain1.Count);  //Quick Train 1
            foreach (var i in QuickTrain1)
            {
                data.AddInt32(i.Data.GetGlobalID());
                data.AddInt32(i.Value);
            }

            data.AddInt32(QuickTrain2.Count);  //Quick Train 2
            foreach (var i in QuickTrain2)
            {
                data.AddInt32(i.Data.GetGlobalID());
                data.AddInt32(i.Value);
            }
            data.AddInt32(QuickTrain3.Count); //Quick Train 3
            foreach (var i in QuickTrain3)
            {
                data.AddInt32(i.Data.GetGlobalID());
                data.AddInt32(i.Value);
            }

            data.AddInt32(QuickTrain1.Count); //Previous trained troop
            foreach (var i in QuickTrain1)
            {
                data.AddInt32(i.Data.GetGlobalID());
                data.AddInt32(i.Value);
            }
            data.AddDataSlots(new List<DataSlot>());
            return data.ToArray();
        }

        public long GetAllianceId() => _allianceId;

        public AllianceMemberEntry GetAllianceMemberEntry()
        {
            var alliance = ObjectManager.GetAlliance(_allianceId);
            if (alliance != null)
                return alliance.GetAllianceMember(_id);
            return null;
        }

        public DateTime GetAccountCreationDate()
        {
            return _accCreateDate;
        }

        public int GetAllianceRole()
        {
            var ame = GetAllianceMemberEntry();
            if (ame != null)
                return ame.GetRole();
            return -1;
        }

        public int GetAvatarHighIdInt() => _highId;
        public int GetAvataLowIdInt() => _lowId;
        public int GetAvatarLevel() => _expLvl;
        public int GetActiveLayout() => _activeLayout;
        public string GetAvatarName() => _name;
        public long GetCurrentHomeId() => _homeId;
        public int GetDiamonds() => _gems;

        public bool Android
        {
            get
            {
                return _isAndroid;
            }

            set
            {
                _isAndroid = value;
            }
        }

        public int GetLeagueId() => _league;
        public int GetSecondsFromLastUpdate() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - LastUpdate;
        public string GetUserRegion() => _region;

        public string Token => _token;
        public long Id => _id;

        public int GetScore()
        {
            UpdateLeague();
            return _trophy;
        }

        public bool HasEnoughDiamonds(int diamondCount) => _gems >= diamondCount;

        public bool HasEnoughResources(ResourceData rd, int buildCost) => GetResourceCount(rd) >= buildCost;

        public void LoadFromJson(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            _id = jsonObject["avatar_id"].ToObject<long>();
            _highId = jsonObject["id_high_int"].ToObject<int>();
            _lowId = jsonObject["id_low_int"].ToObject<int>();
            _token = jsonObject["token"].ToObject<string>();
            _region = jsonObject["region"].ToObject<string>();
            _accCreateDate = jsonObject["avatar_creation_date"].ToObject<DateTime>();
            _activeLayout = jsonObject["active_layout"].ToObject<int>();
            _isAndroid = jsonObject["android"].ToObject<bool>();
            _homeId = jsonObject["current_home_id"].ToObject<long>();
            _allianceId = jsonObject["alliance_id"].ToObject<long>();
            SetAllianceCastleLevel(jsonObject["alliance_castle_level"].ToObject<int>());
            SetAllianceCastleTotalCapacity(jsonObject["alliance_castle_total_capacity"].ToObject<int>());
            SetAllianceCastleUsedCapacity(jsonObject["alliance_castle_used_capacity"].ToObject<int>());
            SetTownHallLevel(jsonObject["townhall_level"].ToObject<int>());
            _name = jsonObject["avatar_name"].ToObject<string>();
            _expLvl = jsonObject["avatar_level"].ToObject<int>();
            _expPoint = jsonObject["experience"].ToObject<int>();
            _gems = jsonObject["current_gems"].ToObject<int>();
            SetScore(jsonObject["score"].ToObject<int>());
            _nameChangeLeft = jsonObject["nameChangesLeft"].ToObject<byte>();
            _nameChoosenByUser = jsonObject["nameChosenByUser"].ToObject<byte>();

            var jsonBookmarkedClan = (JArray)jsonObject["bookmark"];
            foreach (JObject jobject in jsonBookmarkedClan)
            {
                var data = (JObject)jobject;
                var ds = new BookmarkSlot(0);
                ds.Load(data);
                BookmarkedClan.Add(ds);
            }

            var jsonResources = (JArray)jsonObject["resources"];
            foreach (JObject resource in jsonResources)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(resource);
                GetResources().Add(ds);
            }

            var jsonUnits = (JArray)jsonObject["units"];
            foreach (JObject unit in jsonUnits)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(unit);
                m_vUnitCount.Add(ds);
            }

            var jsonSpells = (JArray)jsonObject["spells"];
            foreach (JObject spell in jsonSpells)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(spell);
                m_vSpellCount.Add(ds);
            }

            var jsonUnitLevels = (JArray)jsonObject["unit_upgrade_levels"];
            foreach (JObject unitLevel in jsonUnitLevels)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(unitLevel);
                m_vUnitUpgradeLevel.Add(ds);
            }

            var jsonSpellLevels = (JArray)jsonObject["spell_upgrade_levels"];
            foreach (JObject data in jsonSpellLevels)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vSpellUpgradeLevel.Add(ds);
            }

            var jsonHeroLevels = (JArray)jsonObject["hero_upgrade_levels"];
            foreach (JObject data in jsonHeroLevels)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroUpgradeLevel.Add(ds);
            }

            var jsonHeroHealth = (JArray)jsonObject["hero_health"];
            foreach (JObject data in jsonHeroHealth)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroHealth.Add(ds);
            }

            var jsonHeroState = (JArray)jsonObject["hero_state"];
            foreach (JObject data in jsonHeroState)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                m_vHeroState.Add(ds);
            }

            var jsonAllianceUnits = (JArray)jsonObject["alliance_units"];
            foreach (JObject data in jsonAllianceUnits)
            {
                var ds = new TroopDataSlot(null, 0, 0);
                ds.Load(data);
                AllianceUnits.Add(ds);
            }
            TutorialStepsCount = jsonObject["tutorial_step"].ToObject<uint>();

            var jsonAchievementsProgress = (JArray)jsonObject["achievements_progress"];
            foreach (JObject data in jsonAchievementsProgress)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                Achievements.Add(ds);
            }

            var jsonNpcStars = (JArray)jsonObject["npc_stars"];
            foreach (JObject data in jsonNpcStars)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcStars.Add(ds);
            }

            var jsonNpcLootedGold = (JArray)jsonObject["npc_looted_gold"];
            foreach (JObject data in jsonNpcLootedGold)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcLootedGold.Add(ds);
            }

            var jsonNpcLootedElixir = (JArray)jsonObject["npc_looted_elixir"];
            foreach (JObject data in jsonNpcLootedElixir)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                NpcLootedElixir.Add(ds);
            }
            var jsonQuickTrain1 = (JArray)jsonObject["quick_train_1"];
            foreach (JObject data in jsonQuickTrain1)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                QuickTrain1.Add(ds);
            }
            var jsonQuickTrain2 = (JArray)jsonObject["quick_train_2"];
            foreach (JObject data in jsonQuickTrain2)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                QuickTrain2.Add(ds);
            }
            var jsonQuickTrain3 = (JArray)jsonObject["quick_train_3"];
            foreach (JObject data in jsonQuickTrain3)
            {
                var ds = new DataSlot(null, 0);
                ds.Load(data);
                QuickTrain3.Add(ds);
            }
        }

        public string SaveToJson()
        {
            var jsonData = new JObject();
            jsonData.Add("avatar_id", _id);
            jsonData.Add("id_high_int", _highId);
            jsonData.Add("id_low_int", _lowId);
            jsonData.Add("token", _token);
            jsonData.Add("region", _region);
            jsonData.Add("avatar_creation_date", _accCreateDate);
            jsonData.Add("active_layout", _activeLayout);
            jsonData.Add("android", _isAndroid);
            jsonData.Add("current_home_id", _homeId);
            jsonData.Add("alliance_id", _allianceId);
            jsonData.Add("alliance_castle_level", GetAllianceCastleLevel());
            jsonData.Add("alliance_castle_total_capacity", GetAllianceCastleTotalCapacity());
            jsonData.Add("alliance_castle_used_capacity", GetAllianceCastleUsedCapacity());
            jsonData.Add("townhall_level", GetTownHallLevel());
            jsonData.Add("avatar_name", _name);
            jsonData.Add("avatar_level", _expLvl);
            jsonData.Add("experience", _expPoint);
            jsonData.Add("current_gems", _gems);
            jsonData.Add("score", GetScore());
            jsonData.Add("nameChangesLeft", _nameChangeLeft);
            jsonData.Add("nameChosenByUser", (ushort)_nameChoosenByUser);

            var jsonBookmarkClan = new JArray();
            foreach (var clan in BookmarkedClan)
                jsonBookmarkClan.Add(clan.Save(new JObject()));
            jsonData.Add("bookmark", jsonBookmarkClan);

            var jsonResourcesArray = new JArray();
            foreach (var resource in GetResources())
                jsonResourcesArray.Add(resource.Save(new JObject()));
            jsonData.Add("resources", jsonResourcesArray);

            var jsonUnitsArray = new JArray();
            foreach (var unit in GetUnits())
                jsonUnitsArray.Add(unit.Save(new JObject()));
            jsonData.Add("units", jsonUnitsArray);


            var jsonSpellsArray = new JArray();
            foreach (var spell in GetSpells())
                jsonSpellsArray.Add(spell.Save(new JObject()));
            jsonData.Add("spells", jsonSpellsArray);

            var jsonUnitUpgradeLevelsArray = new JArray();
            foreach (var unitUpgradeLevel in m_vUnitUpgradeLevel)
                jsonUnitUpgradeLevelsArray.Add(unitUpgradeLevel.Save(new JObject()));
            jsonData.Add("unit_upgrade_levels", jsonUnitUpgradeLevelsArray);

            var jsonSpellUpgradeLevelsArray = new JArray();
            foreach (var spellUpgradeLevel in m_vSpellUpgradeLevel)
                jsonSpellUpgradeLevelsArray.Add(spellUpgradeLevel.Save(new JObject()));
            jsonData.Add("spell_upgrade_levels", jsonSpellUpgradeLevelsArray);

            var jsonHeroUpgradeLevelsArray = new JArray();
            foreach (var heroUpgradeLevel in m_vHeroUpgradeLevel)
                jsonHeroUpgradeLevelsArray.Add(heroUpgradeLevel.Save(new JObject()));
            jsonData.Add("hero_upgrade_levels", jsonHeroUpgradeLevelsArray);

            var jsonHeroHealthArray = new JArray();
            foreach (var heroHealth in m_vHeroHealth)
                jsonHeroHealthArray.Add(heroHealth.Save(new JObject()));
            jsonData.Add("hero_health", jsonHeroHealthArray);

            var jsonHeroStateArray = new JArray();
            foreach (var heroState in m_vHeroState)
                jsonHeroStateArray.Add(heroState.Save(new JObject()));
            jsonData.Add("hero_state", jsonHeroStateArray);

            var jsonAllianceUnitsArray = new JArray();
            foreach (var allianceUnit in AllianceUnits)
                jsonAllianceUnitsArray.Add(allianceUnit.Save(new JObject()));
            jsonData.Add("alliance_units", jsonAllianceUnitsArray);

            jsonData.Add("tutorial_step", TutorialStepsCount);

            var jsonAchievementsProgressArray = new JArray();
            foreach (var achievement in Achievements)
                jsonAchievementsProgressArray.Add(achievement.Save(new JObject()));
            jsonData.Add("achievements_progress", jsonAchievementsProgressArray);

            var jsonNpcStarsArray = new JArray();
            foreach (var npcLevel in NpcStars)
                jsonNpcStarsArray.Add(npcLevel.Save(new JObject()));
            jsonData.Add("npc_stars", jsonNpcStarsArray);

            var jsonNpcLootedGoldArray = new JArray();
            foreach (var npcLevel in NpcLootedGold)
                jsonNpcLootedGoldArray.Add(npcLevel.Save(new JObject()));
            jsonData.Add("npc_looted_gold", jsonNpcLootedGoldArray);

            var jsonNpcLootedElixirArray = new JArray();
            foreach (var npcLevel in NpcLootedElixir)
                jsonNpcLootedElixirArray.Add(npcLevel.Save(new JObject()));
            jsonData.Add("npc_looted_elixir", jsonNpcLootedElixirArray);

            var jsonQuickTrain1Array = new JArray();
            foreach (var quicktrain1 in QuickTrain1)
                jsonQuickTrain1Array.Add(quicktrain1.Save(new JObject()));
            jsonData.Add("quick_train_1", jsonQuickTrain1Array);

            var jsonQuickTrain2Array = new JArray();
            foreach (var quicktrain2 in QuickTrain2)
                jsonQuickTrain1Array.Add(quicktrain2.Save(new JObject()));
            jsonData.Add("quick_train_2", jsonQuickTrain2Array);

            var jsonQuickTrain3Array = new JArray();
            foreach (var quicktrain3 in QuickTrain3)
                jsonQuickTrain3Array.Add(quicktrain3.Save(new JObject()));
            jsonData.Add("quick_train_3", jsonQuickTrain3Array);

            return JsonConvert.SerializeObject(jsonData);
        }

        public void InitializeAccountCreationDate()
        {
            _accCreateDate = DateTime.Now;
        }

        public void AddUsedTroop(CombatItemData cid, int value)
        {
            if (State == UserState.PVP)
            {
                var info = default(AttackInfo);
                if (!AttackingInfo.TryGetValue(Id, out info))
                {
                    Logger.Error("Unable to obtain attack info.");
                }
                else
                {
                    var dataSlot = info.UsedTroop.Find(t => t.Data.GetGlobalID() == cid.GetGlobalID());

                    if (dataSlot != null)
                    {
                        // Troops already exist.
                        int i = info.UsedTroop.IndexOf(dataSlot);
                        dataSlot.Value = dataSlot.Value + value;
                        info.UsedTroop[i] = dataSlot;
                    }
                    else
                    {
                        DataSlot ds = new DataSlot(cid, value);
                        info.UsedTroop.Add(ds);
                    }
                }
            }
            //else
            //  Logger.Write("Unsupported state! AddUsedTroop only for PVP for now.PVE Comming Soon");
        }

        public void SetAchievment(AchievementData ad, bool finished)
        {
            var index = GetDataIndex(Achievements, ad);
            var value = finished ? 1 : 0;
            if (index != -1)
                Achievements[index].Value = value;
            else
            {
                var ds = new DataSlot(ad, value);
                Achievements.Add(ds);
            }
        }

        public void SetAllianceId(long id)
        {
            _allianceId = id;
        }

        public void SetShieldTime(int time)
        {
            _shieldTime = time;
        }

        public void SetActiveLayout(int layout)
        {
            _activeLayout = layout;
        }

        public void SetAllianceRole(int a)
        {
            var ame = GetAllianceMemberEntry();
            if (ame != null)
                ame.SetRole(a);
        }

        public void SetDiamonds(int count)
        {
            _gems = count;
        }

        public void SetLeagueId(int id)
        {
            _league = id;
        }

        public void SetName(string name)
        {
            _name = name;
            if (_nameChoosenByUser == 0x01)
            {
                _nameChangeLeft = 0x01;
            }
            else
            {
                _nameChangeLeft = 0x02;
            }
            TutorialStepsCount = 0x0D;
        }

        public void SetScore(int newScore)
        {
            _trophy = newScore;
            UpdateLeague();
        }

        public void SetToken(string token)
        {
            _token = token;
        }

        public void SetRegion(string region)
        {
            _region = region;
        }
        public void SetAvatarLevel(int newlv)
        {
            _expLvl = newlv;
        }

        public void UseDiamonds(int diamondCount)
        {
            _gems -= diamondCount;
        }
    }
}
