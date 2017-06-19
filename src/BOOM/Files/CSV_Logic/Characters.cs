using System;

namespace CRepublic.Boom.Files.CSV_Logic
{
    using CRepublic.Boom.Files.CSV_Helpers;
    using CRepublic.Boom.Files.CSV_Reader;

    internal class Characters : Combat_Item
    {

        public Characters(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }
        public string Name { get; set; }
        public int HousingSpace { get; set; }
        public int[] UnlockTownHallLevel { get; set; }
        public int[] UpgradeHouseLevel { get; set; }
        public int Speed { get; set; }
        public int[] Hitpoints { get; set; }
        public int TrainingTime { get; set; }
        public string[] TrainingCost { get; set; }
        public int[] UpgradeTimeH { get; set; }
        public string[] UpgradeCost { get; set; }
        public int MaxAttackRange { get; set; }
        public int AttackRange { get; set; }
        public int AttackRate { get; set; }
        public int ReloadTime { get; set; }
        public int ShotsBeforeReload { get; set; }
        public int[] LaserDistance { get; set; }
        public int[] Damage { get; set; }
        public int[] LifeLeech { get; set; }
        public int AttackSpread { get; set; }
        public int AttackDistanceOffsetPercent { get; set; }
        public int[] DamageOverFiveSeconds { get; set; }
        public int DamageRadius { get; set; }
        public bool FriendlyFire { get; set; }
        public int BoostTimeMs { get; set; }
        public int SpeedBoost { get; set; }
        public int ArmorBoost { get; set; }
        public int DamageBoost { get; set; }
        public int Energy { get; set; }
        public bool IsNpc { get; set; }
        public bool CanAttackWhileWalking { get; set; }
        public bool TurnTowardsTarget { get; set; }
        public int[] XpGain { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SubtitleTID { get; set; }
        public string FlavorTID { get; set; }
        public string SWF { get; set; }
        public string SpeedTID { get; set; }
        public string AttackRangeTID { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string BigPictureSWF { get; set; }
        public string BigPicture { get; set; }
        public string Projectile { get; set; }
        public string DeployEffect { get; set; }
        public string AttackEffect { get; set; }
        public string AttackEffect2 { get; set; }
        public string AttackEffect3 { get; set; }
        public string HitEffect { get; set; }
        public string HitEffect2 { get; set; }
        public string DisembarkEffect { get; set; }
        public string DieEffect { get; set; }
        public string Animation { get; set; }
        public string AnimationEnemy { get; set; }
        public string MissEffect { get; set; }
        public string Footstep { get; set; }
        public string MoveLoopSound { get; set; }
        public int PatrolRadius { get; set; }
        public int CombatSetupTimeMs { get; set; }
        public int CombatTeardownTimeMs { get; set; }
        public int CombatTeardownDelayMs { get; set; }
        public bool Flying { get; set; }

        public override int GetCombatItemType() => 0;
        public override int GetRequiredLaboratoryLevel(int level) => UpgradeHouseLevel[level];
        public override int GetTrainingCost(int level) => Convert.ToInt32(TrainingCost[level]);

    }
}

