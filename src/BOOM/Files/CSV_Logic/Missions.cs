using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Files.CSV_Helpers;
using CRepublic.Boom.Files.CSV_Reader;

namespace CRepublic.Boom.Files.CSV_Logic
{
    internal class Missions : Data
    {
        public Missions(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
            //LoadData(this, this.GetType(), _Row);
        }

        public string Name { get; set; }
        public string[] Dependencies { get; set; }
        public bool Deprecated { get; set; }
        public string BuildBuilding { get; set; }
        public int BuildBuildingLevel { get; set; }
        public int BuildBuildingCount { get; set; }
        public string DefendNPC { get; set; }
        public string AttackNPC { get; set; }
        public int[] Delay { get; set; }
        public int TrainTroops { get; set; }
        public string[] TutorialText { get; set; }
        public int[] TutorialStep { get; set; }
        public bool[] Darken { get; set; }
        public string[] TutorialTextBox { get; set; }
        public string[] SpeechBubble { get; set; }
        public string[] AbilityName { get; set; }
        public bool[] RightAlignTextBox { get; set; }
        public string[] ButtonText { get; set; }
        public string[] TutorialMusic { get; set; }
        public string[] TutorialSound { get; set; }
        public int TriggerAtTownHallLevel { get; set; }
        public string TriggerAtNodeTypeFound { get; set; }
        public int TriggerAtRegionIndexExplored { get; set; }
        public int[] TriggerAtRegionIndexLiberated { get; set; }
        public int[] TriggerAtNodeIndexLiberated { get; set; }
        public string TriggerAtHeroUnlocked { get; set; }
        public string TriggerAtResourceGained { get; set; }
        public bool[] InitialTutorial { get; set; }
        public string TriggerAtEventSpawn { get; set; }
        public string TriggerAtEventWin { get; set; }


    }
}
