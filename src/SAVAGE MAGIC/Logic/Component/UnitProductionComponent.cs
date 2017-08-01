using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans.Logic.Manager;
using System.Collections;

namespace Magic.ClashOfClans.Logic
{
    internal class UnitProductionComponent : Component
    {
        private readonly List<DataSlot> m_vUnits;
        private bool m_vIsSpellForge;
        private bool m_vIsWaitingForSpace;
        private Timer m_vTimer;

        public override int Type
        {
            get
            {
                return 3;
            }
        }

        public UnitProductionComponent(GameObject go) : base(go)
        {
            m_vUnits = new List<DataSlot>();
            SetUnitType(go);
            m_vTimer = (Timer) null;
            m_vIsWaitingForSpace = false;
        }

        public void AddUnitToProductionQueue(CombatItemData cd)
        {
            if (!CanAddUnitToQueue(cd))
              return;
            for (int index = 0; index < GetSlotCount(); ++index)
            {
                if ((CombatItemData) m_vUnits[index].Data == cd)
                {
                    ++m_vUnits[index].Value;
                    return;
                }
            }
            m_vUnits.Add(new DataSlot((Data)cd, 1));
            if (m_vTimer != null)
              return;
            Avatar avatar = Parent.Level.GetHomeOwnerAvatar();
            m_vTimer = new Timer();
            m_vTimer.StartTimer(cd.GetTrainingTime(avatar.GetUnitUpgradeLevel(cd)), Parent.Level.Time);
        }

        public bool CanAddUnitToQueue(CombatItemData cd) => GetMaxTrainCount() >= GetTotalCount() + cd.GetHousingSpace();

        public CombatItemData GetCurrentlyTrainedUnit()
        {
          CombatItemData combatItemData = (CombatItemData)null;
          if (m_vUnits.Count >= 1)
            combatItemData = (CombatItemData) m_vUnits[0].Data;
            return combatItemData;
        }

        public int GetMaxTrainCount()
        {
            Building parent = (Building) Parent;
            return parent.BuildingData.GetUnitProduction(parent.GetUpgradeLevel());
        }

        public int GetSlotCount() => m_vUnits.Count;

        public int GetTotalCount()
        {
            var count = 0;
            if (GetSlotCount() >= 1)
            {
                for (var i = 0; i < GetSlotCount(); i++)
                {
                    var cnt = m_vUnits[i].Value;
                    var housingSpace = ((CombatItemData) m_vUnits[i].Data).GetHousingSpace();
                    count += cnt * housingSpace;
                }
            }
            if (m_vIsSpellForge)
              count += Parent.Level.GetComponentManager().GetTotalUsedHousing(true);
            return count;
        }

        public int GetTotalRemainingSeconds()
        {
            var result = 0;
            var firstUnit = true;
            if (m_vUnits.Count > 0)
            {
                foreach (DataSlot ds in m_vUnits)
                {
                    CombatItemData cd = (CombatItemData) ds.Data;
                    if (cd != null)
                    {
                        var count = ds.Value;
                        if (count >= 1)
                        {
                            if (firstUnit)
                            {
                                if (m_vTimer != null)
                                    result += m_vTimer.GetRemainingSeconds(Parent.Level.Time);
                                count--;
                                firstUnit = false;
                            }
                            var ca = Parent.Level.GetHomeOwnerAvatar();
                            result += count * cd.GetTrainingTime(ca.GetUnitUpgradeLevel(cd));
                        }
                    }
                }
            }
            return result;
        }

        public int GetTrainingCount(int index) => m_vUnits[index].Value;

        public CombatItemData GetUnit(int index) => (CombatItemData)m_vUnits[index].Data;

        public bool HasHousingSpaceForSpeedUp()
        {
            var totalRoom = 0;
            if (m_vUnits.Count >= 1)
            {
                foreach (DataSlot ds in m_vUnits)
                {
                    CombatItemData cd = (CombatItemData) ds.Data;
                    totalRoom += cd.GetHousingSpace() * ds.Value;
                }
            }
            ComponentManager cm = Parent.Level.GetComponentManager();
            int num2 = this.m_vIsSpellForge ? 1 : 0;
            int totalUsedHousing = cm.GetTotalUsedHousing(num2 != 0);
            int num3 = this.m_vIsSpellForge ? 1 : 0;
            int totalMaxHousing = cm.GetTotalMaxHousing(num3 != 0);
            return totalRoom <= totalMaxHousing - totalUsedHousing;
        }

        public bool IsSpellForge() => m_vIsSpellForge;

        public bool IsWaitingForSpace()
        {
            var result = false;
            if (m_vUnits.Count > 0)
            {
                if (m_vTimer != null)
                {
                    if (m_vTimer.GetRemainingSeconds(Parent.Level.Time) == 0)
                    {
                        result = m_vIsWaitingForSpace;
                    }
                }
            }
            return result;
        }

        public override void Load(JObject jsonObject)
        {
            var unitProdObject = (JObject) jsonObject["unit_prod"];
            m_vIsSpellForge = unitProdObject["unit_type"].ToObject<int>() == 1;
            var timeToken = unitProdObject["t"];
            if (timeToken != null)
            {
                m_vTimer = new Timer();
                var remainingTime = timeToken.ToObject<int>();
                m_vTimer.StartTimer(remainingTime, Parent.Level.Time);
            }
            var unitJsonArray = (JArray) unitProdObject["slots"];
            if (unitJsonArray == null)
              return;
            using (IEnumerator<JToken> enumerator = unitJsonArray.GetEnumerator())
            {
                while (((IEnumerator) enumerator).MoveNext())
                {
                    JObject current = (JObject) enumerator.Current;
                    string str1 = "id";
                    int id = (int) current.GetValue(str1).ToObject<int>();
                    string str2 = "cnt";
                    int num = (int)current.GetValue(str2).ToObject<int>();
                    m_vUnits.Add(new DataSlot(CsvManager.DataTables.GetDataById(id), num));
                }
            }
        }

        public bool ProductionCompleted()
        {
            var result = false;
            var cf = new ComponentFilter(0);
            var x = Parent.X;
            var y = Parent.Y;
            var cm = Parent.Level.GetComponentManager();
            var c = cm.GetClosestComponent(x, y, cf);

            while (c != null)
            {
                Data d = null;
                if (m_vUnits.Count > 0)
                    d = m_vUnits[0].Data;
                if (!((UnitStorageComponent) c).CanAddUnit((CombatItemData) d))
                {
                    cf.AddIgnoreObject(c.Parent);
                    c = cm.GetClosestComponent(x, y, cf);
                }
                else
                    break;
            }

            if (c != null)
            {
                var cd = (CombatItemData) m_vUnits[0].Data;
                ((UnitStorageComponent) c).AddUnit(cd);
                StartProducingNextUnit();
                result = true;
            }
            else
            {
                m_vIsWaitingForSpace = true;
            }
            return result;
        }

        public void RemoveUnit(CombatItemData cd)
        {
            var index = -1;
            if (GetSlotCount() >= 1)
            {
                for (var i = 0; i < GetSlotCount(); i++)
                {
                    if (m_vUnits[i].Data == cd)
                        index = i;
                }
            }
            if (index != -1)
            {
                if (m_vUnits[index].Value >= 1)
                {
                    m_vUnits[index].Value--;
                    if (m_vUnits[index].Value == 0)
                    {
                        m_vUnits.RemoveAt(index);
                        if (GetSlotCount() >= 1)
                        {
                            var ds = m_vUnits[0];
                            var newcd = (CombatItemData) m_vUnits[0].Data;
                            var ca = Parent.Level.GetHomeOwnerAvatar();
                            m_vTimer = new Timer();
                            var trainingTime = newcd.GetTrainingTime(ca.GetUnitUpgradeLevel(newcd));
                            m_vTimer.StartTimer(trainingTime, Parent.Level.Time);
                        }
                    }
                }
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            var unitProdObject = new JObject();
            if (m_vIsSpellForge)
                unitProdObject.Add("unit_type", 1);
            else
                unitProdObject.Add("unit_type", 0);

            if (m_vTimer != null)
            {
                unitProdObject.Add("t", m_vTimer.GetRemainingSeconds(Parent.Level.Time));
            }

            if (GetSlotCount() >= 1)
            {
                var unitJsonArray = new JArray();
                foreach (var unit in m_vUnits)
                {
                    var unitJsonObject = new JObject();
                    unitJsonObject.Add("id", unit.Data.GetGlobalID());
                    unitJsonObject.Add("cnt", unit.Value);
                    unitJsonArray.Add(unitJsonObject);
                }
                unitProdObject.Add("slots", unitJsonArray);
            }
            jsonObject.Add("unit_prod", unitProdObject);
            return jsonObject;
        }

        public void SetUnitType(GameObject go)
        {
            var b = (Building) Parent;
            var bd = b.BuildingData;
            m_vIsSpellForge = bd.IsSpellForge();
        }

        public void SpeedUp()
        {
            while (m_vUnits.Count >= 1 && ProductionCompleted())
            {
            }
        }

        public void StartProducingNextUnit()
        {
            m_vTimer = null;
            if (GetSlotCount() >= 1)
            {
                RemoveUnit((CombatItemData) m_vUnits[0].Data);
            }
        }

        public override void Tick()
        {
            if (m_vTimer != null)
            {
                if (m_vTimer.GetRemainingSeconds(Parent.Level.Time) <= 0)
                {
                    ProductionCompleted();
                }
            }
        }
    }
}