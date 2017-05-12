using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Character_Buffs : Data
    {
        internal Character_Buffs(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Rarity.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Icon file name.
        /// </summary>
        public string IconFileName { get; set; }

        /// <summary>
        /// Gets or sets Icon export name.
        /// </summary>
        public string IconExportName { get; set; }

        /// <summary>
        /// Gets or sets Change control.
        /// </summary>
        public bool ChangeControl { get; set; }

        /// <summary>
        /// Gets or sets No effect to crown towers.
        /// </summary>
        public bool NoEffectToCrownTowers { get; set; }

        /// <summary>
        /// Gets or sets Crown tower damage percent.
        /// </summary>
        public int CrownTowerDamagePercent { get; set; }

        /// <summary>
        /// Gets or sets Damage per second.
        /// </summary>
        public int DamagePerSecond { get; set; }

        /// <summary>
        /// Gets or sets Hit frequency.
        /// </summary>
        public int HitFrequency { get; set; }

        /// <summary>
        /// Gets or sets Damage reduction.
        /// </summary>
        public int DamageReduction { get; set; }

        /// <summary>
        /// Gets or sets Heal per second.
        /// </summary>
        public int HealPerSecond { get; set; }

        /// <summary>
        /// Gets or sets Immune to anti magic.
        /// </summary>
        public bool ImmuneToAntiMagic { get; set; }

        /// <summary>
        /// Gets or sets Hit speed multiplier.
        /// </summary>
        public int HitSpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Speed multiplier.
        /// </summary>
        public int SpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Spawn speed multiplier.
        /// </summary>
        public int SpawnSpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Negates buffs.
        /// </summary>
        public string NegatesBuffs { get; set; }

        /// <summary>
        /// Gets or sets Immunity to buffs.
        /// </summary>
        public string ImmunityToBuffs { get; set; }

        /// <summary>
        /// Gets or sets Invisible.
        /// </summary>
        public bool Invisible { get; set; }

        /// <summary>
        /// Gets or sets Remove on attack.
        /// </summary>
        public bool RemoveOnAttack { get; set; }

        /// <summary>
        /// Gets or sets Remove on heal.
        /// </summary>
        public bool RemoveOnHeal { get; set; }

        /// <summary>
        /// Gets or sets Damage multiplier.
        /// </summary>
        public int DamageMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Panic.
        /// </summary>
        public bool Panic { get; set; }

        /// <summary>
        /// Gets or sets Effect.
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Gets or sets Filter file.
        /// </summary>
        public string FilterFile { get; set; }

        /// <summary>
        /// Gets or sets Filter export name.
        /// </summary>
        public string FilterExportName { get; set; }

        /// <summary>
        /// Gets or sets Filter affects transformation.
        /// </summary>
        public bool FilterAffectsTransformation { get; set; }

        /// <summary>
        /// Gets or sets Filter inherit life duration.
        /// </summary>
        public bool FilterInheritLifeDuration { get; set; }

        /// <summary>
        /// Gets or sets Size multiplier.
        /// </summary>
        public int SizeMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Static target.
        /// </summary>
        public bool StaticTarget { get; set; }

        /// <summary>
        /// Gets or sets Ignore push back.
        /// </summary>
        public bool IgnorePushBack { get; set; }

        /// <summary>
        /// Gets or sets Mark effect.
        /// </summary>
        public string MarkEffect { get; set; }

        /// <summary>
        /// Gets or sets Audio pitch modifier.
        /// </summary>
        public int AudioPitchModifier { get; set; }

        /// <summary>
        /// Gets or sets Portal spell.
        /// </summary>
        public string PortalSpell { get; set; }

        /// <summary>
        /// Gets or sets Attract percentage.
        /// </summary>
        public int AttractPercentage { get; set; }

        /// <summary>
        /// Gets or sets Controlled by parent.
        /// </summary>
        public bool ControlledByParent { get; set; }

        /// <summary>
        /// Gets or sets Clone.
        /// </summary>
        public bool Clone { get; set; }

        /// <summary>
        /// Gets or sets Scale.
        /// </summary>
        public int Scale { get; set; }
    }
}