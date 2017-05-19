using System;
using System.Windows;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Components;
using BL.Servers.CoC.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CoC.Logic.Structure
{
    internal class ConstructionItem : GameObject
    {
        public ConstructionItem(Data data, Level level) : base(data, level)
        {
            this.Level = level;
            this.IsBoosted = false;
            this.IsConstructing = false;
            this.UpgradeLevel = -1;
        }

        internal Level Level;
        internal DateTime BoostEndTime;
        internal Timer Timer;

        internal bool Locked;
        internal bool IsBoosted;
        internal int  UpgradeLevel;
        internal bool IsConstructing;
        public void BoostBuilding()
        {
            IsBoosted = true;
            this.BoostEndTime = Level.Avatar.LastTick.AddMinutes(GetBoostDuration());
        }
        
        internal int GetUpgradeLevel() => this.UpgradeLevel;

        public bool IsMaxUpgradeLevel() => UpgradeLevel >= GetConstructionItemData().GetUpgradeLevelCount() - 1;

        public bool IsUpgrading() => this.IsConstructing && UpgradeLevel >= 0;

        public Construction_Item GetConstructionItemData() => (Construction_Item)GetData();

        public Resource_Storage_Component GetResourceStorageComponent(bool enabled = false)
        {
            Component comp = GetComponent(6, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Resource_Storage_Component)comp;
            }
            return null;
        }
        public Hero_Base_Component GetHeroBaseComponent(bool enabled = false)
        {
            Component comp = GetComponent(10, enabled);
            if (comp != null && comp.Type != -1)
            {
                return (Hero_Base_Component)comp;
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
                this.Level.WorkerManager.DeallocateWorker(this);
                if (UpgradeLevel == -1)
                {
                    this.Level.GameObjectManager.RemoveGameObject(this);
                }
            }
        }
        internal int GetBoostDuration()
        {
            /*if (GetResourceProductionComponent() != null)
            {
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_PRODUCTION_BOOST_MINS")).NumberValue;
            }
           if (GetUnitProductionComponent() != null)
            {
                if (GetUnitProductionComponent().IsSpellForge())
                {
                    return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("SPELL_FACTORY_BOOST_MINS")).NumberValue;
                }
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("BARRACKS_BOOST_MINS")).NumberValue;
            }*/
            if (GetHeroBaseComponent() != null)
            {
                return ((Globals)CSV.Tables.Get(Gamefile.Globals).GetData("HERO_REST_BOOST_MINS")).NumberValue;
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

        internal void FinishConstruction()
        {
            this.IsConstructing = false;
            this.Level.WorkerManager.DeallocateWorker(this);
            SetUpgradeLevel(GetUpgradeLevel() + 1);
         //   if (GetResourceProductionComponent() != null)
            {
           //     GetResourceProductionComponent().Reset();
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

        public int GetRemainingConstructionTime() => this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick);

        public int GetRequiredTownHallLevelForUpgrade()
        {
            int upgradeLevel = Math.Min(UpgradeLevel + 1, GetConstructionItemData().GetUpgradeLevelCount() - 1);
            return GetConstructionItemData().GetRequiredTownHallLevel(upgradeLevel);
        }

        public new void Load(JObject jsonObject)
        {
            UpgradeLevel = jsonObject["lvl"].ToObject<int>();
            this.Level.WorkerManager.DeallocateWorker(this);
            var constTimeToken = jsonObject["const_t"];
            if (constTimeToken != null)
            {
                this.Timer = new Timer();
                this.IsConstructing = true;
                var remainingConstructionTime = constTimeToken.ToObject<int>();
                this.Timer.StartTimer(remainingConstructionTime, this.Level.Avatar.LastTick);
                this.Level.WorkerManager.AllocateWorker(this);
            }
            Locked = false;
            var lockedToken = jsonObject["locked"];
            if (lockedToken != null)
            {
                Locked = lockedToken.ToObject<bool>();
            }
            SetUpgradeLevel(UpgradeLevel);
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            jsonObject.Add("lvl", UpgradeLevel);
            if (IsConstructing)
                jsonObject.Add("const_t", this.Timer.GetRemainingSeconds(this.Level.Avatar.LastTick));
            if (Locked)
                jsonObject.Add("locked", true);
            base.Save(jsonObject);
            return jsonObject;
        }

        public void SpeedUpConstruction()
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

        public void StartConstructing(Vector vector)
        {
            X = (int)vector.X;
            Y = (int)vector.Y;

            int constructionTime = GetConstructionItemData().GetConstructionTime(UpgradeLevel + 1);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                this.Timer = new Timer();
                this.Timer.StartTimer(constructionTime, this.Level.Avatar.LastTick);
                this.Level.WorkerManager.AllocateWorker(this);
                this.IsConstructing = true;
            }
        }
        public void StartUpgrading()
        {
            int constructionTime = GetConstructionItemData().GetConstructionTime(UpgradeLevel + 1);
            if (constructionTime < 1)
            {
                FinishConstruction();
            }
            else
            {
                this.IsConstructing = true;
                this.Timer = new Timer();
                this.Timer.StartTimer(constructionTime, this.Level.Avatar.LastTick);
                this.Level.WorkerManager.AllocateWorker(this);
            }
        }

        public void SetUpgradeLevel(int level)
        {
            UpgradeLevel = level;
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
                    FinishConstruction();
                }
            }
        }
        internal void Unlock()
        {
            Locked = false;
        }

    }
}
