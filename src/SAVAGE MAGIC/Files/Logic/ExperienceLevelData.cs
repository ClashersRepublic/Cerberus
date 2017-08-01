using Magic.Files.CSV;

namespace Magic.Files.Logic
{
    internal class ExperienceLevelData : Data
    {
        public ExperienceLevelData(CsvRow row, DataTable dt): base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public int ExpPoints { get; set; }
    }
}
