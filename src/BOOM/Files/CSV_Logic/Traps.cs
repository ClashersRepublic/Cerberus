namespace CRepublic.Boom.Files.CSV_Logic
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Reader;

    internal class Traps : Construction_Item
    {
        public Traps(Row row, DataTable dt) : base(row, dt)
        {
            Load(row);
            //LoadData(this, GetType(), row);
        }
        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SubtitleTID { get; set; }
        public int[] DefenseValue { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public string IconSWF { get; set; }
        public string BigPictureSWF { get; set; }
        public string BigPicture { get; set; }
        public int[] UpgradeTimeH { get; set; }
        public string[] UpgradeCost { get; set; }
        public int[] XpGain { get; set; }
        public int[] UpgradeHouseLevel { get; set; }
        public int[] Damage { get; set; }
        public int DamageRadius { get; set; }
        public int TriggerRadius { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Effect { get; set; }
        public string Effect2 { get; set; }
        public string DamageEffect { get; set; }
        public bool Passable { get; set; }
        public string[] BuildCost { get; set; }
        public string ExportNameTriggered { get; set; }
        public int ActionFrame { get; set; }
        public string PickUpEffect { get; set; }
        public string PlacingEffect { get; set; }
        public string AppearEffect { get; set; }
        public bool CountersArmored { get; set; }

    }
}
