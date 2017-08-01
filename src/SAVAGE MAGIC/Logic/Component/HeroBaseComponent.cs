using System;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic
{
    internal class HeroBaseComponent : Component
    {
        public HeroBaseComponent(GameObject go, HeroData hd) : base(go)
        {
            m_vHeroData = hd;
        }

        public override int Type => 10;

        readonly HeroData m_vHeroData;
        Timer m_vTimer;
        int m_vUpgradeLevelInProgress;

        public void CancelUpgrade()
        {
            if (m_vTimer != null)
            {
                var ca = Parent.Level.Avatar;
                var currentLevel = ca.GetUnitUpgradeLevel(m_vHeroData);
                var rd = m_vHeroData.GetUpgradeResource(currentLevel);
                var cost = m_vHeroData.GetUpgradeCost(currentLevel);
                var multiplier =
                   CsvManager.DataTables.GetGlobals().GetGlobalData("HERO_UPGRADE_CANCEL_MULTIPLIER").NumberValue;
                var resourceCount = (int) ((cost * multiplier * (long) 1374389535) >> 32);
                resourceCount = Math.Max((resourceCount >> 5) + (resourceCount >> 31), 0);
                ca.CommodityCountChangeHelper(0, rd, resourceCount);
                Parent.Level.WorkerManager.DeallocateWorker(Parent);
                m_vTimer = null;
            }
        }

        public bool CanStartUpgrading()
        {
            var result = false;
            if (m_vTimer == null)
            {
                var currentLevel = Parent.Level.Avatar.GetUnitUpgradeLevel(m_vHeroData);
                if (!IsMaxLevel())
                {
                    var requiredThLevel = m_vHeroData.GetRequiredTownHallLevel(currentLevel + 1);
                    result = Parent.Level.Avatar.GetTownHallLevel() >= requiredThLevel;
                }
            }
            return result;
        }

        public void FinishUpgrading()
        {
            var ca = Parent.Level.Avatar;
            var currentLevel = ca.GetUnitUpgradeLevel(m_vHeroData);
            ca.SetUnitUpgradeLevel(m_vHeroData, currentLevel + 1);
            Parent.Level.WorkerManager.DeallocateWorker(Parent);
            m_vTimer = null;
        }

        public int GetRemainingUpgradeSeconds() => m_vTimer.GetRemainingSeconds(Parent.Level.Time);

        public int GetTotalSeconds() => m_vHeroData.GetUpgradeTime(Parent.Level.Avatar.GetUnitUpgradeLevel(m_vHeroData));

        public bool IsMaxLevel()
        {
            var ca = Parent.Level.Avatar;
            var currentLevel = ca.GetUnitUpgradeLevel(m_vHeroData);
            var maxLevel = m_vHeroData.GetUpgradeLevelCount() - 1;
            return currentLevel >= maxLevel;
        }

        public bool IsUpgrading() => m_vTimer != null;

        public override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject) jsonObject["hero_upg"];
            if (unitUpgradeObject != null)
            {
                m_vTimer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                m_vTimer.StartTimer(remainingTime, Parent.Level.Time);
                m_vUpgradeLevelInProgress = unitUpgradeObject["level"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            if (m_vTimer != null)
            {
                var unitUpgradeObject = new JObject();
                unitUpgradeObject.Add("level", m_vUpgradeLevelInProgress);
                unitUpgradeObject.Add("t", m_vTimer.GetRemainingSeconds(Parent.Level.Time));
                jsonObject.Add("hero_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public void SpeedUpUpgrade()
        {
            var remainingSeconds = 0;
            if (IsUpgrading())
            {
                remainingSeconds = m_vTimer.GetRemainingSeconds(Parent.Level.Time);
            }
            var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
            var ca = Parent.Level.Avatar;
            if (ca.HasEnoughDiamonds(cost))
            {
                ca.UseDiamonds(cost);
                FinishUpgrading();
            }
        }

        public void StartUpgrading()
        {
            if (CanStartUpgrading())
            {
                Parent.Level.WorkerManager.AllocateWorker(Parent);
                m_vTimer = new Timer();
                m_vTimer.StartTimer(GetTotalSeconds(), Parent.Level.Time);
                m_vUpgradeLevelInProgress = Parent.Level.Avatar.GetUnitUpgradeLevel(m_vHeroData) + 1;
            }
        }

        public override void Tick()
        {
            if (m_vTimer != null)
            {
                if (m_vTimer.GetRemainingSeconds(Parent.Level.Time) <= 0)
                {
                    FinishUpgrading();
                }
            }
        }
    }
}
