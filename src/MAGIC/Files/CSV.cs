using System;
using System.Collections.Generic;
using System.IO;
using BL.Servers.CoC.Files.CSV_Reader;

namespace BL.Servers.CoC.Files
{
    internal class CSV
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal static Gamefiles Tables;
        internal CSV()
        {
            CSV.Gamefiles.Add(1, @"Gamefiles/logic/buildings.csv");
            CSV.Gamefiles.Add(3, @"Gamefiles/logic/resources.csv");
            CSV.Gamefiles.Add(4, @"Gamefiles/logic/characters.csv");
            CSV.Gamefiles.Add(8, @"Gamefiles/logic/obstacles.csv");
            //CSV.Gamefiles.Add(11, @"Gamefiles/logic/experience_levels.csv");
            CSV.Gamefiles.Add(12, @"Gamefiles/logic/traps.csv");
            CSV.Gamefiles.Add(14, @"Gamefiles/logic/globals.csv");
            CSV.Gamefiles.Add(18, @"Gamefiles/logic/decos.csv");
            CSV.Gamefiles.Add(21, @"Gamefiles/logic/missions.csv");
            CSV.Tables = new Gamefiles();

            //Parallel.ForEach(CSV.Gamefiles, File => //Parallel is slower in this case (When we have load csv it will help)
            foreach (var File in CSV.Gamefiles)
            {
                if (new System.IO.FileInfo(File.Value).Exists)
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