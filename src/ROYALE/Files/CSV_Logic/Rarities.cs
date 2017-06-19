using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Rarities : Data
    {
        internal Rarities(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the rarities.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Level count.
        /// </summary>
        public int LevelCount { get; set; }

        /// <summary>
        /// Gets or sets Relative level.
        /// </summary>
        public int RelativeLevel { get; set; }

        /// <summary>
        /// Gets or sets Mirror relative level.
        /// </summary>
        public int MirrorRelativeLevel { get; set; }

        /// <summary>
        /// Gets or sets Clone relative level.
        /// </summary>
        public int CloneRelativeLevel { get; set; }

        /// <summary>
        /// Gets or sets Donate capacity.
        /// </summary>
        public int DonateCapacity { get; set; }

        /// <summary>
        /// Gets or sets Sort capacity.
        /// </summary>
        public int SortCapacity { get; set; }

        /// <summary>
        /// Gets or sets Donate reward.
        /// </summary>
        public int DonateReward { get; set; }

        /// <summary>
        /// Gets or sets Donate x p.
        /// </summary>
        public int DonateXP { get; set; }

        /// <summary>
        /// Gets or sets Gold conversion value.
        /// </summary>
        public int GoldConversionValue { get; set; }

        /// <summary>
        /// Gets or sets Chance weight.
        /// </summary>
        public int ChanceWeight { get; set; }

        /// <summary>
        /// Gets or sets Balance multiplier.
        /// </summary>
        public int BalanceMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Upgrade exp.
        /// </summary>
        public int UpgradeExp { get; set; }

        /// <summary>
        /// Gets or sets Upgrade material count.
        /// </summary>
        public int[] UpgradeMaterialCount { get; set; }

        /// <summary>
        /// Gets or sets Upgrade cost.
        /// </summary>
        public int UpgradeCost { get; set; }

        /// <summary>
        /// Gets or sets Power level multiplier.
        /// </summary>
        public int PowerLevelMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Refund gems.
        /// </summary>
        public int RefundGems { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Card base file name.
        /// </summary>
        public string CardBaseFileName { get; set; }

        /// <summary>
        /// Gets or sets Big frame export name.
        /// </summary>
        public string BigFrameExportName { get; set; }

        /// <summary>
        /// Gets or sets Card base export name.
        /// </summary>
        public string CardBaseExportName { get; set; }

        /// <summary>
        /// Gets or sets Stacked card export name.
        /// </summary>
        public string StackedCardExportName { get; set; }

        /// <summary>
        /// Gets or sets Card reward export name.
        /// </summary>
        public string CardRewardExportName { get; set; }

        /// <summary>
        /// Gets or sets Cast effect.
        /// </summary>
        public string CastEffect { get; set; }

        /// <summary>
        /// Gets or sets Info title export name.
        /// </summary>
        public string InfoTitleExportName { get; set; }

        /// <summary>
        /// Gets or sets Card rarity b g export name.
        /// </summary>
        public string CardRarityBGExportName { get; set; }

        /// <summary>
        /// Gets or sets Sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets Red.
        /// </summary>
        public int Red { get; set; }

        /// <summary>
        /// Gets or sets Green.
        /// </summary>
        public int Green { get; set; }

        /// <summary>
        /// Gets or sets Blue.
        /// </summary>
        public int Blue { get; set; }

        /// <summary>
        /// Gets or sets Appear effect.
        /// </summary>
        public string AppearEffect { get; set; }

        /// <summary>
        /// Gets or sets Buy sound.
        /// </summary>
        public string BuySound { get; set; }

        /// <summary>
        /// Gets or sets Loop effect.
        /// </summary>
        public string LoopEffect { get; set; }

        /// <summary>
        /// Gets or sets Card txt bg frame idx.
        /// </summary>
        public int CardTxtBgFrameIdx { get; set; }

        /// <summary>
        /// Gets or sets Card glow instance name.
        /// </summary>
        public string CardGlowInstanceName { get; set; }

        /// <summary>
        /// Gets or sets Spell selected sound.
        /// </summary>
        public string SpellSelectedSound { get; set; }

        /// <summary>
        /// Gets or sets Spell available sound.
        /// </summary>
        public string SpellAvailableSound { get; set; }

        /// <summary>
        /// Gets or sets Rotate export name.
        /// </summary>
        public string RotateExportName { get; set; }
    }
}