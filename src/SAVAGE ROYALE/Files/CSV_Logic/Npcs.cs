using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Npcs : Data
    {
        public Npcs(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(Row);
        }
        public string Name { get; set; }
        public string MapInstanceName { get; set; }
        public string[] MapDependencies { get; set; }
        public string TID { get; set; }
        public int ExpLevel { get; set; }
        public string UnitType { get; set; }
        public int UnitCount { get; set; }
        public string LevelFile { get; set; }
        public int Gold { get; set; }
        public int Elixir { get; set; }
        public bool AlwaysUnlocked { get; set; }
        public string PlayerName { get; set; }
        public string AllianceName { get; set; }

    }
}
