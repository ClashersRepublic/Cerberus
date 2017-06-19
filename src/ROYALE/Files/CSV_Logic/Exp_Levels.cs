using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Exp_Levels : Data
    {
        internal Exp_Levels(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the exp_levels.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Exp to next level.
        /// </summary>
        public int ExpToNextLevel { get; set; }

        /// <summary>
        /// Gets or sets Summoner level.
        /// </summary>
        public int SummonerLevel { get; set; }

        /// <summary>
        /// Gets or sets Tower level.
        /// </summary>
        public int TowerLevel { get; set; }

        /// <summary>
        /// Gets or sets Troop level.
        /// </summary>
        public int TroopLevel { get; set; }

        /// <summary>
        /// Gets or sets Decks.
        /// </summary>
        public int Decks { get; set; }

        /// <summary>
        /// Gets or sets Summoner kill gold.
        /// </summary>
        public int SummonerKillGold { get; set; }

        /// <summary>
        /// Gets or sets Tower kill gold.
        /// </summary>
        public int TowerKillGold { get; set; }

        /// <summary>
        /// Gets or sets Diamond reward.
        /// </summary>
        public int DiamondReward { get; set; }
    }
}