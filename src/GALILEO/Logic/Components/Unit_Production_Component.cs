using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Logic.Structure;
using BL.Servers.CoC.Logic.Structure.Slots;

namespace BL.Servers.CoC.Logic.Components
{
    internal class Unit_Production_Component  : Component
    {
        internal List<DataSlot> Units;
        internal bool IsSpellForge;
        internal bool IsWaitingForSpace;
        internal Timer Timer;

        public Unit_Production_Component(GameObject go) : base(go)
        {
            this.Units = new List<DataSlot>();
            SetUnitType(go);
            this.Timer = null;
            this.IsWaitingForSpace = false;
        }

        internal override int Type => 3;

        internal void SetUnitType(GameObject go)
        {
            if (go.ClassId <= 6 || go.ClassId == 8)
            {
                var b = (Building) GetParent;
                var bd = b.GetBuildingData;
                this.IsSpellForge = bd.ForgesSpells || bd.ForgesSpells;
            }
            else
            {
                var b = (Builder_Building)GetParent;
                var bd = b.GetBuildingData;
                this.IsSpellForge = bd.ForgesSpells || bd.ForgesSpells;

            }
        }
    }
}
