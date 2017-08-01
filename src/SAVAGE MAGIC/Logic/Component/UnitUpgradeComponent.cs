using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic
{
    internal class UnitUpgradeComponent : Component
    {
        public UnitUpgradeComponent(GameObject go) : base(go)
        {
            m_vTimer = null;
            m_vCurrentlyUpgradedUnit = null;
        }

        public override int Type => 9;

        CombatItemData m_vCurrentlyUpgradedUnit;
        Timer m_vTimer;

        public bool CanStartUpgrading(CombatItemData cid)
        {
            var result = false;
            if (m_vCurrentlyUpgradedUnit == null)
            {
                var b = (Building) Parent;
                var ca = Parent.Level.GetHomeOwnerAvatar();
                var cm = Parent.Level.GetComponentManager();
                int maxProductionBuildingLevel;
                if (cid.GetCombatItemType() == 1)
                    maxProductionBuildingLevel = cm.GetMaxSpellForgeLevel();
                else
                    maxProductionBuildingLevel = cm.GetMaxBarrackLevel();
                if (ca.GetUnitUpgradeLevel(cid) < cid.GetUpgradeLevelCount() - 1)
                {
                    if (maxProductionBuildingLevel >= cid.GetRequiredProductionHouseLevel() - 1)
                    {
                        result = b.GetUpgradeLevel() >=
                                 cid.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(cid) + 1) - 1;
                    }
                }
            }
            return result;
        }

        public void FinishUpgrading()
        {
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var ca = Parent.Level.GetHomeOwnerAvatar();
                var level = ca.GetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit);
                ca.SetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit, level + 1);
            }
            m_vTimer = null;
            m_vCurrentlyUpgradedUnit = null;
        }

        public CombatItemData GetCurrentlyUpgradedUnit() => m_vCurrentlyUpgradedUnit;

        public int GetRemainingSeconds()
        {
            var result = 0;
            if (m_vTimer != null)
            {
                result = m_vTimer.GetRemainingSeconds(Parent.Level.Time);
            }
            return result;
        }

        public int GetTotalSeconds()
        {
            var result = 0;
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var ca = Parent.Level.GetHomeOwnerAvatar();
                var level = ca.GetUnitUpgradeLevel(m_vCurrentlyUpgradedUnit);
                result = m_vCurrentlyUpgradedUnit.GetUpgradeTime(level);
            }
            return result;
        }

        public override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject) jsonObject["unit_upg"];
            if (unitUpgradeObject != null)
            {
                m_vTimer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                m_vTimer.StartTimer(remainingTime, Parent.Level.Time);

                var id = unitUpgradeObject["id"].ToObject<int>();
                m_vCurrentlyUpgradedUnit = (CombatItemData)CsvManager.DataTables.GetDataById(id);
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var unitUpgradeObject = new JObject();

                unitUpgradeObject.Add("unit_type", m_vCurrentlyUpgradedUnit.GetCombatItemType());
                unitUpgradeObject.Add("t", m_vTimer.GetRemainingSeconds(Parent.Level.Time));
                unitUpgradeObject.Add("id", m_vCurrentlyUpgradedUnit.GetGlobalID());
                jsonObject.Add("unit_upg", unitUpgradeObject);
            }
            return jsonObject;
        }

        public void SpeedUp()
        {
            if (m_vCurrentlyUpgradedUnit != null)
            {
                var remainingSeconds = 0;
                if (m_vTimer != null)
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
        }

        public void StartUpgrading(CombatItemData cid)
        {
            if (CanStartUpgrading(cid))
            {
                m_vCurrentlyUpgradedUnit = cid;
                m_vTimer = new Timer();
                m_vTimer.StartTimer(GetTotalSeconds(), Parent.Level.Time);
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
