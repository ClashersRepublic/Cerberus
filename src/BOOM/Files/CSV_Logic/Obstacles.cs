namespace CRepublic.Boom.Files.CSV_Logic
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Reader;

    internal class Obstacles : Data
    {
        public Obstacles(Row row, DataTable dt) : base(row, dt)
        {
            Load(row);
        }
        public string Name { get; set; }
        public string TID { get; set; }
        public string TIDIce { get; set; }
        public string TIDFire { get; set; }
        public string[] SWF { get; set; }
        public string ExportName { get; set; }
        public string ExportNameIce { get; set; }
        public string ExportNameFire { get; set; }
        public string ExportNameBase { get; set; }
        public int ClearTimeSeconds { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Passable { get; set; }
        public string ClearCost { get; set; }
        public string LootResource { get; set; }
        public int LootCount { get; set; }
        public string ClearEffect { get; set; }
        public string ClearEffectIce { get; set; }
        public string ClearEffectFire { get; set; }
        public string PickUpEffect { get; set; }
        public string PickUpEffectIce { get; set; }
        public string PickUpEffectFire { get; set; }
        public int RespawnWeight { get; set; }
        public string DepositResource { get; set; }

    }
}
