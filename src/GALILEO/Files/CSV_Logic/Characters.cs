using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Reader;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Files.CSV_Logic
{
    internal class Characters : Combat_Item
    {
        public Characters(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public int HousingSpace { get; set; }
        public int BarrackLevel { get; set; }
        public int[] LaboratoryLevel { get; set; }
        public int Speed { get; set; }
        public int[] Hitpoints { get; set; }
        public int TrainingTime { get; set; }
        public string TrainingResource { get; set; }
        public int[] TrainingCost { get; set; }
        public int[] UpgradeTimeH { get; set; }
        public string UpgradeResource { get; set; }
        public int[] UpgradeCost { get; set; }
        public int DonateCost { get; set; }
        public int[] AttackRange { get; set; }
        public int[] AttackSpeed { get; set; }
        public int[] DPS { get; set; }
        public int PreferedTargetDamageMod { get; set; }
        public int[] DamageRadius { get; set; }
        public bool AreaDamageIgnoresWalls { get; set; }
        public bool SelfAsAoeCenter { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string BigPicture { get; set; }
        public string BigPictureSWF { get; set; }
        public string[] Projectile { get; set; }
        public string PreferedTargetBuilding { get; set; }
        public string PreferedTargetBuildingClass { get; set; }
        public string DeployEffect { get; set; }
        public string[] AttackEffect { get; set; }
        public string[] HitEffect { get; set; }
        public string[] HitEffect2 { get; set; }
        public bool IsFlying { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public int AttackCount { get; set; }
        public string DieEffect { get; set; }
        public string[] Animation { get; set; }
        public int[] UnitOfType { get; set; }
        public bool IsJumper { get; set; }
        public int MovementOffsetAmount { get; set; }
        public int MovementOffsetSpeed { get; set; }
        public string TombStone { get; set; }
        public int[] DieDamage { get; set; }
        public int DieDamageRadius { get; set; }
        public string[] DieDamageEffect { get; set; }
        public int DieDamageDelay { get; set; }
        public bool DisableProduction { get; set; }
        public string SecondaryTroop { get; set; }
        public bool IsSecondaryTroop { get; set; }
        public int[] SecondaryTroopCnt { get; set; }
        public int[] SecondarySpawnDist { get; set; }
        public bool RandomizeSecSpawnDist { get; set; }
        public bool PickNewTargetAfterPushback { get; set; }
        public int PushbackSpeed { get; set; }
        public string SummonTroop { get; set; }
        public int[] SummonTroopCount { get; set; }
        public int[] SummonCooldown { get; set; }
        public string SummonEffect { get; set; }
        public int[] SummonLimit { get; set; }
        public int SpawnIdle { get; set; }
        public int[] StrengthWeight { get; set; }
        public string ChildTroop { get; set; }
        public int ChildTroopCount { get; set; }
        public int SpeedDecreasePerChildTroopLost { get; set; }
        public int ChildTroop0_X { get; set; }
        public int ChildTroop0_Y { get; set; }
        public int ChildTroop1_X { get; set; }
        public int ChildTroop1_Y { get; set; }
        public int ChildTroop2_X { get; set; }
        public int ChildTroop2_Y { get; set; }
        public bool AttackMultipleBuildings { get; set; }
        public bool IncreasingDamage { get; set; }
        public int DPSLv2 { get; set; }
        public int DPSLv3 { get; set; }
        public int Lv2SwitchTime { get; set; }
        public int Lv3SwitchTime { get; set; }
        public string AttackEffectLv2 { get; set; }
        public string AttackEffectLv3 { get; set; }
        public string AttackEffectLv4 { get; set; }
        public string TransitionEffectLv2 { get; set; }
        public string TransitionEffectLv3 { get; set; }
        public string TransitionEffectLv4 { get; set; }
        public int HitEffectOffset { get; set; }
        public int TargetedEffectOffset { get; set; }
        public int SecondarySpawnOffset { get; set; }
        public string CustomDefenderIcon { get; set; }
        public int SpecialMovementMod { get; set; }
        public int InvisibilityRadius { get; set; }
        public int HealthReductionPerSecond { get; set; }
        public int AutoMergeDistance { get; set; }
        public int AutoMergeGroupSize { get; set; }
        public bool IsUnderground { get; set; }
        public int ProjectileBounces { get; set; }
        public int FriendlyGroupWeight { get; set; }
        public int EnemyGroupWeight { get; set; }
        public int NewTargetAttackDelay { get; set; }
        public bool TriggersTraps { get; set; }
        public int ChainShootingDistance { get; set; }
        public string PreAttackEffect { get; set; }
        public string MoveTrailEffect { get; set; }
        public string BecomesTargetableEffect { get; set; }
        public bool BoostedIfAlone { get; set; }
        public int BoostRadius { get; set; }
        public int BoostDmgPerfect { get; set; }
        public int BoostAttackSpeed { get; set; }
        public string HideEffect { get; set; }
        public bool DisableDonate { get; set; }
        public bool ScaleByTH { get; set; }

        public override int GetCombatItemType() => 0;

        public override int GetHousingSpace() => HousingSpace;

        public override int GetRequiredLaboratoryLevel(int level) => LaboratoryLevel[level];

        public override int GetRequiredProductionHouseLevel() => BarrackLevel;

        public override int GetTrainingCost(int level) => TrainingCost[level];

        public override Resource GetTrainingResource() => CSV.Tables.Get(Gamefile.Resources).GetData(TrainingResource) as Resource;

        public override int GetTrainingTime() => TrainingTime;

        public override int GetUpgradeCost(int level) => UpgradeCost[level];

        public override int GetUpgradeLevelCount() => UpgradeCost.Length;

        public override Resource GetUpgradeResource(int level) => CSV.Tables.Get(Gamefile.Resources).GetData(UpgradeResource[level].ToString()) as Resource;

        public override int GetUpgradeTime(int level) => UpgradeTimeH[level] * 3600;

    }
}
