using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic
{
    internal class UnitProduction
    {
        public UnitProduction(CombatItemData cd)
        {
            m_vUnits = new List<DataSlot>();
            Unit = cd;
            m_vTimer = null;
        }

        public void AddUnitToQueue(CombatItemData cd)
        {
            for (var i = 0; i < m_vUnits.Count; i++)
            {
                if ((CombatItemData)m_vUnits[i].Data == cd)
                {
                    m_vUnits[i].Value++;
                    return;
                }
            }

            var ds = new DataSlot(cd, 1);
            m_vUnits.Add(ds);

            if (m_vTimer == null)
            {

            }
        }

        public int GetSlotCount() => m_vUnits.Count;

        public int GetTrainingCount(int index) => m_vUnits[index].Value;

        public CombatItemData GetUnit(int index) => (CombatItemData)m_vUnits[index].Data;

        /*public override void Load(JObject jsonObject)
        {
            var unitProdObject = (JObject)jsonObject["unit_prod"];
            m_vIsSpellForge = unitProdObject["unit_type"].ToObject<int>() == 1;
            var timeToken = unitProdObject["t"];
            if (timeToken != null)
            {
                m_vTimer = new Timer();
                var remainingTime = timeToken.ToObject<int>();
                m_vTimer.StartTimer(remainingTime, GetParent().GetLevel().GetTime());
            }
            var unitJsonArray = (JArray)unitProdObject["slots"];
            if (unitJsonArray != null)
            {
                foreach (JObject unitJsonObject in unitJsonArray)
                {
                    var id = unitJsonObject["id"].ToObject<int>();
                    var cnt = unitJsonObject["cnt"].ToObject<int>();
                    m_vUnits.Add(new DataSlot(CSVManager.DataTables.GetDataById(id), cnt));
                }
            }
        }*/

        /*public override JObject Save(JObject jsonObject)
        {
            var unitProdObject = new JObject();
            if (m_vIsSpellForge)
                unitProdObject.Add("unit_type", 1);
            else
                unitProdObject.Add("unit_type", 0);

            if (m_vTimer != null)
            {
                unitProdObject.Add("t", m_vTimer.GetRemainingSeconds(GetParent().GetLevel().GetTime()));
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
        }*/

        Timer m_vTimer;

        readonly List<DataSlot> m_vUnits;

        public CombatItemData Unit { get; set; }
    }
}
