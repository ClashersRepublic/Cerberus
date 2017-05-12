using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Treasure_Chests : Data
    {
        internal Treasure_Chests(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the treasure_chests.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Base chest.
        /// </summary>
        public string BaseChest { get; set; }

        /// <summary>
        /// Gets or sets Arena.
        /// </summary>
        public string Arena { get; set; }

        /// <summary>
        /// Gets or sets In shop.
        /// </summary>
        public bool InShop { get; set; }

        /// <summary>
        /// Gets or sets In arena info.
        /// </summary>
        public bool InArenaInfo { get; set; }

        /// <summary>
        /// Gets or sets Tournament chest.
        /// </summary>
        public bool TournamentChest { get; set; }

        /// <summary>
        /// Gets or sets Survival chest.
        /// </summary>
        public bool SurvivalChest { get; set; }

        /// <summary>
        /// Gets or sets Shop price without speed up.
        /// </summary>
        public int ShopPriceWithoutSpeedUp { get; set; }

        /// <summary>
        /// Gets or sets Time taken days.
        /// </summary>
        public int TimeTakenDays { get; set; }

        /// <summary>
        /// Gets or sets Time taken hours.
        /// </summary>
        public int TimeTakenHours { get; set; }

        /// <summary>
        /// Gets or sets Time taken minutes.
        /// </summary>
        public int TimeTakenMinutes { get; set; }

        /// <summary>
        /// Gets or sets Time taken seconds.
        /// </summary>
        public int TimeTakenSeconds { get; set; }

        /// <summary>
        /// Gets or sets Random spells.
        /// </summary>
        public int RandomSpells { get; set; }

        /// <summary>
        /// Gets or sets Different spells.
        /// </summary>
        public int DifferentSpells { get; set; }

        /// <summary>
        /// Gets or sets Chest count in chest cycle.
        /// </summary>
        public int ChestCountInChestCycle { get; set; }

        /// <summary>
        /// Gets or sets Rare chance.
        /// </summary>
        public int RareChance { get; set; }

        /// <summary>
        /// Gets or sets Epic chance.
        /// </summary>
        public int EpicChance { get; set; }

        /// <summary>
        /// Gets or sets Legendary chance.
        /// </summary>
        public int LegendaryChance { get; set; }

        /// <summary>
        /// Gets or sets Guaranteed spells.
        /// </summary>
        public string GuaranteedSpells { get; set; }

        /// <summary>
        /// Gets or sets Min gold per card.
        /// </summary>
        public int MinGoldPerCard { get; set; }

        /// <summary>
        /// Gets or sets Max gold per card.
        /// </summary>
        public int MaxGoldPerCard { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }

        /// <summary>
        /// Gets or sets Shop export name.
        /// </summary>
        public string ShopExportName { get; set; }

        /// <summary>
        /// Gets or sets Gained export name.
        /// </summary>
        public string GainedExportName { get; set; }

        /// <summary>
        /// Gets or sets Anim export name.
        /// </summary>
        public string AnimExportName { get; set; }

        /// <summary>
        /// Gets or sets Open instance name.
        /// </summary>
        public string OpenInstanceName { get; set; }

        /// <summary>
        /// Gets or sets Slot land effect.
        /// </summary>
        public string SlotLandEffect { get; set; }

        /// <summary>
        /// Gets or sets Open effect.
        /// </summary>
        public string OpenEffect { get; set; }

        /// <summary>
        /// Gets or sets Tap sound.
        /// </summary>
        public string TapSound { get; set; }

        /// <summary>
        /// Gets or sets Tap sound shop.
        /// </summary>
        public string TapSoundShop { get; set; }

        /// <summary>
        /// Gets or sets Description t i d.
        /// </summary>
        public string DescriptionTID { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Notification t i d.
        /// </summary>
        public string NotificationTID { get; set; }

        /// <summary>
        /// Gets or sets Spell set.
        /// </summary>
        public string SpellSet { get; set; }

        /// <summary>
        /// Gets or sets Exp.
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// Gets or sets Sort value.
        /// </summary>
        public int SortValue { get; set; }

        /// <summary>
        /// Gets or sets Special offer.
        /// </summary>
        public bool SpecialOffer { get; set; }

        /// <summary>
        /// Gets or sets Draft chest.
        /// </summary>
        public bool DraftChest { get; set; }
    }
}