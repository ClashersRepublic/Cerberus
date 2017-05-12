using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Spells_Heroes : Data
    {
        internal Spells_Heroes(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the spells_heroes.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Icon file.
        /// </summary>
        public string IconFile { get; set; }

        /// <summary>
        /// Gets or sets Unlock arena.
        /// </summary>
        public string UnlockArena { get; set; }

        /// <summary>
        /// Gets or sets Rarity.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Gets or sets Mana cost.
        /// </summary>
        public int ManaCost { get; set; }

        /// <summary>
        /// Gets or sets Mana cost from summoner mana.
        /// </summary>
        public bool ManaCostFromSummonerMana { get; set; }

        /// <summary>
        /// Gets or sets Not in use.
        /// </summary>
        public bool NotInUse { get; set; }

        /// <summary>
        /// Gets or sets Mirror.
        /// </summary>
        public bool Mirror { get; set; }

        /// <summary>
        /// Gets or sets Custom deploy time.
        /// </summary>
        public int CustomDeployTime { get; set; }

        /// <summary>
        /// Gets or sets Summon character.
        /// </summary>
        public string SummonCharacter { get; set; }

        /// <summary>
        /// Gets or sets Summon number.
        /// </summary>
        public int SummonNumber { get; set; }

        /// <summary>
        /// Gets or sets Summon character level index.
        /// </summary>
        public int SummonCharacterLevelIndex { get; set; }

        /// <summary>
        /// Gets or sets Summon character second.
        /// </summary>
        public string SummonCharacterSecond { get; set; }

        /// <summary>
        /// Gets or sets Summon radius.
        /// </summary>
        public int SummonRadius { get; set; }

        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Gets or sets Projectile.
        /// </summary>
        public string Projectile { get; set; }

        /// <summary>
        /// Gets or sets Projectile as deploy.
        /// </summary>
        public bool ProjectileAsDeploy { get; set; }

        /// <summary>
        /// Gets or sets Can place on buildings.
        /// </summary>
        public bool CanPlaceOnBuildings { get; set; }

        /// <summary>
        /// Gets or sets Instant damage.
        /// </summary>
        public int InstantDamage { get; set; }

        /// <summary>
        /// Gets or sets Duration seconds.
        /// </summary>
        public int DurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets Instant heal.
        /// </summary>
        public int InstantHeal { get; set; }

        /// <summary>
        /// Gets or sets Heal per second.
        /// </summary>
        public int HealPerSecond { get; set; }

        /// <summary>
        /// Gets or sets Effect.
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Gets or sets Pushback.
        /// </summary>
        public int Pushback { get; set; }

        /// <summary>
        /// Gets or sets Multiple projectiles.
        /// </summary>
        public int MultipleProjectiles { get; set; }

        /// <summary>
        /// Gets or sets Custom first projectile.
        /// </summary>
        public string CustomFirstProjectile { get; set; }

        /// <summary>
        /// Gets or sets Buff time.
        /// </summary>
        public int BuffTime { get; set; }

        /// <summary>
        /// Gets or sets Buff time increase per level.
        /// </summary>
        public int BuffTimeIncreasePerLevel { get; set; }

        /// <summary>
        /// Gets or sets Buff number.
        /// </summary>
        public int BuffNumber { get; set; }

        /// <summary>
        /// Gets or sets Buff type.
        /// </summary>
        public string BuffType { get; set; }

        /// <summary>
        /// Gets or sets Buff on damage.
        /// </summary>
        public string BuffOnDamage { get; set; }

        /// <summary>
        /// Gets or sets Only own troops.
        /// </summary>
        public bool OnlyOwnTroops { get; set; }

        /// <summary>
        /// Gets or sets Only enemies.
        /// </summary>
        public bool OnlyEnemies { get; set; }

        /// <summary>
        /// Gets or sets Can deploy on enemy side.
        /// </summary>
        public bool CanDeployOnEnemySide { get; set; }

        /// <summary>
        /// Gets or sets Cast sound.
        /// </summary>
        public string CastSound { get; set; }

        /// <summary>
        /// Gets or sets Area effect object.
        /// </summary>
        public string AreaEffectObject { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets T i d  i n f o.
        /// </summary>
        public string TID_INFO { get; set; }

        /// <summary>
        /// Gets or sets Indicator effect.
        /// </summary>
        public string IndicatorEffect { get; set; }

        /// <summary>
        /// Gets or sets Hide radius indicator.
        /// </summary>
        public bool HideRadiusIndicator { get; set; }

        /// <summary>
        /// Gets or sets Dest indicator effect.
        /// </summary>
        public string DestIndicatorEffect { get; set; }

        /// <summary>
        /// Gets or sets Release date.
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets Elixir production stop time.
        /// </summary>
        public int ElixirProductionStopTime { get; set; }

        /// <summary>
        /// Gets or sets Dark mirror.
        /// </summary>
        public bool DarkMirror { get; set; }

        /// <summary>
        /// Gets or sets Stats under info.
        /// </summary>
        public bool StatsUnderInfo { get; set; }
    }
}