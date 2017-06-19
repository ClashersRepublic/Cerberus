using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Content_Tests : Data
    {
        internal Content_Tests(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the content_tests.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Source data.
        /// </summary>
        public string SourceData { get; set; }

        /// <summary>
        /// Gets or sets Target data.
        /// </summary>
        public string TargetData { get; set; }

        /// <summary>
        /// Gets or sets Stat1.
        /// </summary>
        public string Stat1 { get; set; }

        /// <summary>
        /// Gets or sets Operator.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets Stat2.
        /// </summary>
        public string Stat2 { get; set; }

        /// <summary>
        /// Gets or sets Result.
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Gets or sets Enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}