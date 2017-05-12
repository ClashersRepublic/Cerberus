using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Resource_Packs : Data
    {
        internal Resource_Packs(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the resource_packs.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Resource.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets Amount.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets Icon file.
        /// </summary>
        public string IconFile { get; set; }
    }
}