using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Reader;

namespace BL.Servers.CoC.Files.CSV_Logic
{
    internal class Resource : Data
    {
        public Resource(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
        }
        public string Name { get; set; }
        public string TID { get; set; }
        public string SWF { get; set; }
        public string CollectEffect { get; set; }
        public string ResourceIconExportName { get; set; }
        public string StealEffect { get; set; }
        public int StealLimitMid { get; set; }
        public string StealEffectMid { get; set; }
        public int StealLimitBig { get; set; }
        public string StealEffectBig { get; set; }
        public bool PremiumCurrency { get; set; }
        public string HudInstanceName { get; set; }
        public string CapFullTID { get; set; }
        public int TextRed { get; set; }
        public int TextGreen { get; set; }
        public int TextBlue { get; set; }
        public string WarRefResource { get; set; }

    }
}
