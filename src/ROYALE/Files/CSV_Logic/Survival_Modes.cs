using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Survival_Modes : Data
    {
        internal Survival_Modes(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the survival_modes.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Icon s w f.
        /// </summary>
        public string IconSWF { get; set; }

        /// <summary>
        /// Gets or sets Icon export name.
        /// </summary>
        public string IconExportName { get; set; }

        /// <summary>
        /// Gets or sets Game mode.
        /// </summary>
        public string GameMode { get; set; }

        /// <summary>
        /// Gets or sets Wins icon export name.
        /// </summary>
        public string WinsIconExportName { get; set; }

        /// <summary>
        /// Gets or sets Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets Event only.
        /// </summary>
        public bool EventOnly { get; set; }

        /// <summary>
        /// Gets or sets Join cost.
        /// </summary>
        public int JoinCost { get; set; }

        /// <summary>
        /// Gets or sets Join cost resource.
        /// </summary>
        public string JoinCostResource { get; set; }

        /// <summary>
        /// Gets or sets Free pass.
        /// </summary>
        public int FreePass { get; set; }

        /// <summary>
        /// Gets or sets Max wins.
        /// </summary>
        public int MaxWins { get; set; }

        /// <summary>
        /// Gets or sets Max loss.
        /// </summary>
        public int MaxLoss { get; set; }

        /// <summary>
        /// Gets or sets Reward cards.
        /// </summary>
        public int RewardCards { get; set; }

        /// <summary>
        /// Gets or sets Reward gold.
        /// </summary>
        public int RewardGold { get; set; }

        /// <summary>
        /// Gets or sets Reward spell count.
        /// </summary>
        public int RewardSpellCount { get; set; }

        /// <summary>
        /// Gets or sets Reward spell.
        /// </summary>
        public string RewardSpell { get; set; }

        /// <summary>
        /// Gets or sets Reward spell max count.
        /// </summary>
        public int RewardSpellMaxCount { get; set; }

        /// <summary>
        /// Gets or sets Item export name.
        /// </summary>
        public string ItemExportName { get; set; }

        /// <summary>
        /// Gets or sets Confirm export name.
        /// </summary>
        public string ConfirmExportName { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Card theme.
        /// </summary>
        public string CardTheme { get; set; }
    }
}