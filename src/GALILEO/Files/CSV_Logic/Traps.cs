using BL.Servers.CoC.Files.CSV_Helpers;
using BL.Servers.CoC.Files.CSV_Reader;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Files.CSV_Logic
{
    internal class Traps : Construction_Item
    {
        public Traps(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public string[] ExportName { get; set; }
        public string[] ExportNameAir { get; set; }
        public string[] ExportNameBuildAnim { get; set; }
        public string[] ExportNameBuildAnimAir { get; set; }
        public string[] ExportNameBroken { get; set; }
        public string[] ExportNameBrokenAir { get; set; }
        public string BigPicture { get; set; }
        public string BigPictureSWF { get; set; }
        public string EffectBroken { get; set; }
        public int[] Damage { get; set; }
        public int[] DamageRadius { get; set; }
        public int TriggerRadius { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Effect { get; set; }
        public string Effect2 { get; set; }
        public string DamageEffect { get; set; }
        public bool Passable { get; set; }
        public string BuildResource { get; set; }
        public int[] BuildTimeD { get; set; }
        public int[] BuildTimeH { get; set; }
        public int[] BuildTimeM { get; set; }
        public int[] BuildCost { get; set; }
        public int[] RearmCost { get; set; }
        public int[] TownHallLevel { get; set; }
        public bool EjectVictims { get; set; }
        public int MinTriggerHousingLimit { get; set; }
        public int[] EjectHousingLimit { get; set; }
        public string[] ExportNameTriggered { get; set; }
        public string[] ExportNameTriggeredAir { get; set; }
        public int ActionFrame { get; set; }
        public string PickUpEffect { get; set; }
        public string PlacingEffect { get; set; }
        public string AppearEffect { get; set; }
        public string ToggleAttackModeEffect { get; set; }
        public int DurationMS { get; set; }
        public int SpeedMod { get; set; }
        public int DamageMod { get; set; }
        public bool AirTrigger { get; set; }
        public bool GroundTrigger { get; set; }
        public bool HealerTrigger { get; set; }
        public int HitDelayMS { get; set; }
        public int HitCnt { get; set; }
        public string[] Projectile { get; set; }
        public string Spell { get; set; }
        public int[] StrengthWeight { get; set; }
        public int PreferredTargetDamageMod { get; set; }
        public string PreferredTarget { get; set; }
        public string SpawnedCharGround { get; set; }
        public string SpawnedCharAir { get; set; }
        public int[] NumSpawns { get; set; }
        public int SpawnInitialDelayMs { get; set; }
        public int TimeBetweenSpawnsMs { get; set; }
        public bool Disabled { get; set; }
        public int ThrowDistance { get; set; }
        public int VillageType { get; set; }
        public int[] Pushback { get; set; }
        public bool[] DoNotScalePushByDamage { get; set; }
        public bool EnabledByCalendar { get; set; }
        public int DirectionCount { get; set; }
        public bool HasAltMode { get; set; }
        public override int GetConstructionTime(int level)
        {
            int Total_Time = 0;
            if (BuildTimeD.Length > level + 1)
                Total_Time += BuildTimeD[level] * 86400;
            if (BuildTimeH.Length > level + 1)
                Total_Time += BuildTimeH[level] * 3600;
            if (BuildTimeM.Length > level + 1)
                Total_Time += BuildTimeM[level] * 60;

            return Total_Time;
            //return BuildTimeS[level] + BuildTimeM[level] * 60 + BuildTimeH[level] * 60 * 60 + BuildTimeD[level] * 60 * 60 * 24;
        }
        public int GetSellPrice(int level)
        {
            var calculation = (int)(((long)BuildCost[level] * 2 * 1717986919) >> 32);
            return (calculation >> 2) + (calculation >> 31);
        }
        public override int GetRequiredTownHallLevel(int level) => TownHallLevel[level] - 1;
        public override int GetUpgradeLevelCount() => BuildCost.Length;
        public override Resource GetBuildResource(int level) => CSV.Tables.Get(Gamefile.Resources).GetData(BuildResource) as Resource;
        public override int GetBuildCost(int level) => BuildCost[level];


    }
}
