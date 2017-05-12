using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Projectiles : Data
    {
        internal Projectiles(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the projectiles.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Rarity.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Gets or sets Speed.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }

        /// <summary>
        /// Gets or sets Red export name.
        /// </summary>
        public string RedExportName { get; set; }

        /// <summary>
        /// Gets or sets Shadow export name.
        /// </summary>
        public string ShadowExportName { get; set; }

        /// <summary>
        /// Gets or sets Red shadow export name.
        /// </summary>
        public string RedShadowExportName { get; set; }

        /// <summary>
        /// Gets or sets Shadow disable rotate.
        /// </summary>
        public bool ShadowDisableRotate { get; set; }

        /// <summary>
        /// Gets or sets Scale.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// Gets or sets Homing.
        /// </summary>
        public bool Homing { get; set; }

        /// <summary>
        /// Gets or sets Hit effect.
        /// </summary>
        public string HitEffect { get; set; }

        /// <summary>
        /// Gets or sets Death effect.
        /// </summary>
        public string DeathEffect { get; set; }

        /// <summary>
        /// Gets or sets Damage.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets Crown tower damage percent.
        /// </summary>
        public int CrownTowerDamagePercent { get; set; }

        /// <summary>
        /// Gets or sets Pushback.
        /// </summary>
        public int Pushback { get; set; }

        /// <summary>
        /// Gets or sets Pushback all.
        /// </summary>
        public bool PushbackAll { get; set; }

        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Gets or sets Radius y.
        /// </summary>
        public int RadiusY { get; set; }

        /// <summary>
        /// Gets or sets Aoe to air.
        /// </summary>
        public bool AoeToAir { get; set; }

        /// <summary>
        /// Gets or sets Aoe to ground.
        /// </summary>
        public bool AoeToGround { get; set; }

        /// <summary>
        /// Gets or sets Only enemies.
        /// </summary>
        public bool OnlyEnemies { get; set; }

        /// <summary>
        /// Gets or sets Only own troops.
        /// </summary>
        public bool OnlyOwnTroops { get; set; }

        /// <summary>
        /// Gets or sets Maximum targets.
        /// </summary>
        public int MaximumTargets { get; set; }

        /// <summary>
        /// Gets or sets Gravity.
        /// </summary>
        public int Gravity { get; set; }

        /// <summary>
        /// Gets or sets Spawn area effect object.
        /// </summary>
        public string SpawnAreaEffectObject { get; set; }

        /// <summary>
        /// Gets or sets Spawn character level index.
        /// </summary>
        public int SpawnCharacterLevelIndex { get; set; }

        /// <summary>
        /// Gets or sets Spawn character deploy time.
        /// </summary>
        public int SpawnCharacterDeployTime { get; set; }

        /// <summary>
        /// Gets or sets Spawn character.
        /// </summary>
        public string SpawnCharacter { get; set; }

        /// <summary>
        /// Gets or sets Spawn const priority.
        /// </summary>
        public bool SpawnConstPriority { get; set; }

        /// <summary>
        /// Gets or sets Spawn character count.
        /// </summary>
        public int SpawnCharacterCount { get; set; }

        /// <summary>
        /// Gets or sets Target buff.
        /// </summary>
        public string TargetBuff { get; set; }

        /// <summary>
        /// Gets or sets Buff time.
        /// </summary>
        public int BuffTime { get; set; }

        /// <summary>
        /// Gets or sets Buff time increase per level.
        /// </summary>
        public int BuffTimeIncreasePerLevel { get; set; }

        /// <summary>
        /// Gets or sets Trail effect.
        /// </summary>
        public string TrailEffect { get; set; }

        /// <summary>
        /// Gets or sets Projectile radius.
        /// </summary>
        public int ProjectileRadius { get; set; }

        /// <summary>
        /// Gets or sets Projectile radius y.
        /// </summary>
        public int ProjectileRadiusY { get; set; }

        /// <summary>
        /// Gets or sets Projectile range.
        /// </summary>
        public int ProjectileRange { get; set; }

        /// <summary>
        /// Gets or sets use360 frames.
        /// </summary>
        public bool use360Frames { get; set; }

        /// <summary>
        /// Gets or sets Hit sound when parent alive.
        /// </summary>
        public string HitSoundWhenParentAlive { get; set; }

        /// <summary>
        /// Gets or sets Spawn projectile.
        /// </summary>
        public string SpawnProjectile { get; set; }

        /// <summary>
        /// Gets or sets Min distance.
        /// </summary>
        public int MinDistance { get; set; }

        /// <summary>
        /// Gets or sets Max distance.
        /// </summary>
        public int MaxDistance { get; set; }

        /// <summary>
        /// Gets or sets Constant height.
        /// </summary>
        public int ConstantHeight { get; set; }

        /// <summary>
        /// Gets or sets Height from target radius.
        /// </summary>
        public bool HeightFromTargetRadius { get; set; }

        /// <summary>
        /// Gets or sets Heal.
        /// </summary>
        public int Heal { get; set; }

        /// <summary>
        /// Gets or sets Crown tower heal percent.
        /// </summary>
        public int CrownTowerHealPercent { get; set; }

        /// <summary>
        /// Gets or sets Target to edge.
        /// </summary>
        public bool TargetToEdge { get; set; }

        /// <summary>
        /// Gets or sets Chained hit radius.
        /// </summary>
        public int ChainedHitRadius { get; set; }

        /// <summary>
        /// Gets or sets Chained hit end effect.
        /// </summary>
        public string ChainedHitEndEffect { get; set; }

        /// <summary>
        /// Gets or sets Pingpong death effect.
        /// </summary>
        public string PingpongDeathEffect { get; set; }

        /// <summary>
        /// Gets or sets Shakes targets.
        /// </summary>
        public bool ShakesTargets { get; set; }

        /// <summary>
        /// Gets or sets Pingpong visual time.
        /// </summary>
        public int PingpongVisualTime { get; set; }

        /// <summary>
        /// Gets or sets Random angle.
        /// </summary>
        public int RandomAngle { get; set; }

        /// <summary>
        /// Gets or sets Random distance.
        /// </summary>
        public int RandomDistance { get; set; }

        /// <summary>
        /// Gets or sets Scatter.
        /// </summary>
        public string Scatter { get; set; }
    }
}