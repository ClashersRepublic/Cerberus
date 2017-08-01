using Magic.ClashOfClans.Core;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic
{
    internal class Building : ConstructionItem
    {
        public Building(Data data, Level level) : base(data, level)
        {
            Locked = BuildingData.Locked;
            AddComponent(new HitpointComponent());
            if (BuildingData.IsHeroBarrack)
            {
                var hd = CsvManager.DataTables.GetHeroByName(BuildingData.HeroType);
                AddComponent(new HeroBaseComponent(this, hd));
            }
            if (BuildingData.UpgradesUnits)
                AddComponent(new UnitUpgradeComponent(this));
            if (BuildingData.UnitProduction[0] > 0)
                AddComponent(new UnitProductionComponent(this));
            if (BuildingData.HousingSpace[0] > 0)
            {
                if (BuildingData.Bunker)
                    AddComponent(new BunkerComponent());
                else
                    AddComponent(new UnitStorageComponent(this, 0));
            }
            if (BuildingData.Damage[0] > 0 || BuildingData.BuildingClass == "Defense")
                AddComponent(new CombatComponent(this, level));
            if (BuildingData.ProducesResource != null && BuildingData.ProducesResource != string.Empty)
            {
                var s = BuildingData.ProducesResource;
                AddComponent(new ResourceProductionComponent(this, level));
            }
            if (BuildingData.MaxStoredGold[0] > 0 ||
                BuildingData.MaxStoredElixir[0] > 0 ||
                BuildingData.MaxStoredDarkElixir[0] > 0 ||
                BuildingData.MaxStoredWarGold[0] > 0 ||
                BuildingData.MaxStoredWarElixir[0] > 0 ||
                BuildingData.MaxStoredWarDarkElixir[0] > 0)
                AddComponent(new ResourceStorageComponent(this));
        }

        public override int ClassId => 0;

        public BuildingData BuildingData => (BuildingData)Data;
    }
}