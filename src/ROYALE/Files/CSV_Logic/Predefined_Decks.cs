using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Predefined_Decks : Data
    {
        internal Predefined_Decks(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the predefined_decks.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Spells.
        /// </summary>
        public string Spells { get; set; }

        /// <summary>
        /// Gets or sets Spell level.
        /// </summary>
        public int SpellLevel { get; set; }

        /// <summary>
        /// Gets or sets Random spell sets.
        /// </summary>
        public string RandomSpellSets { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }
    }
}