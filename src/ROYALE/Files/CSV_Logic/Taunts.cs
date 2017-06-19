using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Taunts : Data
    {
        internal Taunts(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the taunts.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Taunt menu.
        /// </summary>
        public bool TauntMenu { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }

        /// <summary>
        /// Gets or sets Icon export name.
        /// </summary>
        public string IconExportName { get; set; }

        /// <summary>
        /// Gets or sets Btn export name.
        /// </summary>
        public string BtnExportName { get; set; }

        /// <summary>
        /// Gets or sets Sound.
        /// </summary>
        public string Sound { get; set; }
    }
}