using Magic.Files.CSV;

namespace Magic.Files.Logic
{
    internal class LocalesData : Data
    {
        public LocalesData(CsvRow row, DataTable dt) : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string Description { get; set; }
        public bool HasEvenSpaceCharacters { get; set; }
        public string HelpshiftSDKLanguage { get; set; }
        public string HelpshiftSDKLanguageAndroid { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string TestExcludes { get; set; }
        public bool TestLanguage { get; set; }
        public string UsedSystemFont { get; set; }
    }
}
