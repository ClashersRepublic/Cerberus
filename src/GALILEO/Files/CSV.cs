using System;
using System.Collections.Generic;
using System.IO;
using BL.Servers.CoC.Files.CSV_Reader;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Files
{
    internal class CSV
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal static Gamefiles Tables;
        internal CSV()
        {
            CSV.Gamefiles.Add((int)Gamefile.Buildings, @"Gamefiles/logic/buildings.csv");
            CSV.Gamefiles.Add((int)Gamefile.Resources, @"Gamefiles/logic/resources.csv");
            CSV.Gamefiles.Add((int)Gamefile.Characters, @"Gamefiles/logic/characters.csv");
            CSV.Gamefiles.Add((int)Gamefile.Obstacles, @"Gamefiles/logic/obstacles.csv");
            CSV.Gamefiles.Add((int)Gamefile.Experience_Levels, @"Gamefiles/logic/experience_levels.csv");
            CSV.Gamefiles.Add((int)Gamefile.Traps, @"Gamefiles/logic/traps.csv");
            CSV.Gamefiles.Add((int)Gamefile.Globals, @"Gamefiles/logic/globals.csv");
            CSV.Gamefiles.Add((int)Gamefile.Npcs, @"Gamefiles/logic/npcs.csv");
            CSV.Gamefiles.Add((int)Gamefile.Decos, @"Gamefiles/logic/decos.csv");
            CSV.Gamefiles.Add((int)Gamefile.Missions, @"Gamefiles/logic/missions.csv");
            CSV.Gamefiles.Add((int)Gamefile.Heroes, @"Gamefiles/logic/heroes.csv");
            CSV.Gamefiles.Add((int)Gamefile.Leagues, @"Gamefiles/logic/leagues.csv");
            CSV.Gamefiles.Add((int)Gamefile.Variables, @"Gamefiles/logic/variables.csv");
            CSV.Gamefiles.Add((int)Gamefile.Village_Objects, @"Gamefiles/logic/village_objects.csv");
            CSV.Tables = new Gamefiles();

            //Parallel.ForEach(CSV.Gamefiles, File => //Parallel is slower in this case (When we have load csv it will help)
            foreach (var File in CSV.Gamefiles)
            {
                if (new FileInfo(File.Value).Exists)
                {
                    CSV.Tables.Initialize(new Table(File.Value), File.Key);
                }
                else
                {
                    throw new FileNotFoundException($"{File.Value} does not exist!");
                }
            }//);

            Console.WriteLine(CSV.Gamefiles.Count + " CSV Files, loaded and stored in memory.\n");
        }
    }
}