using Magic.Files.CSV;

namespace Magic.Files.Logic
{
    internal class RegionsData : Data
    {
        public RegionsData(CsvRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string DisplayName { get; set; }
        public bool IsCountry { get; set; }
        public string Name { get; set; }
        public string TID { get; set; }
    }
}
