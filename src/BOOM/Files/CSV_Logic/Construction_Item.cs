namespace CRepublic.Boom.Files.CSV_Logic
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Reader;

    internal class Construction_Item : Data
    {
        public Construction_Item(Row row, DataTable dt) : base(row, dt)
        {
        }

        public virtual int GetBuildCost(int level) => -1;

        public virtual Resource GetBuildResource(int level) => null;

        public virtual int GetConstructionTime(int level) => -1;

        public virtual int GetRequiredTownHallLevel(int level) => -1;

        public virtual int GetUpgradeLevelCount() => -1;

        public virtual bool IsHQ() => false;
    }

}
