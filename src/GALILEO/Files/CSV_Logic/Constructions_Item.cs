using Republic.Magic.Files.CSV_Helpers;
using Republic.Magic.Files.CSV_Reader;

namespace Republic.Magic.Files.CSV_Logic
{
    internal class Construction_Item : Data
    {
        public Construction_Item(Row row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetBuildCost(int level) => -1;

        public virtual Resource GetBuildResource(int level) => null;

        public virtual int GetConstructionTime(int level) => -1;

        public virtual int GetGearUpTime(int level) => -1;

        public virtual int GetRequiredTownHallLevel(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual bool IsTownHall() => false;
        public virtual bool IsTownHall2() => false;
    }

}
