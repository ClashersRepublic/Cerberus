using Newtonsoft.Json.Linq;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic
{
    internal class CombatComponent : Component
    {
        public CombatComponent(ConstructionItem ci, Level level) : base(ci)
        {
            var bd = (BuildingData) ci.Data;
            if (bd.AmmoCount != 0)
            {
                m_vAmmo = bd.AmmoCount;
            }
        }

        public override int Type => 1;

        const int m_vType = 0x01AB3F00;

        int m_vAmmo;

        public void FillAmmo()
        {
            var ca = Parent.Level.Avatar;
            var bd = (BuildingData) Parent.Data;
            var rd = CsvManager.DataTables.GetResourceByName(bd.AmmoResource);

            if (ca.HasEnoughResources(rd, bd.AmmoCost))
            {
                ca.CommodityCountChangeHelper(0, rd, bd.AmmoCost);
                m_vAmmo = bd.AmmoCount;
            }
        }

        public override void Load(JObject jsonObject)
        {
            if (jsonObject["ammo"] != null)
            {
                m_vAmmo = jsonObject["ammo"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject.Add("ammo", m_vAmmo);
            return jsonObject;
        }
    }
}
