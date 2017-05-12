using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Achievements : Data
    {
        public Achievements(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        internal string Name { get; set; }

        internal int Level { get; set; }

        internal string TID { get; set; }

        internal string InfoTID { get; set; }

        internal string Action { get; set; }

        internal int ActionCount { get; set; }

        internal int ExpReward { get; set; }

        internal int DiamondReward { get; set; }

        internal int SortIndex { get; set; }

        internal bool Hidden { get; set; }

        internal string AndroidID { get; set; }

        internal string Type { get; set; }
    }
}
