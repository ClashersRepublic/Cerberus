namespace CRepublic.Boom.Files.CSV_Logic
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Reader;

    internal class Resource : Data
    {
        public Resource(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
            //LoadData(this, this.GetType(), _Row);
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string SubtitleTID { get; set; }
        public string CategoryTID { get; set; }
        public string SWF { get; set; }
        public string CollectEffect { get; set; }
        public string IconExportName { get; set; }
        public string ResourceIconExportName { get; set; }
        public string GroupIconExportName { get; set; }
        public string StealEffect { get; set; }
        public string DestroyEffect { get; set; }
        public bool BaseResource { get; set; }
        public bool HeroResource { get; set; }
        public int ArtifactType { get; set; }
        public int ArtifactTier { get; set; }
        public int PrototypePart { get; set; }
        public bool PremiumCurrency { get; set; }
        public string HudInstanceName { get; set; }
        public string CapFullTID { get; set; }
        public int TextRed { get; set; }
        public int TextGreen { get; set; }

    }
}
