using System;
using System.Windows;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Components;
using CRepublic.Magic.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Structure
{
    internal class ConstructionItem : GameObject
    {
        public ConstructionItem(Data data, Level level) : base(data, level)
        {
            this.IsBoosted = false;
            this.IsBoostPause = false;
            this.IsConstructing = false;
            this.IsGearing = false;
            this.UpgradeLevel = -1;
        }
        
        internal Timer BoostTimer;
        internal Timer Timer;

        internal bool Locked;
        internal bool IsBoosted;
        internal bool IsBoostPause;
        internal int  UpgradeLevel;
        internal bool IsConstructing;
        internal bool IsGearing;
        internal bool Builder_Village;
        public void BoostBuilding()
        {
            this.IsBoosted = true;
            this.IsBoostPause = false;
            this.BoostTimer = new Timer();
            this.BoostTimer.StartTimer(this.Level.Avatar.LastTick, GetBoostDuration());
        }

        internal DateTime GetBoostEndTime => this.BoostTimer.GetEndTime;

        internal int GetUpgradeLevel() => this.UpgradeLevel;

        internal bool IsMaxUpgradeLevel() => UpgradeLevel >= GetConstructionItemData().GetUpgradeLevelCount() - 1;

        internal bool IsUpgrading() => this.IsConstructing && UpgradeLevel >= 0;

        internal Construction_Item GetConstructionItemData() => (Construction_Item)GetData();

        internal Resource_Production_Component GetResourceProductionComponent(bool enabled = false)
        {
            var comp = GetComponent(5, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Resource_Production_Component)comp;
            }
            return null;
        }

        internal Resource_Storage_Component GetResourceStorageComponent(bool enabled = false)
        {
            Component comp = GetComponent(6, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Resource_Storage_Component)comp;
            }
            return null;
        }


        internal Hero_Base_Component GetHeroBaseComponent(bool enabled = false)
        {
            Component comp = GetComponent(10, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Hero_Base_Component)comp;
            }
            return null;
        }

        internal Unit_Upgrade_Component GetUnitUpgradeComponent(bool enabled = false)
        {
            var comp = GetComponent(9, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Unit_Upgrade_Component)comp;
            }
            return null;
        }

        internal Unit_Production_Component GetUnitProductionComponent(bool enabled = false)
        {
            var comp = GetComponent(3, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Unit_Production_Component)comp;
            }
            return null;
        }

        internal Unit_Storage_V2_Componenent GetUnitStorageV2Component(bool enabled = false)
        {
            var comp = GetComponent(11, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Unit_Storage_V2_Componenent)comp;
            }
            return null;
        }

        internal void CancelConstruction()
        {
            if (this.IsConstructing)
            {
                bool wasUpgrading = IsUpgrading();
                this.IsConstructing = false;
                if (wasUpgrading)
                {
                    SetUpgradeLevel(UpgradeLevel);
                }
                Construction_Item bd = GetConstructionItemData();
                Files.CSV_Logic.Resource rd = bd.GetBuildResource(UpgradeLevel + 1);
                int cost = bd.GetBuildCost(UpgradeLevel + 1);
                int multiplier = (CSV.Tables.Get(Gamefile.Globals).GetData("BUILD_CANCEL_MULTIPLIER") as Globals).NumberValue;
                int resourceCount = (int)((cost * multiplier * (long)1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                this.Level.Avatar.Resources.ResourceChangeHelper(rd.GetGlobalID(), resourceCount);

                if (Builder_Village)
                    this.Level.BuilderVillageWorkerManager.DeallocateWorker(this);
                else
                    this.Level.VillageWorkerManager.DeallocateWorker(this);

                if (UpgradeLevel == -1)
                {
                    this.Level.GameObjectManager.RemoveGameObject(this);
                }
            }
        }

        internal int GetBoostDuration()
        {
            if (GetResourceProductionComponent() != null)
            {
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_PRODUCTION_BOOST_MINS")).NumberValue;
            }
           if (GetUnitProductionComponent() != null)
            {
                if (GetUnitProductionComponent().IsSpellForge)
                {
                    return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("SPELL_FACTORY_BOOST_MINS")).NumberValue;
                }
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("BARRACKS_BOOST_MINS")).NumberValue;
            }
            if (GetHeroBaseComponent() != null)
            {
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("HERO_REST_BOOST_MINS")).NumberValue;
            }

            return 0;
        }

        internal float GetBoostMultipier()
        {
            if (GetResourceProductionComponent() != null)
            {
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_PRODUCTION_BOOST_MULTIPLIER")).NumberValue;
            }
            if (GetUnitProductionComponent() != null)
            {
                if (GetUnitProductionComponent().IsSpellForge)
                {
                    return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("SPELL_FACTORY_BOOST_MULTIPLIER")).NumberValue;
                }
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("BARRACKS_BOOST_MULTIPLIER")).NumberValue;
            }
            if (GetHeroBaseComponent() != null)
            {
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("HERO_REST_BOOST_MULTIPLIER")).NumberValue;
            }

            return 0;
        }

        internal bool CanUpgrade()
        {
            bool result = false;
            if (!IsConstructing)
            {
                if (UpgradeLevel < GetConstructionItemData().GetUpgradeLevelCount() - 1)
                {
                    result = true;
                    if (ClassId == 0 || ClassId == 4)
                    {
                         int currentTownHallLevel = this.Level.Avatar.TownHall_Level;
                        int requiredTownHallLevel = GetRequiredTownHallLevelForUpgrade();
                        if (currentTownHallLevel < requiredTownHallLevel)
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        internal void FinishUnlocking()
        {
            this.IsConstructing = false;

            if (Builder_Village)
                this.Level.BuilderVillageWorkerManager.DeallocateWorker(this);
            else
                this.Level.VillageWorkerManager.DeallocateWorker(this);

            this.Unlock();
            if (GetResourceProductionComponent() != null)
            {
                GetResourceProductionComponent().Reset();
            }

            int constructionTime = GetConstructionItemData().GetConstructionTime(GetUpgradeLevel());
            this.Level.Avatar.AddExperience((int)Math.Pow(constructionTime, 0.5f));
            if (GetHeroBaseComponent(true) != null)
            {
                Buildings data = (Buildings)GetData();
                Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(data.HeroType) as Heroes;
                Level.Avatar.SetUnitUpgradeLevel(hd, 0);
                Level.Avatar.SetHeroHealth(hd, 0);
                Level.Avatar.SetHeroState(hd, 3);
            }
        }

        internal void FinishConstruction()
        {
            this.IsConstructing = false;

            if (Builder_Village)
                this.Level.BuilderVillageWorkerManager.DeallocateWorker(this);
            else
                this.Level.VillageWorkerManager.DeallocateWorker(this);

            SetUpgradeLevel(GetUpgradeLevel() + 1);
             if (GetResourceProductionComponent() != null)
            {
                 GetResourceProductionComponent().Reset();
            }

            int constructionTime = GetConstructionItemData().GetConstructionTime(GetUpgradeLevel());
            this.Level.Avatar.AddExperience((int)Math.Pow(constructionTime, 0.5f));
            if (GetHeroBaseComponent(true) != null)
            {
                Buildings data = (Buildings)GetData();
                Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(data.HeroType) as Heroes;
                Level.Avatar.SetUnitUpgradeLevel(hd, 0);
                Level.Avatar.SetHeroHealth(hd, 0);
                Level.Avatar.SetHeroState(hd, 3);
            }
        }

        internal int GetRemainingConstructionTime() => this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick);

        internal int GetRequiredTownHallLevelForUpgrade()
        {
            var upgradeLevel = Math.Min(UpgradeLevel + 1, GetConstructionItemData().GetUpgradeLevelCount() - 1);
            return GetConstructionItemData().GetRequiredTownHallLevel(upgradeLevel);
        }

        public new void Load(JObject jsonObject)
        {
            var builderVillageToken = jsonObject["bv"];
            if (builderVillageToken != null)
            {
                this.Builder_Village = builderVillageToken.ToObject<bool>();

            }
            this.UpgradeLevel = jsonObject["lvl"].ToObject<int>();

            if (Builder_Village)
                this.Level.BuilderVillageWorkerManager.DeallocateWorker(this);
            else
                this.Level.VillageWorkerManager.DeallocateWorker(this);

            var gearingToken = jsonObject["gearing"];
            var gearingTimeToken = jsonObject["const_t"];
            var gearingTimeEndToken = jsonObject["const_t_end"];

            if (gearingToken != null && gearingTimeToken != null && gearingTimeEndToken != null)
            {
                this.Timer = new Timer();
                this.IsGearing = true;

                var remainingGearingTime = gearingTimeEndToken.ToObject<int>();
                var startTime = (int)TimeUtils.ToUnixTimestamp(this.Level.Avatar.LastTick);
                var duration = remainingGearingTime - startTime;

                if (duration < 0)
                    duration = 0;

                this.Timer.StartTimer(this.Level.Avatar.LastTick, duration);


                if (Builder_Village)
                    this.Level.BuilderVillageWorkerManager.AllocateWorker(this);
                else
                    this.Level.VillageWorkerManager.AllocateWorker(this);
            }

            var constTimeToken = jsonObject["const_t"];
            var constTimeEndToken = jsonObject["const_t_end"];
            if (constTimeToken != null && constTimeEndToken != null)
            {
                this.Timer = new Timer();
                this.IsConstructing = true;

                var remainingConstructionEndTime = constTimeEndToken.ToObject<int>();
                var startTime = (int)TimeUtils.ToUnixTimestamp(this.Level.Avatar.LastTick);
                var duration = remainingConstructionEndTime - startTime;

                if (duration < 0)
                    duration = 0;

                this.Timer.StartTimer(this.Level.Avatar.LastTick, duration);

                if (Builder_Village)
                    this.Level.BuilderVillageWorkerManager.AllocateWorker(this);
                else
                    this.Level.VillageWorkerManager.AllocateWorker(this);
            }
            var boostToken = jsonObject["boost_t"];
            var boostEndToken = jsonObject["boost_t_end"];
            if (boostToken != null && boostEndToken != null)
            {
                this.BoostTimer = new Timer();
                this.IsBoosted = true;
                //this.IsBoostPause = boostPauseToken.ToObject<bool>();

                var remainingBoostEndTime = boostEndToken.ToObject<int>();
                var startTime = (int)TimeUtils.ToUnixTimestamp(this.Level.Avatar.LastTick);
                var duration = remainingBoostEndTime - startTime;

                this.BoostTimer.StartTimer(this.Level.Avatar.LastTick, duration);
            }
            Locked = false;
            var lockedToken = jsonObject["locked"];
            if (lockedToken != null)
            {
                Locked = lockedToken.ToObject<bool>();
            }
            this.SetUpgradeLevel(this.UpgradeLevel);
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            jsonObject.Add("bv", this.Builder_Village);
            jsonObject.Add("lvl", UpgradeLevel);
            if (this.IsGearing)
            {
                jsonObject.Add("gearing", this.IsGearing);
                jsonObject.Add("const_t", this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick));
                jsonObject.Add("const_t_end", this.Timer.EndTime);
            }

            if (this.IsConstructing)
            {
                jsonObject.Add("const_t", this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick));
                jsonObject.Add("const_t_end", this.Timer.EndTime);
            }
            if (this.IsBoosted)
            {
                jsonObject.Add("boost_t", this.BoostTimer.GetRemainingSeconds(this.Level.Avatar.LastTick));
                jsonObject.Add("boost_t_end", this.BoostTimer.EndTime);
            }
            if (this.Locked)
                jsonObject.Add("locked", true);

            base.Save(jsonObject);
            return jsonObject;
        }

        internal void SpeedUpConstruction()
        {
            if (this.IsConstructing)
            {
                Player ca = this.Level.Avatar;
                int remainingSeconds = this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick);
                int cost = GameUtils.GetSpeedUpCost(remainingSeconds);
                if (ca.Resources.Gems >= cost)
                {
                    ca.Resources.Minus(Enums.Resource.Diamonds, cost);
                    FinishConstruction();
                }
            }
        }

        internal void StartConstructing(Vector vector, bool builder_village, bool instant = false)
        {
            this.X = (int) vector.X;
            this.Y = (int) vector.Y;
            this.Builder_Village = builder_village;
            int constructionTime = instant ? 0 : GetConstructionItemData().GetConstructionTime(0);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                this.Timer = new Timer();
                this.Timer.StartTimer(this.Level.Avatar.LastTick, constructionTime);
                if (builder_village)
                    this.Level.BuilderVillageWorkerManager.AllocateWorker(this);
                else
                    this.Level.VillageWorkerManager.AllocateWorker(this);
                this.IsConstructing = true;
            }

        }

        internal void StartUpgrading(bool builder_village)
        {
            this.Builder_Village = builder_village;
            int constructionTime = GetConstructionItemData().GetConstructionTime(UpgradeLevel + 1);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                this.IsConstructing = true;
                this.Timer = new Timer();
                this.Timer.StartTimer(this.Level.Avatar.LastTick, constructionTime);
                if (Builder_Village)
                    this.Level.BuilderVillageWorkerManager.AllocateWorker(this);
                else
                    this.Level.VillageWorkerManager.AllocateWorker(this);
            }
        }

        [Obsolete] //Not Working ¯\_(ツ)_/¯
        internal void StartGearUp(bool builder_village)
        {
            this.Builder_Village = builder_village;
            int gearUpTime = GetConstructionItemData().GetGearUpTime(UpgradeLevel + 1);
            if (gearUpTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                this.IsGearing = true;
                this.Timer = new Timer();
                this.Timer.StartTimer(this.Level.Avatar.LastTick, gearUpTime);
                if (Builder_Village)
                    this.Level.BuilderVillageWorkerManager.AllocateWorker(this);
                else
                    this.Level.VillageWorkerManager.AllocateWorker(this);
            }
        }

        [Obsolete] //Not Working ¯\_(ツ)_/¯
        internal void StartUnlocking(bool builder_village)
        {
            this.Builder_Village = builder_village;
            int constructionTime = GetConstructionItemData().GetConstructionTime(0);
            Console.WriteLine(constructionTime);
            if (constructionTime < 1)
            {
                FinishUnlocking();
            }
            else
            {
                this.IsConstructing = true;
                this.Timer = new Timer();
                this.Timer.StartTimer(this.Level.Avatar.LastTick, constructionTime);
                if (Builder_Village)
                    this.Level.BuilderVillageWorkerManager.AllocateWorker(this);
                else
                    this.Level.VillageWorkerManager.AllocateWorker(this);
            }
        }

        internal void SetUpgradeLevel(int level)
        {
            this.UpgradeLevel = level;
            if (UpgradeLevel > -1 || IsUpgrading() || !IsConstructing)
            {
                /*if (GetUnitStorageComponent(true) != null)
                {
                    var data = (Buildings)GetData();
                    if (data.GetUnitStorageCapacity(level) > 0)
                    {
                        if (!data.Bunker)
                        {
                            GetUnitStorageComponent().SetMaxCapacity(data.GetUnitStorageCapacity(level));
                            GetUnitStorageComponent().SetEnabled(!IsConstructing());
                        }
                    }
                }*/

                var resourceStorageComponent = GetResourceStorageComponent(true);
                if (resourceStorageComponent != null)
                {
                   var maxStoredResourcesList = ((Buildings)GetData()).GetMaxStoredResourceCounts(UpgradeLevel);
                    resourceStorageComponent.SetMaxArray(maxStoredResourcesList);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (this.IsConstructing)
            {
                if (this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick) <= 0)
                {
                    if (this.Locked)
                        this.FinishUnlocking();
                    else
                        this.FinishConstruction();
                }
            }
            if (this.IsBoosted)
            {
                if (this.BoostTimer.GetRemainingSeconds(this.Level.Avatar.LastTick) <= 0)
                {
                    this.IsBoosted = false;
                }
            }

            if (this.IsGearing)
            {
                if (this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick) <= 0)
                {
                    this.FinishConstruction();
                }
            }
        }

        internal void Unlock()
        {
            Locked = false;
        }

    }
}
