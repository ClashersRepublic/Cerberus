using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class NPCS : Data
    {
        internal NPCS(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the npcs.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets Predefined deck.
        /// </summary>
        public string PredefinedDeck { get; set; }

        /// <summary>
        /// Gets or sets Trophies.
        /// </summary>
        public int Trophies { get; set; }

        /// <summary>
        /// Gets or sets Mana regen ms.
        /// </summary>
        public int ManaRegenMs { get; set; }

        /// <summary>
        /// Gets or sets Mana regen ms end.
        /// </summary>
        public int ManaRegenMsEnd { get; set; }

        /// <summary>
        /// Gets or sets Mana regen ms overtime.
        /// </summary>
        public int ManaRegenMsOvertime { get; set; }

        /// <summary>
        /// Gets or sets Exp level.
        /// </summary>
        public int ExpLevel { get; set; }

        /// <summary>
        /// Gets or sets Can replay.
        /// </summary>
        public bool CanReplay { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Exp reward.
        /// </summary>
        public int ExpReward { get; set; }

        /// <summary>
        /// Gets or sets Seed.
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        /// Gets or sets Full deck not needed.
        /// </summary>
        public bool FullDeckNotNeeded { get; set; }

        /// <summary>
        /// Gets or sets Mana reserve.
        /// </summary>
        public int ManaReserve { get; set; }

        /// <summary>
        /// Gets or sets Starting mana.
        /// </summary>
        public int StartingMana { get; set; }

        /// <summary>
        /// Gets or sets Wizard hp multiplier.
        /// </summary>
        public int WizardHpMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Start taunt.
        /// </summary>
        public string StartTaunt { get; set; }

        /// <summary>
        /// Gets or sets Own tower destroyed taunt.
        /// </summary>
        public string OwnTowerDestroyedTaunt { get; set; }

        /// <summary>
        /// Gets or sets Highlight targets on mana full.
        /// </summary>
        public bool HighlightTargetsOnManaFull { get; set; }
    }
}