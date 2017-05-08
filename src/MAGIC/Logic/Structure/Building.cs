using System;
using System.Collections.Generic;
using BL.Servers.CoC.Logic.Components;

namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;
    internal class Building : ConstructionItem
    {
        public Building(Data data, Level level) : base(data, level)
        {
            AddComponent(new Hitpoint_Component());
            // if (GetBuildingData().UpgradesUnits)
            {
                // AddComponent(new UnitUpgradeComponent(this));
            }
             if (GetBuildingData().UnitProduction[0] > 0)
            {
               // AddComponent(new Unit_Production_Component(this));
            }

            //if (GetBuildingData().HousingSpace[0] > 0)
            {
                //AddComponent(new UnitStorageComponent(this, 0));
            }
            //if (GetBuildingData().Damage[0] > 0 || GetBuildingData().BuildingClass == "Defense")
            {
                //AddComponent(new CombatComponent(this, level));
            }
            if (!string.IsNullOrEmpty(GetBuildingData().ProducesResource))
            {
                //AddComponent(new ResourceProductionComponent(this, level));
            }

            if (GetBuildingData().MaxStoredGold[0] > 0 || GetBuildingData().MaxStoredElixir[0] > 0 || GetBuildingData().MaxStoredDarkElixir[0] > 0 || GetBuildingData().MaxStoredWarGold[0] > 0 || GetBuildingData().MaxStoredWarElixir[0] > 0 || GetBuildingData().MaxStoredWarDarkElixir[0] > 0)
            {
                AddComponent(new Resource_Storage_Component(this));
            }
        }


        internal override int ClassId => 0;

        public Buildings GetBuildingData() => (Buildings)GetData();
    }
}
