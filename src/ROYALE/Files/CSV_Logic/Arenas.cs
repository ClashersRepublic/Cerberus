using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Arenas : Data
    {
        internal Arenas(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Subtitle t i d.
        /// </summary>
        public string SubtitleTID { get; set; }

        /// <summary>
        /// Gets or sets Arena.
        /// </summary>
        public int Arena { get; set; }

        /// <summary>
        /// Gets or sets Chest arena.
        /// </summary>
        public string ChestArena { get; set; }

        /// <summary>
        /// Gets or sets Tv arena.
        /// </summary>
        public string TvArena { get; set; }

        /// <summary>
        /// Gets or sets Is in use.
        /// </summary>
        public bool IsInUse { get; set; }

        /// <summary>
        /// Gets or sets Training camp.
        /// </summary>
        public bool TrainingCamp { get; set; }

        /// <summary>
        /// Gets or sets Trophy limit.
        /// </summary>
        public int TrophyLimit { get; set; }

        /// <summary>
        /// Gets or sets Demote trophy limit.
        /// </summary>
        public int DemoteTrophyLimit { get; set; }

        /// <summary>
        /// Gets or sets Season trophy reset.
        /// </summary>
        public int SeasonTrophyReset { get; set; }

        /// <summary>
        /// Gets or sets Chest reward multiplier.
        /// </summary>
        public int ChestRewardMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Chest shop price multiplier.
        /// </summary>
        public int ChestShopPriceMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Request size.
        /// </summary>
        public int RequestSize { get; set; }

        /// <summary>
        /// Gets or sets Max donation count common.
        /// </summary>
        public int MaxDonationCountCommon { get; set; }

        /// <summary>
        /// Gets or sets Max donation count rare.
        /// </summary>
        public int MaxDonationCountRare { get; set; }

        /// <summary>
        /// Gets or sets Max donation count epic.
        /// </summary>
        public int MaxDonationCountEpic { get; set; }

        /// <summary>
        /// Gets or sets Icon s w f.
        /// </summary>
        public string IconSWF { get; set; }

        /// <summary>
        /// Gets or sets Icon export name.
        /// </summary>
        public string IconExportName { get; set; }

        /// <summary>
        /// Gets or sets Main menu icon export name.
        /// </summary>
        public string MainMenuIconExportName { get; set; }

        /// <summary>
        /// Gets or sets Small icon export name.
        /// </summary>
        public string SmallIconExportName { get; set; }

        /// <summary>
        /// Gets or sets Matchmaking min trophy delta.
        /// </summary>
        public int MatchmakingMinTrophyDelta { get; set; }

        /// <summary>
        /// Gets or sets Matchmaking max trophy delta.
        /// </summary>
        public int MatchmakingMaxTrophyDelta { get; set; }

        /// <summary>
        /// Gets or sets Matchmaking max seconds.
        /// </summary>
        public int MatchmakingMaxSeconds { get; set; }

        /// <summary>
        /// Gets or sets Pvp location.
        /// </summary>
        public string PvpLocation { get; set; }

        /// <summary>
        /// Gets or sets Team vs team location.
        /// </summary>
        public string TeamVsTeamLocation { get; set; }

        /// <summary>
        /// Gets or sets Daily donation capacity limit.
        /// </summary>
        public int DailyDonationCapacityLimit { get; set; }

        /// <summary>
        /// Gets or sets Battle reward gold.
        /// </summary>
        public int BattleRewardGold { get; set; }

        /// <summary>
        /// Gets or sets Release date.
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets Season reward chest.
        /// </summary>
        public string SeasonRewardChest { get; set; }
    }
}