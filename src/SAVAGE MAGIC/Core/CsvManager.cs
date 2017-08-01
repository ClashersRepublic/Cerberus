using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Magic.Files.CSV;
using Magic.Files.Logic;
using static Magic.ClashOfClans.Core.Logger;

namespace Magic.ClashOfClans.Core
{
    internal static class CsvManager
    {
        private static readonly List<Tuple<string, string, int>> _gameFiles = new List<Tuple<string, string, int>>();
        private static readonly DataTables _dataTables = new DataTables();

        public static void Initialize()
        {
            _gameFiles.Add(new Tuple<string, string, int>("Buildings", @"contents/logic/buildings.csv", 0));
            _gameFiles.Add(new Tuple<string, string, int>("Resources", @"contents/logic/resources.csv", 2));
            _gameFiles.Add(new Tuple<string, string, int>("Characters", @"contents/logic/characters.csv", 3));
            _gameFiles.Add(new Tuple<string, string, int>("Obstacles", @"contents/logic/obstacles.csv", 7));
            _gameFiles.Add(new Tuple<string, string, int>("Experience Levels", @"contents/logic/experience_levels.csv", 10));
            _gameFiles.Add(new Tuple<string, string, int>("Traps", @"contents/logic/traps.csv", 11));
            _gameFiles.Add(new Tuple<string, string, int>("Leagues", @"contents/logic/leagues.csv", 12));
            _gameFiles.Add(new Tuple<string, string, int>("Globals", @"contents/logic/globals.csv", 13));
            _gameFiles.Add(new Tuple<string, string, int>("Townhall Levels", @"contents/logic/townhall_levels.csv", 14));
            _gameFiles.Add(new Tuple<string, string, int>("NPCs", @"contents/logic/npcs.csv", 16));
            _gameFiles.Add(new Tuple<string, string, int>("Decos", @"contents/logic/decos.csv", 17));
            _gameFiles.Add(new Tuple<string, string, int>("Shields", @"contents/logic/shields.csv", 19));
            _gameFiles.Add(new Tuple<string, string, int>("Achievements", @"contents/logic/achievements.csv", 22));
            _gameFiles.Add(new Tuple<string, string, int>("Spells", @"contents/logic/spells.csv", 25));
            _gameFiles.Add(new Tuple<string, string, int>("Heroes", @"contents/logic/heroes.csv", 27));
            int count = 0;
            try
            {
                Parallel.ForEach(_gameFiles, file =>
                {

                    _dataTables.InitDataTable(new CsvTable(file.Item2), file.Item3);
                    ++count;
                });
                Say("Successfully loaded " + count + "  CSV file(s).");
            }
            catch (Exception Ex)
            {
                Say();
                Error("Error loading game files. Looks like you have :");
                Error("     -> Edited the Files Wrong");
                Error("     -> Made mistakes by deleting values");
                Error("     -> Entered too High or Low value");
                Error("     -> Please check to these errors");
                Error(Ex.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static List<Tuple<string, string, int>> Gamefiles => _gameFiles;
        public static DataTables DataTables => _dataTables;
    }
}
