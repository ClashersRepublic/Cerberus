using Magic.Files.CSV;

namespace Magic.Files.Logic
{
    internal class VariablesData : Data
    {
        public VariablesData(CsvRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
        public int Value { get; set; }
    }
}
