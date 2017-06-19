using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Resources : Data
    {
        internal Resources(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the resources.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Icon s w f.
        /// </summary>
        public string IconSWF { get; set; }

        /// <summary>
        /// Gets or sets Used in battle.
        /// </summary>
        public bool UsedInBattle { get; set; }

        /// <summary>
        /// Gets or sets Collect effect.
        /// </summary>
        public string CollectEffect { get; set; }

        /// <summary>
        /// Gets or sets Icon export name.
        /// </summary>
        public string IconExportName { get; set; }

        /// <summary>
        /// Gets or sets Premium currency.
        /// </summary>
        public bool PremiumCurrency { get; set; }

        /// <summary>
        /// Gets or sets Cap full t i d.
        /// </summary>
        public string CapFullTID { get; set; }

        /// <summary>
        /// Gets or sets Text red.
        /// </summary>
        public int TextRed { get; set; }

        /// <summary>
        /// Gets or sets Text green.
        /// </summary>
        public int TextGreen { get; set; }

        /// <summary>
        /// Gets or sets Text blue.
        /// </summary>
        public int TextBlue { get; set; }

        /// <summary>
        /// Gets or sets Cap.
        /// </summary>
        public int Cap { get; set; }

        /// <summary>
        /// Gets or sets Icon file.
        /// </summary>
        public string IconFile { get; set; }

        /// <summary>
        /// Gets or sets Shop icon.
        /// </summary>
        public string ShopIcon { get; set; }
    }
}