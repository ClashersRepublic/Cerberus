using Magic.Files.CSV;

namespace Magic.Files.Logic
{
    internal class EffectsData : Data
    {
        public EffectsData(CsvRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
    }
}
