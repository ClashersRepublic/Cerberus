using System.IO;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files
{
    using System;
    using System.Collections.Generic;
    using BL.Servers.CR.Files.CSV_Reader;

    internal class CSV : IDisposable
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal bool _Disposed = false;

        internal static Gamefiles Tables;
        internal CSV()
        {
            CSV.Gamefiles.Add(1, @"Gamefiles/csv_logic/abilities.csv");
            CSV.Gamefiles.Add(2, @"Gamefiles/csv_logic/achievements.csv");
            CSV.Gamefiles.Add(3, @"Gamefiles/csv_logic/alliance_badges.csv");
            CSV.Gamefiles.Add(4, @"Gamefiles/csv_logic/alliance_roles.csv");
            CSV.Gamefiles.Add(5, @"Gamefiles/csv_logic/area_effect_objects.csv");
            CSV.Gamefiles.Add(6, @"Gamefiles/csv_logic/arenas.csv");
            CSV.Gamefiles.Add(7, @"Gamefiles/csv_logic/buildings.csv");
            CSV.Gamefiles.Add(8, @"Gamefiles/csv_logic/character_buffs.csv");
            CSV.Gamefiles.Add(9, @"Gamefiles/csv_logic/characters.csv");
            CSV.Gamefiles.Add(10, @"Gamefiles/csv_logic/chest_order.csv");
            CSV.Gamefiles.Add(11, @"Gamefiles/csv_logic/configuration_definitions.csv");
            CSV.Gamefiles.Add(12, @"Gamefiles/csv_logic/content_tests.csv");
            CSV.Gamefiles.Add(13, @"Gamefiles/csv_logic/decos.csv");
            CSV.Gamefiles.Add(14, @"Gamefiles/csv_logic/draft_deck.csv");
            CSV.Gamefiles.Add(15, @"Gamefiles/csv_logic/event_categories.csv");
            CSV.Gamefiles.Add(16, @"Gamefiles/csv_logic/event_category_definitions.csv");
            CSV.Gamefiles.Add(17, @"Gamefiles/csv_logic/event_category_enums.csv");
            CSV.Gamefiles.Add(18, @"Gamefiles/csv_logic/event_category_object_definitions.csv");
            CSV.Gamefiles.Add(19, @"Gamefiles/csv_logic/exp_levels.csv");
            CSV.Gamefiles.Add(20, @"Gamefiles/csv_logic/gamble_chests.csv");
            CSV.Gamefiles.Add(21, @"Gamefiles/csv_logic/game_modes.csv");
            CSV.Gamefiles.Add(22, @"Gamefiles/csv_logic/globals.csv");    
            CSV.Gamefiles.Add(23, @"Gamefiles/csv_logic/heroes.csv");
            CSV.Gamefiles.Add(24, @"Gamefiles/csv_logic/locales.csv");
            CSV.Gamefiles.Add(25, @"Gamefiles/csv_logic/locations.csv");
            CSV.Gamefiles.Add(26, @"Gamefiles/csv_logic/npcs.csv");
            CSV.Gamefiles.Add(27, @"Gamefiles/csv_logic/predefined_decks.csv");
            CSV.Gamefiles.Add(28, @"Gamefiles/csv_logic/projectiles.csv");
            CSV.Gamefiles.Add(29, @"Gamefiles/csv_logic/rarities.csv");
            CSV.Gamefiles.Add(30, @"Gamefiles/csv_logic/regions.csv");
            CSV.Gamefiles.Add(31, @"Gamefiles/csv_logic/resource_packs.csv");
            CSV.Gamefiles.Add(32, @"Gamefiles/csv_logic/resources.csv");
            CSV.Gamefiles.Add(33, @"Gamefiles/csv_logic/shop.csv");
            CSV.Gamefiles.Add(34, @"Gamefiles/csv_logic/spell_sets.csv");
            CSV.Gamefiles.Add(35, @"Gamefiles/csv_logic/spells_buildings.csv");
            CSV.Gamefiles.Add(36, @"Gamefiles/csv_logic/spells_characters.csv");
            CSV.Gamefiles.Add(37, @"Gamefiles/csv_logic/spells_heroes.csv");
            CSV.Gamefiles.Add(38, @"Gamefiles/csv_logic/spells_other.csv");
            CSV.Gamefiles.Add(39, @"Gamefiles/csv_logic/survival_modes.csv");
            CSV.Gamefiles.Add(40, @"Gamefiles/csv_logic/taunts.csv");
            CSV.Gamefiles.Add(41, @"Gamefiles/csv_logic/tournament_tiers.csv");
            CSV.Gamefiles.Add(42, @"Gamefiles/csv_logic/treasure_chests.csv");
            CSV.Gamefiles.Add(43, @"Gamefiles/csv_logic/tutorials_home.csv");
            CSV.Gamefiles.Add(44, @"Gamefiles/csv_logic/tutorials_npc.csv");

            CSV.Tables = new Gamefiles();

            Parallel.ForEach(CSV.Gamefiles, File =>
            {
                if (new System.IO.FileInfo(File.Value).Exists)
                {
                    CSV.Tables.Initialize(new Table(File.Value), File.Key);
                }
                else
                {
                    throw new FileNotFoundException($"{File.Value} does not exist!");
                }
            });
            
            Console.WriteLine(CSV.Gamefiles.Count + " CSV Files, loaded and stored in memory.\n");
        }

        ~CSV()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (!_Disposed)
            {
                if (_Disposed)
                {

                }

                CSV.Gamefiles.Clear();
                CSV.Tables.DataTables.Clear();
            }

            _Disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}