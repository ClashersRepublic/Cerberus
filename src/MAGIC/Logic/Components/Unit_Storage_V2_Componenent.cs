using CRepublic.Magic.Logic.Structure.Slots;
using System.Collections.Generic;
using System.Linq;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Components
{
    internal class Unit_Storage_V2_Componenent : Component
    {
        internal List<DataSlot> Units;
        internal int MaxCapacity;
        internal override int Type => 11;

        public Unit_Storage_V2_Componenent(GameObject go, int capacity) : base(go)
        {
            this.Units = new List<DataSlot>();
            this.MaxCapacity = capacity;
        }

        internal void AddUnit(Combat_Item cd)
        {
            AddUnitImpl(cd);
        }

        internal bool CanAddUnit(Combat_Item cd)
        {
            var result = false;
            if (cd != null)
            {
                var cm = GetParent.Level.GetComponentManager;
                var maxCapacity = cm.GetTotalMaxHousingV2();
                var usedCapacity = cm.GetTotalUsedHousing();
                var housingSpace = cd.GetHousingSpace();
                if (GetUsedCapacity() < this.MaxCapacity)
                    result = maxCapacity >= usedCapacity + housingSpace;

            }
            return result;
        }

        internal void AddUnitImpl(Combat_Item cd)
        {
            //if (CanAddUnit(cd))
            {
                var ca = GetParent.Level.Avatar;
                var UnitInCamp = ((Characters) cd).UnitsInCamp[ca.GetUnitUpgradeLevel(cd)];
                var unitIndex = GetUnitTypeIndex(cd);
                if (unitIndex == -1)
                {
                    var us = new DataSlot(cd, UnitInCamp);
                    this.Units.Add(us);
                }
                else
                {
                    this.Units[unitIndex].Value += UnitInCamp;
                }
                var unitCount = ca.Get_Unit_Count_V2(cd);
                ca.Set_Unit_Count_V2(cd, unitCount + UnitInCamp);
            }
        }

        public int GetUnitCountByData(Combat_Item cd)
        {
            return this.Units.Where(t => t.Data == cd).Sum(t => t.Value);
        }

        internal int GetUnitTypeIndex(Combat_Item cd)
        {
            var index = -1;
            for (var i = 0; i < this.Units.Count; i++)
            {
                if (this.Units[i].Data == cd)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        internal int GetUsedCapacity()
        {
            var count = 0;
            if (this.Units.Count >= 1)
            {
                count += (from t in this.Units let cnt = t.Value let housingSpace = ((Combat_Item) t.Data).GetHousingSpace() select cnt * housingSpace).Sum();
            }
            return count;
        }

        public void RemoveUnits(Combat_Item cd, int count)
        {
            RemoveUnitsImpl(cd, count);
        }

        public void RemoveUnitsImpl(Combat_Item cd, int count)
        {
            var unitIndex = GetUnitTypeIndex(cd);
            if (unitIndex != -1)
            {
                var us = this.Units[unitIndex];
                if (us.Value <= count)
                {
                    this.Units.Remove(us);
                }
                else
                {
                    us.Value -= count;
                }
                var ca = GetParent.Level.Avatar;
                var unitCount = ca.Get_Unit_Count_V2(cd);
                ca.Set_Unit_Count_V2(cd, unitCount - count);
                
            }
        }

        internal override void Load(JObject jsonObject)
        {

            var unitObject = (JObject)jsonObject["up2"];
            var unitArray = (JArray) unitObject?["unit"];
            if (unitArray?.Count > 0)
            {
                var id = unitArray[0].ToObject<int>();
                var cnt = unitArray[1].ToObject<int>();
                this.Units.Add(new DataSlot((Combat_Item) CSV.Tables.GetWithGlobalID(id), cnt));
            }
        }

        internal override JObject Save(JObject jsonObject)
        {
            var unitObject = new JObject();
            var unitJsonArray = new JArray();
            if (this.Units.Count > 0)
            {
                foreach (var unit in this.Units)
                {
                    unitJsonArray = new JArray {unit.Data.GetGlobalID(), unit.Value};
                }
                unitObject.Add("unit", unitJsonArray);
            }
            jsonObject.Add("up2", unitObject);
            return jsonObject;
        }
    }
}
