namespace BL.Servers.BB.Files
{
    using System;
    using System.Collections.Generic;
    using BL.Servers.BB.Files.CSV_Reader;

    internal class CSV
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal static Gamefiles Tables;
        internal CSV()
        {
            CSV.Gamefiles.Add(1, @"Gamefiles/csv/buildings.csv");
            CSV.Gamefiles.Add(3, @"Gamefiles/csv/resources.csv");
            CSV.Gamefiles.Add(4, @"Gamefiles/csv/characters.csv");
            CSV.Gamefiles.Add(8, @"Gamefiles/csv/obstacles.csv");
            CSV.Gamefiles.Add(11, @"Gamefiles/csv/experience_levels.csv");
            CSV.Gamefiles.Add(12, @"Gamefiles/csv/traps.csv");
            CSV.Gamefiles.Add(14, @"Gamefiles/csv/globals.csv");
            CSV.Gamefiles.Add(18, @"Gamefiles/csv/decos.csv");
            CSV.Gamefiles.Add(21, @"Gamefiles/csv/missions.csv");
            CSV.Tables = new Gamefiles();

            //Parallel.ForEach(CSV.Gamefiles, File => //Parallel is slower in this case (When we have load csv it will help)
            foreach (var File in CSV.Gamefiles)
            {
                if (new System.IO.FileInfo(File.Value).Exists)
                {
                    CSV.Tables.Initialize(new Table(File.Value), File.Key);
                }
            }//);
            
            Console.WriteLine(CSV.Gamefiles.Count + " CSV Files, loaded and stored in memory.\n");
        }
    }
}