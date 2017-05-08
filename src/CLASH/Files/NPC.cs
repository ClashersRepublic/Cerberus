using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BL.Servers.CoC.Files
{
    class NPC
    {
        internal static Dictionary<int, string> Levels = new Dictionary<int, string>();

        internal NPC()
        {
            string[] Files = Directory.GetFiles(@"Gamefiles\level\", "npc*.json");

            NPC.Levels.Add(17000000, File.ReadAllText(@"Gamefiles\level\tutorial_npc.json", Encoding.UTF8));
            NPC.Levels.Add(17000001, File.ReadAllText(@"Gamefiles\level\tutorial_npc2.json", Encoding.UTF8));

            for (int _Index = 0; _Index < Files.Length; _Index++)
            {
                NPC.Levels.Add(_Index + 17000002, File.ReadAllText(Files[_Index], Encoding.UTF8));
            }

            Console.WriteLine((Files.Length + 2) + " NPC Files, loaded and stored in memory.");
        }
    }
}