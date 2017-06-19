using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Shop : Data
    {
        internal Shop(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the shop.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Rarity.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Gets or sets Disabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets Resource.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets Cost.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets Count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets Cycle duration.
        /// </summary>
        public int CycleDuration { get; set; }

        /// <summary>
        /// Gets or sets Cycle deadzone start.
        /// </summary>
        public int CycleDeadzoneStart { get; set; }

        /// <summary>
        /// Gets or sets Cycle deadzone end.
        /// </summary>
        public int CycleDeadzoneEnd { get; set; }

        /// <summary>
        /// Gets or sets Top section.
        /// </summary>
        public bool TopSection { get; set; }

        /// <summary>
        /// Gets or sets Special offer.
        /// </summary>
        public bool SpecialOffer { get; set; }

        /// <summary>
        /// Gets or sets Duration secs.
        /// </summary>
        public int DurationSecs { get; set; }

        /// <summary>
        /// Gets or sets Availability secs.
        /// </summary>
        public int AvailabilitySecs { get; set; }

        /// <summary>
        /// Gets or sets Sync to shop cycle.
        /// </summary>
        public bool SyncToShopCycle { get; set; }

        /// <summary>
        /// Gets or sets Chest.
        /// </summary>
        public string Chest { get; set; }

        /// <summary>
        /// Gets or sets Trophy limit.
        /// </summary>
        public int TrophyLimit { get; set; }

        /// <summary>
        /// Gets or sets I a p.
        /// </summary>
        public string IAP { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item0  type.
        /// </summary>
        public string StarterPack_Item0_Type { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item0  i d.
        /// </summary>
        public string StarterPack_Item0_ID { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item0  param1.
        /// </summary>
        public int StarterPack_Item0_Param1 { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item1  type.
        /// </summary>
        public string StarterPack_Item1_Type { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item1  i d.
        /// </summary>
        public string StarterPack_Item1_ID { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item1  param1.
        /// </summary>
        public int StarterPack_Item1_Param1 { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item2  type.
        /// </summary>
        public string StarterPack_Item2_Type { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item2  i d.
        /// </summary>
        public string StarterPack_Item2_ID { get; set; }

        /// <summary>
        /// Gets or sets Starter pack  item2  param1.
        /// </summary>
        public int StarterPack_Item2_Param1 { get; set; }

        /// <summary>
        /// Gets or sets Value multiplier.
        /// </summary>
        public int ValueMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Append arena to chest name.
        /// </summary>
        public bool AppendArenaToChestName { get; set; }

        /// <summary>
        /// Gets or sets Tied to arena unlock.
        /// </summary>
        public string TiedToArenaUnlock { get; set; }

        /// <summary>
        /// Gets or sets Repeat purchase gem pack override.
        /// </summary>
        public string RepeatPurchaseGemPackOverride { get; set; }

        /// <summary>
        /// Gets or sets Event name.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets Cost adjust based on chest contents.
        /// </summary>
        public bool CostAdjustBasedOnChestContents { get; set; }

        /// <summary>
        /// Gets or sets Is chronos offer.
        /// </summary>
        public bool IsChronosOffer { get; set; }
    }
}