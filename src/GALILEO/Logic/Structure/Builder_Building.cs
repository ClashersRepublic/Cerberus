using BL.Servers.CoC.Logic.Components;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Logic.Structure
{
    using BL.Servers.CoC.Files;
    using BL.Servers.CoC.Files.CSV_Helpers;
    using BL.Servers.CoC.Files.CSV_Logic;
    internal class Builder_Building : ConstructionItem
    {
        public Builder_Building(Data data, Level level) : base(data, level)
        {
            AddComponent(new Hitpoint_Component());
            if (GetBuildingData.IsHeroBarrack)
            {
                Heroes hd = CSV.Tables.Get(Gamefile.Heroes).GetData(GetBuildingData.HeroType) as Heroes;
                AddComponent(new Hero_Base_Component(this, hd));
            }
            if (GetBuildingData.UpgradesUnits)
                AddComponent(new Unit_Upgrade_Component(this));
            if (GetBuildingData.UnitProduction[0] > 0)
            {
                AddComponent(new Unit_Production_Component(this));
            }
            //if (GetBuildingData.HousingSpace[0] > 0)
            {
                //AddComponent(new UnitStorageComponent(this, 0));
            }
            if (GetBuildingData.BuildingClass == "Defense")
            {
                AddComponent(new Combat_Component(this));
            }
            if (!string.IsNullOrEmpty(GetBuildingData.ProducesResource))
            {
                AddComponent(new Resource_Production_Component(this, level));
            }

            if (GetBuildingData.MaxStoredElixir2[0] > 0 || GetBuildingData.MaxStoredGold2[0] > 0)
            {
                AddComponent(new Resource_Storage_Component(this));
            }
        }


        internal override int ClassId => 7;

        public Buildings GetBuildingData => (Buildings)GetData();
    }
}
