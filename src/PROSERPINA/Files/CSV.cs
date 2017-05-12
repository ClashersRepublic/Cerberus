using System.Threading.Tasks;

namespace BL.Servers.CR.Files
{
    using System;
    using System.Collections.Generic;
    using BL.Servers.CR.Files.CSV_Reader;

    internal class CSV
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal static Gamefiles Tables;
        internal CSV()
        {
            CSV.Gamefiles.Add(1, @"Gamefiles/csv/abilites.csv");
            CSV.Gamefiles.Add(2, @"Gamefiles/csv/achievements.csv");
            CSV.Gamefiles.Add(3, @"Gamefiles/csv/alliance_badges.csv");
            CSV.Gamefiles.Add(4, @"Gamefiles/csv/alliances_roles.csv");
            CSV.Gamefiles.Add(5, @"Gamefiles/csv/area_effect_objects.csv");
            CSV.Gamefiles.Add(6, @"Gamefiles/csv/arenas.csv");
            CSV.Gamefiles.Add(7, @"Gamefiles/csv/buildings.csv");
            CSV.Gamefiles.Add(8, @"Gamefiles/csv/character_buffs.csv");
            CSV.Gamefiles.Add(9, @"Gamefiles/csv/charcters.csv");
            CSV.Gamefiles.Add(10, @"Gamefiles/csv/chest_order.csv");
            CSV.Gamefiles.Add(11, @"Gamefiles/csv/configuration_definitions.csv");
            CSV.Gamefiles.Add(12, @"Gamefiles/csv/content_tests.csv");
            CSV.Gamefiles.Add(13, @"Gamefiles/csv/decos.csv");
            CSV.Gamefiles.Add(14, @"Gamefiles/csv/draft_deck.csv");
            CSV.Gamefiles.Add(15, @"Gamefiles/csv/event_categories.csv");
            CSV.Gamefiles.Add(16, @"Gamefiles/csv/event_category_definitions.csv");
            CSV.Gamefiles.Add(17, @"Gamefiles/csv/event_category_enums.csv");
            CSV.Gamefiles.Add(18, @"Gamefiles/csv/event_category_object_definitions.csv");
            CSV.Gamefiles.Add(19, @"Gamefiles/csv/exp_levels.csv");
            CSV.Gamefiles.Add(20, @"Gamefiles/csv/gamble_chests.csv");
            CSV.Gamefiles.Add(21, @"Gamefiles/csv/game_modes.csv");
            CSV.Gamefiles.Add(22, @"Gamefiles/csv/globals.csv");
            CSV.Gamefiles.Add(23, @"Gamefiles/csv/heroes.csv");
            CSV.Gamefiles.Add(24, @"Gamefiles/csv/locales.csv");
            CSV.Gamefiles.Add(25, @"Gamefiles/csv/locations.csv");
            CSV.Gamefiles.Add(26, @"Gamefiles/csv/npcs.csv");
            CSV.Gamefiles.Add(27, @"Gamefiles/csv/predefined_decks.csv");
            CSV.Gamefiles.Add(28, @"Gamefiles/csv/projectiles.csv");
            CSV.Gamefiles.Add(29, @"Gamefiles/csv/rarities.csv");
            CSV.Gamefiles.Add(30, @"Gamefiles/csv/regions.csv");
            CSV.Gamefiles.Add(31, @"Gamefiles/csv/resource_packs.csv");
            CSV.Gamefiles.Add(32, @"Gamefiles/csv/resources.csv");
            CSV.Gamefiles.Add(33, @"Gamefiles/csv/shop.csv");
            CSV.Gamefiles.Add(34, @"Gamefiles/csv/spell_sets.csv");
            CSV.Gamefiles.Add(35, @"Gamefiles/csv/spells_buildings.csv");
            CSV.Gamefiles.Add(36, @"Gamefiles/csv/spells_characters.csv");
            CSV.Gamefiles.Add(37, @"Gamefiles/csv/spells_heroes.csv");
            CSV.Gamefiles.Add(38, @"Gamefiles/csv/spells_other.csv");
            CSV.Gamefiles.Add(39, @"Gamefiles/csv/survival_modes.csv");
            CSV.Gamefiles.Add(40, @"Gamefiles/csv/taunts.csv");
            CSV.Gamefiles.Add(41, @"Gamefiles/csv/tournament_tiers.csv");
            CSV.Gamefiles.Add(42, @"Gamefiles/csv/treasure_chests.csv");
            CSV.Gamefiles.Add(43, @"Gamefiles/csv/tutorials_home.csv");
            CSV.Gamefiles.Add(44, @"Gamefiles/csv/tutorials_npc.csv");

            CSV.Tables = new Gamefiles();

            Parallel.ForEach(CSV.Gamefiles, File =>
            {
                if (new System.IO.FileInfo(File.Value).Exists)
                {
                    CSV.Tables.Initialize(new Table(File.Value), File.Key);
                }
            });
            
            Console.WriteLine(CSV.Gamefiles.Count + " CSV Files, loaded and stored in memory.\n");
        }
    }
}