using Magic.Files.CSV;

namespace Magic.Files.Logic
{
    internal class ShieldData : Data
    {
        public ShieldData(CsvRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int CooldownS { get; set; }
        public int Diamonds { get; set; }
        public string IconExportName { get; set; }
        public string IconSWF { get; set; }
        public string InfoTID { get; set; }
        public int LockedAboveScore { get; set; }
        public string TID { get; set; }
        public int TimeH { get; set; }
    }
}
