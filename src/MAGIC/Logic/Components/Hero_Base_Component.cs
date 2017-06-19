using System;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;
using Newtonsoft.Json.Linq;
using Resource = CRepublic.Magic.Logic.Enums.Resource;

namespace CRepublic.Magic.Logic.Components
{
    internal class Hero_Base_Component : Component
    {
        public Hero_Base_Component(GameObject go, Heroes hd) : base(go)
        {
            this.HeroData = hd;
        }

        internal override int Type => 10;

        internal Heroes HeroData;
        internal Timer Timer;
        internal bool Builder_Village;
        internal int UpgradeLevelInProgress;

        internal void CancelUpgrade()
        {
            if (this.Timer != null)
            {
                var ca = GetParent.Level.Avatar;
                var currentLevel = ca.GetUnitUpgradeLevel(this.HeroData);
                var rd = this.HeroData.GetUpgradeResource();
                var cost = this.HeroData.GetUpgradeCost(currentLevel);
                var multiplier = (CSV.Tables.Get(Gamefile.Globals).GetData("HERO_UPGRADE_CANCEL_MULTIPLIER") as Globals)
                    .NumberValue;
                var resourceCount = (int)((cost * multiplier * (long)1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                ca.Resources.ResourceChangeHelper(rd.GetGlobalID(), resourceCount);
                if (Builder_Village)
                    GetParent.Level.BuilderVillageWorkerManager.DeallocateWorker(GetParent);
                else
                    GetParent.Level.VillageWorkerManager.DeallocateWorker(GetParent);
                this.Timer = null;
            }
        }

        internal bool CanStartUpgrading()
        {
            var result = false;
            if (this.Timer == null)
            {
                var currentLevel = GetParent.Level.Avatar.GetUnitUpgradeLevel(this.HeroData);
                if (!IsMaxLevel())
                {
                    var requiredThLevel = this.HeroData.GetRequiredTownHallLevel(currentLevel + 1);
                    result = GetParent.Level.Avatar.TownHall_Level >= requiredThLevel;
                }
            }
            return result;
        }

        internal void FinishUpgrading()
        {
            var ca = GetParent.Level.Avatar;
            var currentLevel = ca.GetUnitUpgradeLevel(this.HeroData);
            ca.SetUnitUpgradeLevel(this.HeroData, currentLevel + 1);
            if (Builder_Village)
                GetParent.Level.BuilderVillageWorkerManager.DeallocateWorker(GetParent);
            else
                GetParent.Level.VillageWorkerManager.DeallocateWorker(GetParent);
            this.Timer = null;
        }

        internal int GetRemainingUpgradeSeconds() => this.Timer.GetRemainingSeconds(GetParent.Level.Avatar.LastTick);

        internal int GetTotalSeconds() => this.HeroData.GetUpgradeTime(GetParent.Level.Avatar.GetUnitUpgradeLevel(this.HeroData));

        internal bool IsMaxLevel()
        {
            var ca = GetParent.Level.Avatar;
            var currentLevel = ca.GetUnitUpgradeLevel(this.HeroData);
            var maxLevel = this.HeroData.GetUpgradeLevelCount() - 1;
            return currentLevel >= maxLevel;
        }

        internal bool IsUpgrading() => this.Timer != null;

        internal override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject)jsonObject["hero_upg"];
            if (unitUpgradeObject != null)
            {
                this.Timer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                this.Timer.StartTimer(GetParent.Level.Avatar.LastTick, remainingTime);
                this.UpgradeLevelInProgress = unitUpgradeObject["level"].ToObject<int>();
            }
        }

        internal override JObject Save(JObject jsonObject)
        {
            if (this.Timer != null)
            {
                var unitUpgradeObject = new JObject
                {
                    {"level", this.UpgradeLevelInProgress},
                    {"t", this.Timer.GetRemainingSeconds(GetParent.Level.Avatar.LastTick)}
                };
                jsonObject.Add("hero_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        internal void SpeedUpUpgrade()
        {
            var remainingSeconds = 0;
            if (IsUpgrading())
            {
                remainingSeconds = this.Timer.GetRemainingSeconds(GetParent.Level.Avatar.LastTick);
            }
            var cost = GameUtils.GetSpeedUpCost(remainingSeconds);
            var ca = GetParent.Level.Avatar;
            if (ca.Resources.Gems >= cost)
            {
                ca.Resources.Minus(Resource.Diamonds, cost);
                FinishUpgrading();
            }
        }

        internal void StartUpgrading(bool builder_village)
        {
            if (CanStartUpgrading())
            {
                this.Builder_Village = builder_village;
                if (builder_village)
                    GetParent.Level.BuilderVillageWorkerManager.DeallocateWorker(GetParent);
                else
                    GetParent.Level.VillageWorkerManager.DeallocateWorker(GetParent);
                this.Timer = new Timer();
                this.Timer.StartTimer(GetParent.Level.Avatar.LastTick, GetTotalSeconds());
                this.UpgradeLevelInProgress = GetParent.Level.Avatar.GetUnitUpgradeLevel(this.HeroData) + 1;
            }
        }

        internal override void Tick()
        {
            if (this.Timer?.GetRemainingSeconds(GetParent.Level.Avatar.LastTick) <= 0)
            {
                FinishUpgrading();
            }
        }
    }
}
