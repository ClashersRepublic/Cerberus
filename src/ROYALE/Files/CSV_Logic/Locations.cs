using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Locations : Data
    {
        internal Locations(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the locations.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Npc only.
        /// </summary>
        public bool NpcOnly { get; set; }

        /// <summary>
        /// Gets or sets Pvp only.
        /// </summary>
        public bool PvpOnly { get; set; }

        /// <summary>
        /// Gets or sets Shadow r.
        /// </summary>
        public int ShadowR { get; set; }

        /// <summary>
        /// Gets or sets Shadow g.
        /// </summary>
        public int ShadowG { get; set; }

        /// <summary>
        /// Gets or sets Shadow b.
        /// </summary>
        public int ShadowB { get; set; }

        /// <summary>
        /// Gets or sets Shadow a.
        /// </summary>
        public int ShadowA { get; set; }

        /// <summary>
        /// Gets or sets Shadow offset x.
        /// </summary>
        public int ShadowOffsetX { get; set; }

        /// <summary>
        /// Gets or sets Shadow offset y.
        /// </summary>
        public int ShadowOffsetY { get; set; }

        /// <summary>
        /// Gets or sets Sound.
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// Gets or sets Extra time music.
        /// </summary>
        public string ExtraTimeMusic { get; set; }

        /// <summary>
        /// Gets or sets Match length.
        /// </summary>
        public int MatchLength { get; set; }

        /// <summary>
        /// Gets or sets Win condition.
        /// </summary>
        public string WinCondition { get; set; }

        /// <summary>
        /// Gets or sets Overtime seconds.
        /// </summary>
        public int OvertimeSeconds { get; set; }

        /// <summary>
        /// Gets or sets End screen delay.
        /// </summary>
        public int EndScreenDelay { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Ambient sound.
        /// </summary>
        public string AmbientSound { get; set; }

        /// <summary>
        /// Gets or sets Overlay s c.
        /// </summary>
        public string OverlaySC { get; set; }

        /// <summary>
        /// Gets or sets Overlay export name.
        /// </summary>
        public string OverlayExportName { get; set; }

        /// <summary>
        /// Gets or sets Crowd effects.
        /// </summary>
        public bool CrowdEffects { get; set; }

        /// <summary>
        /// Gets or sets Cloud file name.
        /// </summary>
        public string CloudFileName { get; set; }

        /// <summary>
        /// Gets or sets Cloud export name.
        /// </summary>
        public string CloudExportName { get; set; }

        /// <summary>
        /// Gets or sets Cloud min scale.
        /// </summary>
        public int CloudMinScale { get; set; }

        /// <summary>
        /// Gets or sets Cloud max scale.
        /// </summary>
        public int CloudMaxScale { get; set; }

        /// <summary>
        /// Gets or sets Cloud min speed.
        /// </summary>
        public int CloudMinSpeed { get; set; }

        /// <summary>
        /// Gets or sets Cloud max speed.
        /// </summary>
        public int CloudMaxSpeed { get; set; }

        /// <summary>
        /// Gets or sets Cloud min alpha.
        /// </summary>
        public int CloudMinAlpha { get; set; }

        /// <summary>
        /// Gets or sets Cloud max alpha.
        /// </summary>
        public int CloudMaxAlpha { get; set; }

        /// <summary>
        /// Gets or sets Cloud count.
        /// </summary>
        public int CloudCount { get; set; }

        /// <summary>
        /// Gets or sets Tile data file name.
        /// </summary>
        public string TileDataFileName { get; set; }

        /// <summary>
        /// Gets or sets Walk effect.
        /// </summary>
        public string WalkEffect { get; set; }

        /// <summary>
        /// Gets or sets Walk effect overtime.
        /// </summary>
        public string WalkEffectOvertime { get; set; }

        /// <summary>
        /// Gets or sets Looping effect regular time.
        /// </summary>
        public string LoopingEffectRegularTime { get; set; }

        /// <summary>
        /// Gets or sets Looping effect overtime.
        /// </summary>
        public string LoopingEffectOvertime { get; set; }

        /// <summary>
        /// Gets or sets Looping effect.
        /// </summary>
        public string LoopingEffect { get; set; }

        /// <summary>
        /// Gets or sets Looping effect overtime side.
        /// </summary>
        public string LoopingEffectOvertimeSide { get; set; }

        /// <summary>
        /// Gets or sets Reflection red.
        /// </summary>
        public int ReflectionRed { get; set; }

        /// <summary>
        /// Gets or sets Reflection green.
        /// </summary>
        public int ReflectionGreen { get; set; }

        /// <summary>
        /// Gets or sets Reflection blue.
        /// </summary>
        public int ReflectionBlue { get; set; }
    }
}