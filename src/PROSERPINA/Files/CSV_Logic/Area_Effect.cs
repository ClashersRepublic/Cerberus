using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Area_Effect : Data
    {
        internal Area_Effect(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
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
        /// Gets or sets Life duration.
        /// </summary>
        public int LifeDuration { get; set; }

        /// <summary>
        /// Gets or sets Life duration increase per level.
        /// </summary>
        public int LifeDurationIncreasePerLevel { get; set; }

        /// <summary>
        /// Gets or sets Life duration increase after tournament cap.
        /// </summary>
        public int LifeDurationIncreaseAfterTournamentCap { get; set; }

        /// <summary>
        /// Gets or sets Affects hidden.
        /// </summary>
        public bool AffectsHidden { get; set; }

        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Gets or sets Looping effect.
        /// </summary>
        public string LoopingEffect { get; set; }

        /// <summary>
        /// Gets or sets One shot effect.
        /// </summary>
        public string OneShotEffect { get; set; }

        /// <summary>
        /// Gets or sets Scaled effect.
        /// </summary>
        public string ScaledEffect { get; set; }

        /// <summary>
        /// Gets or sets Hit effect.
        /// </summary>
        public string HitEffect { get; set; }

        /// <summary>
        /// Gets or sets Pushback.
        /// </summary>
        public int Pushback { get; set; }

        /// <summary>
        /// Gets or sets Maximum targets.
        /// </summary>
        public int MaximumTargets { get; set; }

        /// <summary>
        /// Gets or sets Hit speed.
        /// </summary>
        public int HitSpeed { get; set; }

        /// <summary>
        /// Gets or sets Damage.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets No effect to crown towers.
        /// </summary>
        public bool NoEffectToCrownTowers { get; set; }

        /// <summary>
        /// Gets or sets Crown tower damage percent.
        /// </summary>
        public int CrownTowerDamagePercent { get; set; }

        /// <summary>
        /// Gets or sets Hit biggest targets.
        /// </summary>
        public bool HitBiggestTargets { get; set; }

        /// <summary>
        /// Gets or sets Buff.
        /// </summary>
        public string Buff { get; set; }

        /// <summary>
        /// Gets or sets Buff time.
        /// </summary>
        public int BuffTime { get; set; }

        /// <summary>
        /// Gets or sets Buff time increase per level.
        /// </summary>
        public int BuffTimeIncreasePerLevel { get; set; }

        /// <summary>
        /// Gets or sets Buff time increase after tournament cap.
        /// </summary>
        public int BuffTimeIncreaseAfterTournamentCap { get; set; }

        /// <summary>
        /// Gets or sets Cap buff time to area effect time.
        /// </summary>
        public bool CapBuffTimeToAreaEffectTime { get; set; }

        /// <summary>
        /// Gets or sets Buff number.
        /// </summary>
        public int BuffNumber { get; set; }

        /// <summary>
        /// Gets or sets Only enemies.
        /// </summary>
        public bool OnlyEnemies { get; set; }

        /// <summary>
        /// Gets or sets Only own troops.
        /// </summary>
        public bool OnlyOwnTroops { get; set; }

        /// <summary>
        /// Gets or sets Ignore buildings.
        /// </summary>
        public bool IgnoreBuildings { get; set; }

        /// <summary>
        /// Gets or sets Projectile.
        /// </summary>
        public string Projectile { get; set; }

        /// <summary>
        /// Gets or sets Spawn character.
        /// </summary>
        public string SpawnCharacter { get; set; }

        /// <summary>
        /// Gets or sets Spawn interval.
        /// </summary>
        public int SpawnInterval { get; set; }

        /// <summary>
        /// Gets or sets Spawn effect.
        /// </summary>
        public string SpawnEffect { get; set; }

        /// <summary>
        /// Gets or sets Spawn deploy base anim.
        /// </summary>
        public string SpawnDeployBaseAnim { get; set; }

        /// <summary>
        /// Gets or sets Spawn time.
        /// </summary>
        public int SpawnTime { get; set; }

        /// <summary>
        /// Gets or sets Spawn character level index.
        /// </summary>
        public int SpawnCharacterLevelIndex { get; set; }

        /// <summary>
        /// Gets or sets Spawn initial delay.
        /// </summary>
        public int SpawnInitialDelay { get; set; }

        /// <summary>
        /// Gets or sets Spawn max count.
        /// </summary>
        public int SpawnMaxCount { get; set; }

        /// <summary>
        /// Gets or sets Hits ground.
        /// </summary>
        public bool HitsGround { get; set; }

        /// <summary>
        /// Gets or sets Hits air.
        /// </summary>
        public bool HitsAir { get; set; }

        /// <summary>
        /// Gets or sets Projectile start height.
        /// </summary>
        public int ProjectileStartHeight { get; set; }

        /// <summary>
        /// Gets or sets Projectiles to center.
        /// </summary>
        public bool ProjectilesToCenter { get; set; }

        /// <summary>
        /// Gets or sets Spawns a e o.
        /// </summary>
        public string SpawnsAEO { get; set; }

        /// <summary>
        /// Gets or sets Controls buff.
        /// </summary>
        public bool ControlsBuff { get; set; }

        /// <summary>
        /// Gets or sets Clone.
        /// </summary>
        public bool Clone { get; set; }

        /// <summary>
        /// Gets or sets Attract percentage.
        /// </summary>
        public int AttractPercentage { get; set; }

        /// <summary>
        /// Gets or sets Transport.
        /// </summary>
        public bool Transport { get; set; }
    }
}