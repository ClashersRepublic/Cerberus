using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic.Structure;
using Newtonsoft.Json.Linq;
using Resource = BL.Servers.CoC.Logic.Enums.Resource;

namespace BL.Servers.CoC.Logic.Components
{
    internal class Unit_Upgrade_Component : Component
    {
        internal Combat_Item Unit;
        internal Timer Timer;

        public Unit_Upgrade_Component(GameObject go) : base(go)
        {
            this.Timer = null;
            this.Unit = null;
        }

        internal override int Type => 9;
        internal Combat_Item GetUnit => this.Unit;

        internal bool CanStartUpgrading(Combat_Item data)
        {
            var result = false;
            if (this.Unit == null)
            {
                if (GetParent.ClassId == 0)
                {
                    var b = (Building) GetParent;
                    var ca = GetParent.Level.Avatar;
                    var cm = GetParent.Level.GetComponentManager;
                    var maxProductionBuildingLevel = data.GetCombatItemType() == 1
                        ? cm.GetMaxSpellForgeLevel()
                        : cm.GetMaxBarrackLevel();
                    if (ca.GetUnitUpgradeLevel(data) < data.GetUpgradeLevelCount() - 1)
                    {
                        if (maxProductionBuildingLevel >= data.GetRequiredProductionHouseLevel() - 1)
                        {
                            result = b.GetUpgradeLevel() >=
                                     data.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(data) + 1) - 1;
                        }
                    }
                }
                else if (GetParent.ClassId == 7)
                {
                    var b = (Builder_Building)GetParent;
                    var ca = GetParent.Level.Avatar;
                    var cm = GetParent.Level.GetComponentManager;
                    var maxProductionBuildingLevel = data.GetCombatItemType() == 1
                        ? cm.GetMaxSpellForgeLevel()
                        : cm.GetMaxBarrackLevel();
                    if (ca.GetUnitUpgradeLevel(data) < data.GetUpgradeLevelCount() - 1)
                    {
                        if (maxProductionBuildingLevel >= data.GetRequiredProductionHouseLevel() - 1)
                        {
                            result = b.GetUpgradeLevel() >=
                                     data.GetRequiredLaboratoryLevel(ca.GetUnitUpgradeLevel(data) + 1) - 1;
                        }
                    }
                }
            }
            return result;
        }

        internal void FinishUpgrading()
        {
            if (this.Unit != null)
            {
                var ca = GetParent.Level.Avatar;
                var level = ca.GetUnitUpgradeLevel(this.Unit);
                ca.SetUnitUpgradeLevel(this.Unit, level + 1);
            }
            this.Timer = null;
            this.Unit = null;
        }

        internal int GetRemainingSeconds()
        {
            var result = 0;
            if (this.Timer != null)
            {
                result = this.Timer.GetRemainingSeconds(GetParent.Level.Avatar.LastTick);
            }
            return result;
        }

        internal int GetTotalSeconds()
        {
            var result = 0;
            if (this.Unit != null)
            {
                var ca = GetParent.Level.Avatar;
                var level = ca.GetUnitUpgradeLevel(this.Unit);
                result = this.Unit.GetUpgradeTime(level);
            }
            return result;
        }

        internal override void Load(JObject jsonObject)
        {
            var unitUpgradeObject = (JObject)jsonObject["unit_upg"];
            if (unitUpgradeObject != null)
            {
                this.Timer = new Timer();
                var remainingTime = unitUpgradeObject["t"].ToObject<int>();
                var remainingEndTime = unitUpgradeObject["t_end"].ToObject<int>();

                var startTime = (int)TimeUtils.ToUnixTimestamp(GetParent.Level.Avatar.LastTick);
                var duration = remainingEndTime - startTime;

                if (duration < 0)
                    duration = 0;

                this.Timer.StartTimer(GetParent.Level.Avatar.LastTick, duration);

                var id = unitUpgradeObject["id"].ToObject<int>();
                this.Unit = (Combat_Item)CSV.Tables.GetWithGlobalID(id);
            }
        }

        internal override JObject Save(JObject jsonObject)
        {
            if (this.Unit != null)
            {
                var unitUpgradeObject = new JObject
                {
                    {"unit_type", this.Unit.GetCombatItemType()},
                    {"t", this.Timer.GetRemainingSeconds(GetParent.Level.Avatar.LastTick)},
                    {"t_end", this.Timer.EndTime},
                    {"id", this.Unit.GetGlobalID()}
                };

                jsonObject.Add("unit_upg", unitUpgradeObject);
            }
            return jsonObject;
        }
        public void SpeedUp()
        {
            if (this.Unit != null)
            {
                var remainingSeconds = 0;
                if (this.Timer != null)
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
        }

        internal void StartUpgrading(Combat_Item cid)
        {
            if (CanStartUpgrading(cid))
            {
                this.Unit = cid;
                this.Timer = new Timer();
                this.Timer.StartTimer(GetParent.Level.Avatar.LastTick, GetTotalSeconds());
            }
        }

        internal override void Tick()
        {
            if (Timer?.GetRemainingSeconds(GetParent.Level.Avatar.LastTick) <= 0)
            {
                FinishUpgrading();
            }
        }
    }
}
