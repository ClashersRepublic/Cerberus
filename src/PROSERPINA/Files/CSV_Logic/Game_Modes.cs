using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Game_Modes : Data
    {
        internal Game_Modes(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the game_modes.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Card level adjustment.
        /// </summary>
        public string CardLevelAdjustment { get; set; }

        /// <summary>
        /// Gets or sets Deck selection.
        /// </summary>
        public string DeckSelection { get; set; }

        /// <summary>
        /// Gets or sets Overtime seconds.
        /// </summary>
        public int OvertimeSeconds { get; set; }

        /// <summary>
        /// Gets or sets Predefined decks.
        /// </summary>
        public string PredefinedDecks { get; set; }

        /// <summary>
        /// Gets or sets Elixir production multiplier.
        /// </summary>
        public int ElixirProductionMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Elixir production overtime multiplier.
        /// </summary>
        public int ElixirProductionOvertimeMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Use starting elixir.
        /// </summary>
        public bool UseStartingElixir { get; set; }

        /// <summary>
        /// Gets or sets Starting elixir.
        /// </summary>
        public int StartingElixir { get; set; }

        /// <summary>
        /// Gets or sets Heroes.
        /// </summary>
        public bool Heroes { get; set; }

        /// <summary>
        /// Gets or sets Forced deck cards.
        /// </summary>
        public string ForcedDeckCards { get; set; }

        /// <summary>
        /// Gets or sets Help export name.
        /// </summary>
        public string HelpExportName { get; set; }

        /// <summary>
        /// Gets or sets Team.
        /// </summary>
        public bool Team { get; set; }

        /// <summary>
        /// Gets or sets Event deck set limit.
        /// </summary>
        public string EventDeckSetLimit { get; set; }

        /// <summary>
        /// Gets or sets Forced deck cards using card theme.
        /// </summary>
        public bool ForcedDeckCardsUsingCardTheme { get; set; }

        /// <summary>
        /// Gets or sets Confirm export name.
        /// </summary>
        public string ConfirmExportName { get; set; }
    }
}