using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Tournament_Tiers : Data
    {
        internal Tournament_Tiers(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the tournament_tiers.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets Disabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets Create cost.
        /// </summary>
        public int CreateCost { get; set; }

        /// <summary>
        /// Gets or sets Max players.
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Gets or sets Prize1.
        /// </summary>
        public int Prize1 { get; set; }

        /// <summary>
        /// Gets or sets Prize2.
        /// </summary>
        public int Prize2 { get; set; }

        /// <summary>
        /// Gets or sets Prize3.
        /// </summary>
        public int Prize3 { get; set; }

        /// <summary>
        /// Gets or sets Prize10.
        /// </summary>
        public int Prize10 { get; set; }

        /// <summary>
        /// Gets or sets Prize20.
        /// </summary>
        public int Prize20 { get; set; }

        /// <summary>
        /// Gets or sets Prize30.
        /// </summary>
        public int Prize30 { get; set; }

        /// <summary>
        /// Gets or sets Prize40.
        /// </summary>
        public int Prize40 { get; set; }

        /// <summary>
        /// Gets or sets Prize50.
        /// </summary>
        public int Prize50 { get; set; }

        /// <summary>
        /// Gets or sets Prize60.
        /// </summary>
        public int Prize60 { get; set; }

        /// <summary>
        /// Gets or sets Prize70.
        /// </summary>
        public int Prize70 { get; set; }

        /// <summary>
        /// Gets or sets Prize80.
        /// </summary>
        public int Prize80 { get; set; }

        /// <summary>
        /// Gets or sets Prize90.
        /// </summary>
        public int Prize90 { get; set; }

        /// <summary>
        /// Gets or sets Prize100.
        /// </summary>
        public int Prize100 { get; set; }

        /// <summary>
        /// Gets or sets Prize150.
        /// </summary>
        public int Prize150 { get; set; }

        /// <summary>
        /// Gets or sets Prize200.
        /// </summary>
        public int Prize200 { get; set; }

        /// <summary>
        /// Gets or sets Prize250.
        /// </summary>
        public int Prize250 { get; set; }

        /// <summary>
        /// Gets or sets Prize300.
        /// </summary>
        public int Prize300 { get; set; }

        /// <summary>
        /// Gets or sets Prize350.
        /// </summary>
        public int Prize350 { get; set; }

        /// <summary>
        /// Gets or sets Prize400.
        /// </summary>
        public int Prize400 { get; set; }

        /// <summary>
        /// Gets or sets Prize450.
        /// </summary>
        public int Prize450 { get; set; }

        /// <summary>
        /// Gets or sets Prize500.
        /// </summary>
        public int Prize500 { get; set; }

        /// <summary>
        /// Gets or sets Open chest variation.
        /// </summary>
        public int OpenChestVariation { get; set; }
    }
}