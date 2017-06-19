using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Tutorials_Home : Data
    {
        internal Tutorials_Home(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the tutorials_home.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets N p c.
        /// </summary>
        public string NPC { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Button t i d.
        /// </summary>
        public string ButtonTID { get; set; }

        /// <summary>
        /// Gets or sets Finish requirement.
        /// </summary>
        public string FinishRequirement { get; set; }

        /// <summary>
        /// Gets or sets Chest.
        /// </summary>
        public string Chest { get; set; }

        /// <summary>
        /// Gets or sets Wait time m s.
        /// </summary>
        public int WaitTimeMS { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Popup corner.
        /// </summary>
        public int PopupCorner { get; set; }

        /// <summary>
        /// Gets or sets Popup export name.
        /// </summary>
        public string PopupExportName { get; set; }

        /// <summary>
        /// Gets or sets Bubble export name.
        /// </summary>
        public string BubbleExportName { get; set; }

        /// <summary>
        /// Gets or sets Sound.
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// Gets or sets Darkening.
        /// </summary>
        public bool Darkening { get; set; }

        /// <summary>
        /// Gets or sets Bubble object.
        /// </summary>
        public string BubbleObject { get; set; }

        /// <summary>
        /// Gets or sets Overlay export name.
        /// </summary>
        public string OverlayExportName { get; set; }

        /// <summary>
        /// Gets or sets Spell drag export name.
        /// </summary>
        public string SpellDragExportName { get; set; }

        /// <summary>
        /// Gets or sets Spell to cast.
        /// </summary>
        public string SpellToCast { get; set; }

        /// <summary>
        /// Gets or sets Tutorial spell.
        /// </summary>
        public string TutorialSpell { get; set; }

        /// <summary>
        /// Gets or sets Spell tile x.
        /// </summary>
        public int SpellTileX { get; set; }

        /// <summary>
        /// Gets or sets Spell tile y.
        /// </summary>
        public int SpellTileY { get; set; }

        /// <summary>
        /// Gets or sets Disable spells.
        /// </summary>
        public bool DisableSpells { get; set; }

        /// <summary>
        /// Gets or sets Hide combat u i.
        /// </summary>
        public bool HideCombatUI { get; set; }

        /// <summary>
        /// Gets or sets Disable troop movement.
        /// </summary>
        public bool DisableTroopMovement { get; set; }

        /// <summary>
        /// Gets or sets Disable leader movement.
        /// </summary>
        public bool DisableLeaderMovement { get; set; }

        /// <summary>
        /// Gets or sets Disable spawn points.
        /// </summary>
        public bool DisableSpawnPoints { get; set; }

        /// <summary>
        /// Gets or sets Disable opponent spells.
        /// </summary>
        public bool DisableOpponentSpells { get; set; }

        /// <summary>
        /// Gets or sets Pause combat.
        /// </summary>
        public bool PauseCombat { get; set; }

        /// <summary>
        /// Gets or sets Dependency.
        /// </summary>
        public string Dependency { get; set; }

        /// <summary>
        /// Gets or sets Priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets Taunt.
        /// </summary>
        public string Taunt { get; set; }

        /// <summary>
        /// Gets or sets Highlight targets on mana full.
        /// </summary>
        public bool HighlightTargetsOnManaFull { get; set; }

        /// <summary>
        /// Gets or sets Disable battle start screen.
        /// </summary>
        public bool DisableBattleStartScreen { get; set; }

        /// <summary>
        /// Gets or sets Npc matches played.
        /// </summary>
        public int NpcMatchesPlayed { get; set; }

        /// <summary>
        /// Gets or sets Disable battle menu.
        /// </summary>
        public bool DisableBattleMenu { get; set; }

        /// <summary>
        /// Gets or sets Close automatically after seconds.
        /// </summary>
        public int CloseAutomaticallyAfterSeconds { get; set; }

        /// <summary>
        /// Gets or sets Group mod.
        /// </summary>
        public int GroupMod { get; set; }

        /// <summary>
        /// Gets or sets Group value.
        /// </summary>
        public int GroupValue { get; set; }
    }
}