using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.BB.Files.CSV_Helpers;
using BL.Servers.BB.Files.CSV_Reader;

namespace BL.Servers.BB.Files.CSV_Logic
{
    internal class Decos : Data
    {
        public Decos(Row row, DataTable dt) : base(row, dt)
        {
            Load(row);
            //LoadData(this, GetType(), row);
        }
        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public string ExportName { get; set; }
        public string ExportNameConstruction { get; set; }
        public string BuildCost { get; set; }
        public int RequiredExpLevel { get; set; }
        public int MaxCount { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Icon { get; set; }
        public string ExportNameBase { get; set; }
        public int VillagerProbability { get; set; }

    }
}
