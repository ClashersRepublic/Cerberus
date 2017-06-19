using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Regions : Data
    {
        internal Regions(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the regions.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets Is country.
        /// </summary>
        public bool IsCountry { get; set; }

        /// <summary>
        /// Gets or sets Region popup.
        /// </summary>
        public bool RegionPopup { get; set; }
    }
}