using System;
using System.Collections.Generic;
using CRepublic.Boom.Logic.Components;

namespace CRepublic.Boom.Logic.Structure
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Logic;
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

            if (!string.IsNullOrEmpty(GetBuildingData().MaxStoredResource[0]))
            {
                string[] resource = GetBuildingData().MaxStoredResource[0].Split(',');
                if (Convert.ToInt32(resource[0]) > 0 || Convert.ToInt32(resource[1]) > 0 ||
                    Convert.ToInt32(resource[2]) > 0 || Convert.ToInt32(resource[3]) > 0 ||
                    Convert.ToInt32(resource[4]) > 0)
                {
                 //   AddComponent(new ResourceStorageComponent(this));
                }
            }
        }


        internal override int ClassId => 0;

        public Buildings GetBuildingData() => (Buildings)GetData();
    }
}
