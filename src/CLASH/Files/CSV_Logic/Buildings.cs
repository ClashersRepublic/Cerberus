using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Reader;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Files.CSV_Logic
{
    internal class Buildings : Construction_Item
    {
        public Buildings(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }
        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string[] TID_Instructor { get; set; }
        public int[] InstructorWeight { get; set; }
        public string BuildingClass { get; set; }
        public string SecondaryTargetingClass { get; set; }
        public string ShopBuildingClass { get; set; }
        public string SWF { get; set; }
        public string[] ExportName { get; set; }
        public string ExportNameNpc { get; set; }
        public string ExportNameConstruction { get; set; }
        public int[] BuildTimeD { get; set; }
        public int[] BuildTimeH { get; set; }
        public int[] BuildTimeM { get; set; }
        public int[] BuildTimeS { get; set; }
        public string[] BuildResource { get; set; }
        public int[] BuildCost { get; set; }
        public int[] TownHallLevel { get; set; }
        public int[] Width { get; set; }
        public int[] Height { get; set; }
        public string Icon { get; set; }
        public string[] ExportNameBuildAnim { get; set; }
        public string[] ExportNameUpgradeAnim { get; set; }
        public int[] MaxStoredGold { get; set; }
        public int[] MaxStoredElixir { get; set; }
        public int[] MaxStoredDarkElixir { get; set; }
        public int[] MaxStoredWarGold { get; set; }
        public int[] MaxStoredWarElixir { get; set; }
        public int[] MaxStoredWarDarkElixir { get; set; }
        public int PercentageStoredGold { get; set; }
        public int PercentageStoredElixir { get; set; }
        public int[] PercentageStoredDarkElixir { get; set; }
        public bool LootOnDestruction { get; set; }
        public bool Bunker { get; set; }
        public int[] HousingSpace { get; set; }
        public int[] HousingSpaceAlt { get; set; }
        public string ProducesResource { get; set; }
        public int[] ResourcePerHour { get; set; }
        public int[] ResourceMax { get; set; }
        public int[] ResourceIconLimit { get; set; }
        public int[] UnitProduction { get; set; }
        public bool UpgradesUnits { get; set; }
        public int[] ProducesUnitsOfType { get; set; }
        public int[] BoostCost { get; set; }
        public int[] Hitpoints { get; set; }
        public int[] RegenTime { get; set; }
        public int AttackRange { get; set; }
        public bool AltAttackMode { get; set; }
        public int AltAttackRange { get; set; }
        public int PrepareSpeed { get; set; }
        public int AttackSpeed { get; set; }
        public int CoolDownOverride { get; set; }
        public int[] DPS { get; set; }
        public int[] Damage { get; set; }
        public string PreferredTarget { get; set; }
        public int PreferredTargetDamageMod { get; set; }
        public bool RandomHitPosition { get; set; }
        public string[] DestroyEffect { get; set; }
        public string[] DestroyDamageEffect { get; set; }
        public string[] AttackEffect { get; set; }
        public string[] AttackEffect2 { get; set; }
        public string[] HitEffect { get; set; }
        public string[] Projectile { get; set; }
        public string[] ExportNameDamaged { get; set; }
        public int BuildingW { get; set; }
        public int BuildingH { get; set; }
        public string[] ExportNameBase { get; set; }
        public string[] ExportNameBaseNpc { get; set; }
        public string[] ExportNameBaseWar { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public bool AltAirTargets { get; set; }
        public bool AltGroundTargets { get; set; }
        public bool AltMultiTargets { get; set; }
        public int AmmoCount { get; set; }
        public string AmmoResource { get; set; }
        public int[] AmmoCost { get; set; }
        public int MinAttackRange { get; set; }
        public int DamageRadius { get; set; }
        public int PushBack { get; set; }
        public bool[] WallCornerPieces { get; set; }
        public string LoadAmmoEffect { get; set; }
        public string NoAmmoEffect { get; set; }
        public string ToggleAttackModeEffect { get; set; }
        public string PickUpEffect { get; set; }
        public string PlacingEffect { get; set; }
        public bool CanNotSellLast { get; set; }
        public string[] DefenderCharacter { get; set; }
        public int[] DefenderCount { get; set; }
        public int[] DefenderZ { get; set; }
        public int[] DestructionXP { get; set; }
        public bool Locked { get; set; }
        public bool Hidden { get; set; }
        public string AOESpell { get; set; }
        public string AOESpellAlternate { get; set; }
        public int TriggerRadius { get; set; }
        public string[] ExportNameTriggered { get; set; }
        public string AppearEffect { get; set; }
        public bool ForgesSpells { get; set; }
        public bool ForgesMiniSpells { get; set; }
        public bool IsHeroBarrack { get; set; }
        public string HeroType { get; set; }
        public bool IncreasingDamage { get; set; }
        public int[] DPSLv2 { get; set; }
        public int[] DPSLv3 { get; set; }
        public int[] DPSMulti { get; set; }
        public int[] Lv2SwitchTime { get; set; }
        public int[] Lv3SwitchTime { get; set; }
        public string[] AttackEffectLv2 { get; set; }
        public string[] AttackEffectLv3 { get; set; }
        public string TransitionEffectLv2 { get; set; }
        public string TransitionEffectLv3 { get; set; }
        public int[] AltNumMultiTargets { get; set; }
        public bool PreventsHealing { get; set; }
        public int[] StrengthWeight { get; set; }
        public int AlternatePickNewTargetDelay { get; set; }
        public string[] AltBuildResource { get; set; }
        public int SpeedMod { get; set; }
        public int StatusEffectTime { get; set; }
        public int[] ShockwavePushStrength { get; set; }
        public int ShockwaveArcLength { get; set; }
        public int ShockwaveExpandRadius { get; set; }
        public int TargetingConeAngle { get; set; }
        public int AimRotateStep { get; set; }
        public bool PenetratingProjectile { get; set; }
        public int PenetratingRadius { get; set; }
        public int TurnSpeed { get; set; }
        public bool NeedsAim { get; set; }
        public bool TargetGroups { get; set; }
        public int TargetGroupsRadius { get; set; }
        public string HitSpell { get; set; }
        public int HitSpellLevel { get; set; }
        public string ExportNameBeamStart { get; set; }
        public string ExportNameBeamEnd { get; set; }
        public int Damage2 { get; set; }
        public int Damage2Radius { get; set; }
        public int Damage2Delay { get; set; }
        public int Damage2Min { get; set; }
        public int Damage2FalloffStart { get; set; }
        public int Damage2FalloffEnd { get; set; }
        public string HitEffect2 { get; set; }
        public int WakeUpSpeed { get; set; }
        public int WakeUpSpace { get; set; }
        public string PreAttackEffect { get; set; }
        public bool ShareHeroCombatData { get; set; }
        public int BurstCount { get; set; }
        public int BurstDelay { get; set; }
        public int[] DieDamage { get; set; }
        public int DieDamageRadius { get; set; }
        public string DieDamageEffect { get; set; }
        public int DieDamageDelay { get; set; }
        public bool IsRed { get; set; }

        public override int GetRequiredTownHallLevel(int level) => TownHallLevel[level] - 1;

        public override int GetUpgradeLevelCount() => BuildCost.Length;

        public override int GetConstructionTime(int level)
        {
            int Total_Time = 0;
            if (BuildTimeD.Length > level + 1)
                Total_Time += BuildTimeD[level] * 86400;
            if (BuildTimeH.Length > level + 1)
                Total_Time += BuildTimeH[level] * 3600;
            if (BuildTimeM.Length > level + 1)
                Total_Time += BuildTimeM[level] * 60;
            if (BuildTimeS.Length > level + 1)
                Total_Time += BuildTimeS[level];

            return Total_Time;
            //return BuildTimeS[level] + BuildTimeM[level] * 60 + BuildTimeH[level] * 60 * 60 + BuildTimeD[level] * 60 * 60 * 24;
        }

        public override bool IsTownHall() => BuildingClass == "Town Hall";
        public bool IsWorkerBuilding() => BuildingClass == "Worker";

        public Resource GetAltBuildResource(int level) => CSV.Tables.Get(Gamefile.Resources).GetData(AltBuildResource[level]) as Resource;

        public override Resource GetBuildResource(int level) => CSV.Tables.Get(Gamefile.Resources).GetData(BuildResource[level]) as Resource;

        public List<int> GetMaxStoredResourceCounts(int level)
        {
            var maxStoredResourceCounts = new List<int>();
            var resourceDataTable = CSV.Tables.Get(Gamefile.Resources);

            foreach (Data t in resourceDataTable.Datas)
            {
                var value = 0;
                var resourceData = (Resource)t;
                var propertyName = "MaxStored" + resourceData.Name;
                if (GetType().GetProperty(propertyName) != null)
                {
                    var propertyInfo = GetType().GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        var obj = propertyInfo.GetValue(this, null);
                        value = ((int[])obj)[level];
                    }
                }
                maxStoredResourceCounts.Add(value);
            }
            return maxStoredResourceCounts;
        }
    }
}
