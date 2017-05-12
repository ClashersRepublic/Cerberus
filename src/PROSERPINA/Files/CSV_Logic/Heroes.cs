using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Heroes : Data
    {
        internal Heroes(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the heroes.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Rarity.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Gets or sets Sight range.
        /// </summary>
        public int SightRange { get; set; }

        /// <summary>
        /// Gets or sets Deploy time.
        /// </summary>
        public int DeployTime { get; set; }

        /// <summary>
        /// Gets or sets Charge range.
        /// </summary>
        public int ChargeRange { get; set; }

        /// <summary>
        /// Gets or sets Speed.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Gets or sets Hitpoints.
        /// </summary>
        public int Hitpoints { get; set; }

        /// <summary>
        /// Gets or sets Hit speed.
        /// </summary>
        public int HitSpeed { get; set; }

        /// <summary>
        /// Gets or sets Load time.
        /// </summary>
        public int LoadTime { get; set; }

        /// <summary>
        /// Gets or sets Damage.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets Damage special.
        /// </summary>
        public int DamageSpecial { get; set; }

        /// <summary>
        /// Gets or sets Crown tower damage percent.
        /// </summary>
        public int CrownTowerDamagePercent { get; set; }

        /// <summary>
        /// Gets or sets Load first hit.
        /// </summary>
        public bool LoadFirstHit { get; set; }

        /// <summary>
        /// Gets or sets Stop time after attack.
        /// </summary>
        public int StopTimeAfterAttack { get; set; }

        /// <summary>
        /// Gets or sets Stop time after special attack.
        /// </summary>
        public int StopTimeAfterSpecialAttack { get; set; }

        /// <summary>
        /// Gets or sets Projectile.
        /// </summary>
        public string Projectile { get; set; }

        /// <summary>
        /// Gets or sets Custom first projectile.
        /// </summary>
        public string CustomFirstProjectile { get; set; }

        /// <summary>
        /// Gets or sets Multiple projectiles.
        /// </summary>
        public int MultipleProjectiles { get; set; }

        /// <summary>
        /// Gets or sets Multiple targets.
        /// </summary>
        public int MultipleTargets { get; set; }

        /// <summary>
        /// Gets or sets All targets hit.
        /// </summary>
        public bool AllTargetsHit { get; set; }

        /// <summary>
        /// Gets or sets Range.
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        /// Gets or sets Minimum range.
        /// </summary>
        public int MinimumRange { get; set; }

        /// <summary>
        /// Gets or sets Attacks ground.
        /// </summary>
        public bool AttacksGround { get; set; }

        /// <summary>
        /// Gets or sets Attacks air.
        /// </summary>
        public bool AttacksAir { get; set; }

        /// <summary>
        /// Gets or sets Death damage radius.
        /// </summary>
        public int DeathDamageRadius { get; set; }

        /// <summary>
        /// Gets or sets Death damage.
        /// </summary>
        public int DeathDamage { get; set; }

        /// <summary>
        /// Gets or sets Death push back.
        /// </summary>
        public int DeathPushBack { get; set; }

        /// <summary>
        /// Gets or sets Attack push back.
        /// </summary>
        public int AttackPushBack { get; set; }

        /// <summary>
        /// Gets or sets Pushback static dir.
        /// </summary>
        public bool PushbackStaticDir { get; set; }

        /// <summary>
        /// Gets or sets Reload after hits.
        /// </summary>
        public int ReloadAfterHits { get; set; }

        /// <summary>
        /// Gets or sets Reload time.
        /// </summary>
        public int ReloadTime { get; set; }

        /// <summary>
        /// Gets or sets Life time.
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Gets or sets Projectile special.
        /// </summary>
        public string ProjectileSpecial { get; set; }

        /// <summary>
        /// Gets or sets Projectile effect.
        /// </summary>
        public string ProjectileEffect { get; set; }

        /// <summary>
        /// Gets or sets Projectile effect special.
        /// </summary>
        public string ProjectileEffectSpecial { get; set; }

        /// <summary>
        /// Gets or sets Area damage radius.
        /// </summary>
        public int AreaDamageRadius { get; set; }

        /// <summary>
        /// Gets or sets Target only buildings.
        /// </summary>
        public bool TargetOnlyBuildings { get; set; }

        /// <summary>
        /// Gets or sets Special attack interval.
        /// </summary>
        public int SpecialAttackInterval { get; set; }

        /// <summary>
        /// Gets or sets Opponent card health reduction.
        /// </summary>
        public int OpponentCardHealthReduction { get; set; }

        /// <summary>
        /// Gets or sets Own card health reduction.
        /// </summary>
        public int OwnCardHealthReduction { get; set; }

        /// <summary>
        /// Gets or sets Buff on damage.
        /// </summary>
        public string BuffOnDamage { get; set; }

        /// <summary>
        /// Gets or sets Buff on damage time.
        /// </summary>
        public int BuffOnDamageTime { get; set; }

        /// <summary>
        /// Gets or sets Ignore target if immune to buff.
        /// </summary>
        public bool IgnoreTargetIfImmuneToBuff { get; set; }

        /// <summary>
        /// Gets or sets Starting buff.
        /// </summary>
        public string StartingBuff { get; set; }

        /// <summary>
        /// Gets or sets Starting buff time.
        /// </summary>
        public int StartingBuffTime { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Blue export name.
        /// </summary>
        public string BlueExportName { get; set; }

        /// <summary>
        /// Gets or sets Blue top export name.
        /// </summary>
        public string BlueTopExportName { get; set; }

        /// <summary>
        /// Gets or sets Red export name.
        /// </summary>
        public string RedExportName { get; set; }

        /// <summary>
        /// Gets or sets Red top export name.
        /// </summary>
        public string RedTopExportName { get; set; }

        /// <summary>
        /// Gets or sets Use animator.
        /// </summary>
        public bool UseAnimator { get; set; }

        /// <summary>
        /// Gets or sets Attached character.
        /// </summary>
        public string AttachedCharacter { get; set; }

        /// <summary>
        /// Gets or sets Attached character height.
        /// </summary>
        public int AttachedCharacterHeight { get; set; }

        /// <summary>
        /// Gets or sets Damage effect.
        /// </summary>
        public string DamageEffect { get; set; }

        /// <summary>
        /// Gets or sets Damage effect special.
        /// </summary>
        public string DamageEffectSpecial { get; set; }

        /// <summary>
        /// Gets or sets Death effect.
        /// </summary>
        public string DeathEffect { get; set; }

        /// <summary>
        /// Gets or sets Move effect.
        /// </summary>
        public string MoveEffect { get; set; }

        /// <summary>
        /// Gets or sets Spawn effect.
        /// </summary>
        public string SpawnEffect { get; set; }

        /// <summary>
        /// Gets or sets Crowd effects.
        /// </summary>
        public bool CrowdEffects { get; set; }

        /// <summary>
        /// Gets or sets Shadow scale x.
        /// </summary>
        public int ShadowScaleX { get; set; }

        /// <summary>
        /// Gets or sets Shadow scale y.
        /// </summary>
        public int ShadowScaleY { get; set; }

        /// <summary>
        /// Gets or sets Shadow x.
        /// </summary>
        public int ShadowX { get; set; }

        /// <summary>
        /// Gets or sets Shadow y.
        /// </summary>
        public int ShadowY { get; set; }

        /// <summary>
        /// Gets or sets Shadow skew.
        /// </summary>
        public int ShadowSkew { get; set; }

        /// <summary>
        /// Gets or sets Pushback.
        /// </summary>
        public int Pushback { get; set; }

        /// <summary>
        /// Gets or sets Ignore pushback.
        /// </summary>
        public bool IgnorePushback { get; set; }

        /// <summary>
        /// Gets or sets Scale.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// Gets or sets Collision radius.
        /// </summary>
        public int CollisionRadius { get; set; }

        /// <summary>
        /// Gets or sets Mass.
        /// </summary>
        public int Mass { get; set; }

        /// <summary>
        /// Gets or sets Tile size override.
        /// </summary>
        public int TileSizeOverride { get; set; }

        /// <summary>
        /// Gets or sets Area buff.
        /// </summary>
        public string AreaBuff { get; set; }

        /// <summary>
        /// Gets or sets Area buff time.
        /// </summary>
        public int AreaBuffTime { get; set; }

        /// <summary>
        /// Gets or sets Area buff radius.
        /// </summary>
        public int AreaBuffRadius { get; set; }

        /// <summary>
        /// Gets or sets Area buff own troops.
        /// </summary>
        public bool AreaBuffOwnTroops { get; set; }

        /// <summary>
        /// Gets or sets Area buff enemies.
        /// </summary>
        public bool AreaBuffEnemies { get; set; }

        /// <summary>
        /// Gets or sets Gold.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// Gets or sets Mana on death.
        /// </summary>
        public int ManaOnDeath { get; set; }

        /// <summary>
        /// Gets or sets Health bar.
        /// </summary>
        public string HealthBar { get; set; }

        /// <summary>
        /// Gets or sets Health bar offset y.
        /// </summary>
        public int HealthBarOffsetY { get; set; }

        /// <summary>
        /// Gets or sets Show health number.
        /// </summary>
        public bool ShowHealthNumber { get; set; }

        /// <summary>
        /// Gets or sets Flying height.
        /// </summary>
        public int FlyingHeight { get; set; }

        /// <summary>
        /// Gets or sets Fly from ground.
        /// </summary>
        public bool FlyFromGround { get; set; }

        /// <summary>
        /// Gets or sets Damage export name.
        /// </summary>
        public string DamageExportName { get; set; }

        /// <summary>
        /// Gets or sets Grow time.
        /// </summary>
        public int GrowTime { get; set; }

        /// <summary>
        /// Gets or sets Grow size.
        /// </summary>
        public int GrowSize { get; set; }

        /// <summary>
        /// Gets or sets Morph character.
        /// </summary>
        public string MorphCharacter { get; set; }

        /// <summary>
        /// Gets or sets Morph effect.
        /// </summary>
        public string MorphEffect { get; set; }

        /// <summary>
        /// Gets or sets Heal on morph.
        /// </summary>
        public bool HealOnMorph { get; set; }

        /// <summary>
        /// Gets or sets Area effect on morph.
        /// </summary>
        public string AreaEffectOnMorph { get; set; }

        /// <summary>
        /// Gets or sets Attack start effect.
        /// </summary>
        public string AttackStartEffect { get; set; }

        /// <summary>
        /// Gets or sets Attack start effect special.
        /// </summary>
        public string AttackStartEffectSpecial { get; set; }

        /// <summary>
        /// Gets or sets Dash start effect.
        /// </summary>
        public string DashStartEffect { get; set; }

        /// <summary>
        /// Gets or sets Dash effect.
        /// </summary>
        public string DashEffect { get; set; }

        /// <summary>
        /// Gets or sets Dash cooldown.
        /// </summary>
        public int DashCooldown { get; set; }

        /// <summary>
        /// Gets or sets Jump height.
        /// </summary>
        public int JumpHeight { get; set; }

        /// <summary>
        /// Gets or sets Dash push back.
        /// </summary>
        public int DashPushBack { get; set; }

        /// <summary>
        /// Gets or sets Dash radius.
        /// </summary>
        public int DashRadius { get; set; }

        /// <summary>
        /// Gets or sets Dash damage.
        /// </summary>
        public int DashDamage { get; set; }

        /// <summary>
        /// Gets or sets Landing effect.
        /// </summary>
        public string LandingEffect { get; set; }

        /// <summary>
        /// Gets or sets Dash min range.
        /// </summary>
        public int DashMinRange { get; set; }

        /// <summary>
        /// Gets or sets Dash max range.
        /// </summary>
        public int DashMaxRange { get; set; }

        /// <summary>
        /// Gets or sets Jump speed.
        /// </summary>
        public int JumpSpeed { get; set; }

        /// <summary>
        /// Gets or sets Continuous effect.
        /// </summary>
        public string ContinuousEffect { get; set; }

        /// <summary>
        /// Gets or sets Opponent card spawn.
        /// </summary>
        public int OpponentCardSpawn { get; set; }

        /// <summary>
        /// Gets or sets Own card spawn.
        /// </summary>
        public int OwnCardSpawn { get; set; }

        /// <summary>
        /// Gets or sets Spawn start time.
        /// </summary>
        public int SpawnStartTime { get; set; }

        /// <summary>
        /// Gets or sets Spawn interval.
        /// </summary>
        public int SpawnInterval { get; set; }

        /// <summary>
        /// Gets or sets Spawn number.
        /// </summary>
        public int SpawnNumber { get; set; }

        /// <summary>
        /// Gets or sets Spawn limit.
        /// </summary>
        public int SpawnLimit { get; set; }

        /// <summary>
        /// Gets or sets Spawn pause time.
        /// </summary>
        public int SpawnPauseTime { get; set; }

        /// <summary>
        /// Gets or sets Spawn character level index.
        /// </summary>
        public int SpawnCharacterLevelIndex { get; set; }

        /// <summary>
        /// Gets or sets Spawn character.
        /// </summary>
        public string SpawnCharacter { get; set; }

        /// <summary>
        /// Gets or sets Spawn character effect.
        /// </summary>
        public string SpawnCharacterEffect { get; set; }

        /// <summary>
        /// Gets or sets Spawn deploy base anim.
        /// </summary>
        public string SpawnDeployBaseAnim { get; set; }

        /// <summary>
        /// Gets or sets Spawn radius.
        /// </summary>
        public int SpawnRadius { get; set; }

        /// <summary>
        /// Gets or sets Death spawn count.
        /// </summary>
        public int DeathSpawnCount { get; set; }

        /// <summary>
        /// Gets or sets Death spawn character.
        /// </summary>
        public string DeathSpawnCharacter { get; set; }

        /// <summary>
        /// Gets or sets Death spawn radius.
        /// </summary>
        public int DeathSpawnRadius { get; set; }

        /// <summary>
        /// Gets or sets Death spawn angle shift.
        /// </summary>
        public int DeathSpawnAngleShift { get; set; }

        /// <summary>
        /// Gets or sets Death spawn deploy time.
        /// </summary>
        public int DeathSpawnDeployTime { get; set; }

        /// <summary>
        /// Gets or sets Death spawn pushback.
        /// </summary>
        public bool DeathSpawnPushback { get; set; }

        /// <summary>
        /// Gets or sets Death area effect.
        /// </summary>
        public string DeathAreaEffect { get; set; }

        /// <summary>
        /// Gets or sets Kamikaze.
        /// </summary>
        public bool Kamikaze { get; set; }

        /// <summary>
        /// Gets or sets Kamikaze effect.
        /// </summary>
        public string KamikazeEffect { get; set; }

        /// <summary>
        /// Gets or sets Spawn pathfind speed.
        /// </summary>
        public int SpawnPathfindSpeed { get; set; }

        /// <summary>
        /// Gets or sets Spawn pathfind effect.
        /// </summary>
        public string SpawnPathfindEffect { get; set; }

        /// <summary>
        /// Gets or sets Spawn pathfind morph.
        /// </summary>
        public string SpawnPathfindMorph { get; set; }

        /// <summary>
        /// Gets or sets Spawn pushback.
        /// </summary>
        public int SpawnPushback { get; set; }

        /// <summary>
        /// Gets or sets Spawn pushback radius.
        /// </summary>
        public int SpawnPushbackRadius { get; set; }

        /// <summary>
        /// Gets or sets Spawn area object.
        /// </summary>
        public string SpawnAreaObject { get; set; }

        /// <summary>
        /// Gets or sets Spawn area object level index.
        /// </summary>
        public int SpawnAreaObjectLevelIndex { get; set; }

        /// <summary>
        /// Gets or sets Charge effect.
        /// </summary>
        public string ChargeEffect { get; set; }

        /// <summary>
        /// Gets or sets Take damage effect.
        /// </summary>
        public string TakeDamageEffect { get; set; }

        /// <summary>
        /// Gets or sets Projectile start radius.
        /// </summary>
        public int ProjectileStartRadius { get; set; }

        /// <summary>
        /// Gets or sets Projectile start z.
        /// </summary>
        public int ProjectileStartZ { get; set; }

        /// <summary>
        /// Gets or sets Stop movement after m s.
        /// </summary>
        public int StopMovementAfterMS { get; set; }

        /// <summary>
        /// Gets or sets Wait m s.
        /// </summary>
        public int WaitMS { get; set; }

        /// <summary>
        /// Gets or sets Dont stop move anim.
        /// </summary>
        public bool DontStopMoveAnim { get; set; }

        /// <summary>
        /// Gets or sets Is summoner tower.
        /// </summary>
        public bool IsSummonerTower { get; set; }

        /// <summary>
        /// Gets or sets No deploy size w.
        /// </summary>
        public int NoDeploySizeW { get; set; }

        /// <summary>
        /// Gets or sets No deploy size h.
        /// </summary>
        public int NoDeploySizeH { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Variable damage2.
        /// </summary>
        public int VariableDamage2 { get; set; }

        /// <summary>
        /// Gets or sets Variable damage time1.
        /// </summary>
        public int VariableDamageTime1 { get; set; }

        /// <summary>
        /// Gets or sets Variable damage3.
        /// </summary>
        public int VariableDamage3 { get; set; }

        /// <summary>
        /// Gets or sets Variable damage time2.
        /// </summary>
        public int VariableDamageTime2 { get; set; }

        /// <summary>
        /// Gets or sets Targetted damage effect1.
        /// </summary>
        public string TargettedDamageEffect1 { get; set; }

        /// <summary>
        /// Gets or sets Targetted damage effect2.
        /// </summary>
        public string TargettedDamageEffect2 { get; set; }

        /// <summary>
        /// Gets or sets Targetted damage effect3.
        /// </summary>
        public string TargettedDamageEffect3 { get; set; }

        /// <summary>
        /// Gets or sets Damage level transition effect12.
        /// </summary>
        public string DamageLevelTransitionEffect12 { get; set; }

        /// <summary>
        /// Gets or sets Damage level transition effect23.
        /// </summary>
        public string DamageLevelTransitionEffect23 { get; set; }

        /// <summary>
        /// Gets or sets Flame effect1.
        /// </summary>
        public string FlameEffect1 { get; set; }

        /// <summary>
        /// Gets or sets Flame effect2.
        /// </summary>
        public string FlameEffect2 { get; set; }

        /// <summary>
        /// Gets or sets Flame effect3.
        /// </summary>
        public string FlameEffect3 { get; set; }

        /// <summary>
        /// Gets or sets Target effect y.
        /// </summary>
        public int TargetEffectY { get; set; }

        /// <summary>
        /// Gets or sets Self as aoe center.
        /// </summary>
        public bool SelfAsAoeCenter { get; set; }

        /// <summary>
        /// Gets or sets Hides when not attacking.
        /// </summary>
        public bool HidesWhenNotAttacking { get; set; }

        /// <summary>
        /// Gets or sets Hide time ms.
        /// </summary>
        public int HideTimeMs { get; set; }

        /// <summary>
        /// Gets or sets Hide before first hit.
        /// </summary>
        public bool HideBeforeFirstHit { get; set; }

        /// <summary>
        /// Gets or sets Special attack when hidden.
        /// </summary>
        public bool SpecialAttackWhenHidden { get; set; }

        /// <summary>
        /// Gets or sets Targeted hit effect.
        /// </summary>
        public string TargetedHitEffect { get; set; }

        /// <summary>
        /// Gets or sets Targeted hit effect special.
        /// </summary>
        public string TargetedHitEffectSpecial { get; set; }

        /// <summary>
        /// Gets or sets Up time ms.
        /// </summary>
        public int UpTimeMs { get; set; }

        /// <summary>
        /// Gets or sets Hide effect.
        /// </summary>
        public string HideEffect { get; set; }

        /// <summary>
        /// Gets or sets Appear effect.
        /// </summary>
        public string AppearEffect { get; set; }

        /// <summary>
        /// Gets or sets Appear pushback radius.
        /// </summary>
        public int AppearPushbackRadius { get; set; }

        /// <summary>
        /// Gets or sets Appear pushback.
        /// </summary>
        public int AppearPushback { get; set; }

        /// <summary>
        /// Gets or sets Appear area object.
        /// </summary>
        public string AppearAreaObject { get; set; }

        /// <summary>
        /// Gets or sets Mana collect amount.
        /// </summary>
        public int ManaCollectAmount { get; set; }

        /// <summary>
        /// Gets or sets Mana generate time ms.
        /// </summary>
        public int ManaGenerateTimeMs { get; set; }

        /// <summary>
        /// Gets or sets Mana generate limit.
        /// </summary>
        public int ManaGenerateLimit { get; set; }

        /// <summary>
        /// Gets or sets Has rotation on timeline.
        /// </summary>
        public bool HasRotationOnTimeline { get; set; }

        /// <summary>
        /// Gets or sets Turret movement.
        /// </summary>
        public int TurretMovement { get; set; }

        /// <summary>
        /// Gets or sets Projectile y offset.
        /// </summary>
        public int ProjectileYOffset { get; set; }

        /// <summary>
        /// Gets or sets Charge speed multiplier.
        /// </summary>
        public int ChargeSpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets Deploy delay.
        /// </summary>
        public int DeployDelay { get; set; }

        /// <summary>
        /// Gets or sets Deploy base anim export name.
        /// </summary>
        public string DeployBaseAnimExportName { get; set; }

        /// <summary>
        /// Gets or sets Jump enabled.
        /// </summary>
        public bool JumpEnabled { get; set; }

        /// <summary>
        /// Gets or sets Sight clip.
        /// </summary>
        public int SightClip { get; set; }

        /// <summary>
        /// Gets or sets Area effect on dash.
        /// </summary>
        public string AreaEffectOnDash { get; set; }

        /// <summary>
        /// Gets or sets Sight clip side.
        /// </summary>
        public int SightClipSide { get; set; }

        /// <summary>
        /// Gets or sets Walking speed tweak percentage.
        /// </summary>
        public int WalkingSpeedTweakPercentage { get; set; }

        /// <summary>
        /// Gets or sets Shield hitpoints.
        /// </summary>
        public int ShieldHitpoints { get; set; }

        /// <summary>
        /// Gets or sets Shield die pushback.
        /// </summary>
        public int ShieldDiePushback { get; set; }

        /// <summary>
        /// Gets or sets Shield lost effect.
        /// </summary>
        public string ShieldLostEffect { get; set; }

        /// <summary>
        /// Gets or sets Blue shield export name.
        /// </summary>
        public string BlueShieldExportName { get; set; }

        /// <summary>
        /// Gets or sets Red shield export name.
        /// </summary>
        public string RedShieldExportName { get; set; }

        /// <summary>
        /// Gets or sets Load attack effect1.
        /// </summary>
        public string LoadAttackEffect1 { get; set; }

        /// <summary>
        /// Gets or sets Load attack effect2.
        /// </summary>
        public string LoadAttackEffect2 { get; set; }

        /// <summary>
        /// Gets or sets Load attack effect3.
        /// </summary>
        public string LoadAttackEffect3 { get; set; }

        /// <summary>
        /// Gets or sets Load attack effect ready.
        /// </summary>
        public string LoadAttackEffectReady { get; set; }

        /// <summary>
        /// Gets or sets Rotate angle speed.
        /// </summary>
        public int RotateAngleSpeed { get; set; }

        /// <summary>
        /// Gets or sets Enable attack on damage.
        /// </summary>
        public bool EnableAttackOnDamage { get; set; }

        /// <summary>
        /// Gets or sets Secondary hit delay.
        /// </summary>
        public int SecondaryHitDelay { get; set; }

        /// <summary>
        /// Gets or sets Deploy timer delay.
        /// </summary>
        public int DeployTimerDelay { get; set; }

        /// <summary>
        /// Gets or sets Retarget after attack.
        /// </summary>
        public bool RetargetAfterAttack { get; set; }

        /// <summary>
        /// Gets or sets Attack shake time.
        /// </summary>
        public int AttackShakeTime { get; set; }

        /// <summary>
        /// Gets or sets Visual hit speed.
        /// </summary>
        public int VisualHitSpeed { get; set; }

        /// <summary>
        /// Gets or sets Ability.
        /// </summary>
        public string Ability { get; set; }

        /// <summary>
        /// Gets or sets Burst.
        /// </summary>
        public int Burst { get; set; }

        /// <summary>
        /// Gets or sets Burst delay.
        /// </summary>
        public int BurstDelay { get; set; }

        /// <summary>
        /// Gets or sets Burst keep target.
        /// </summary>
        public bool BurstKeepTarget { get; set; }

        /// <summary>
        /// Gets or sets Activation time.
        /// </summary>
        public int ActivationTime { get; set; }
    }
}