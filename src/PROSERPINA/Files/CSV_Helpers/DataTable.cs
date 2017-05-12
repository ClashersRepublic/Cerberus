using BL.Servers.CR.Files.CSV_Logic;
using BL.Servers.CR.Logic.Enums;
namespace BL.Servers.CR.Files.CSV_Helpers
{
    using System.Collections.Generic;
    using BL.Servers.CR.Files.CSV_Reader;
    internal class DataTable
    {
        internal List<Data> Datas;
        internal int Index;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTable"/> class.
        /// </summary>
        internal DataTable()
        {
            this.Datas = new List<Data>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTable"/> class.
        /// </summary>
        /// <param name="Table">The table.</param>
        /// <param name="Index">The index.</param>
        internal DataTable(Table Table, int Index)
        {
            this.Index = Index;
            this.Datas = new List<Data>();

            for (int i = 0; i < Table.GetRowCount(); i++)
            {
                Row Row = Table.GetRowAt(i);
                Data Data = this.Create(Row);

                this.Datas.Add(Data);
            }
        }

        internal Data Create(Row _Row)
        {
            Data _Data;

            switch (this.Index)
            {                
                //case 1:
                //    _Data = new Abilities(_Row, this);
                //    break;
                //case 2:
                //    _Data = new Achievements(_Row, this);
                //    break;
                //case 3:
                //    _Data = new Alliance_Badges(_Row, this);
                //    break;
                //case 4:
                //    _Data = new Alliance_Roles(_Row, this);
                //    break;
                //case 5:
                //    _Data = new Area_Effect(_Row, this);
                //    break;
                //case 6:
                //    _Data = new Arenas(_Row, this);
                //    break;
                //case 7:
                //    _Data = new Buildings(_Row, this);
                //    break;
                //case 8:
                //    _Data = new Character_Buffs(_Row, this);
                //    break;
                //case 9:
                //    _Data = new Characters(_Row, this);
                //    break;
                //case 10:
                //    _Data = new Chest_Order(_Row, this);
                //    break;
                //case 11:
                //    _Data = new Configuration_Definitions(_Row, this);
                //    break;
                //case 12:
                //    _Data = new Content_Tests(_Row, this);
                //    break;
                //case 13:
                //    _Data = new Decos(_Row, this);
                //    break;
                //case 14:
                //    _Data = new Draft_Deck(_Row, this);
                //    break;
                //case 15:
                //    _Data = new Event_Categories(_Row, this);
                //    break;
                //case 16:
                //    _Data = new Event_Category_Definitions(_Row, this);
                //    break;
                //case 17:
                //    _Data = new Event_Category_Enums(_Row, this);
                //    break;
                //case 18:
                //    _Data = new Event_Category_Object_Definitions(_Row, this);
                //    break;
                //case 19:
                //    _Data = new Exp_Levels(_Row, this);
                //    break;
                //case 20:
                //    _Data = new Gamble_Chests(_Row, this);
                //    break;
                //case 21:
                //    _Data = new Game_Modes(_Row, this);
                //    break;
                //case 22:
                //    _Data = new Globals(_Row, this);
                //    break;
                //case 23:
                //    _Data = new Heroes(_Row, this);
                //    break;
                //case 24:
                //    _Data = new Locales(_Row, this);
                //    break;
                //case 25:
                //    _Data = new Locations(_Row, this);
                //    break;
                //case 26:
                //    _Data = new NPCS(_Row, this);
                //    break;
                //case 27:
                //    _Data = new Predefined_Decks(_Row, this);
                //    break;
                //case 28:
                //    _Data = new Projectiles(_Row, this);
                //    break;
                //case 29:
                //    _Data = new Rarities(_Row, this);
                //    break;
                //case 30:
                //    _Data = new Regions(_Row, this);
                //    break;
                //case 31:
                //    _Data = new Resource_Packs(_Row, this);
                //    break;
                //case 32:
                //    _Data = new Resources(_Row, this);
                //    break;
                //case 33:
                //    _Data = new Shop(_Row, this);
                //    break;
                //case 34:
                //    _Data = new Spell_Sets(_Row, this);
                //    break;
                //case 35:
                //    _Data = new Spells_Buildings(_Row, this);
                //    break;
                //case 36:
                //    _Data = new Spells_Characters(_Row, this);
                //    break;
                //case 37:
                //    _Data = new Spells_Heroes(_Row, this);
                //    break;
                //case 38:
                //    _Data = new Spells_Others(_Row, this);
                //    break;
                //case 39:
                //    _Data = new Survival_Modes(_Row, this);
                //    break;
                //case 40:
                //    _Data = new Taunts(_Row, this);
                //    break;
                //case 41:
                //    _Data = new Tournament_Tiers(_Row, this);
                //    break;
                //case 42:
                //    _Data = new Treasure_Chests(_Row, this);
                //    break;
                //case 43:
                //    _Data = new Tutorials_Home(_Row, this);
                //    break;
                //case 44:
                //    _Data = new Tutorial_NPC(_Row, this);
                //    break;

                default:
                {
                    _Data = new Data(_Row, this);
                    break;
                }
            }

            return _Data;
        }

        internal Data GetDataWithID(int ID)
        {
            int InstanceID = GlobalID.GetID(ID);
            return this.Datas[InstanceID];
        }

        internal Data GetDataWithInstanceID(int ID)
        {
            return this.Datas[ID];
        }

        internal Data GetData(string _Name)
        {
            return this.Datas.Find(_Data => _Data.Row.Name == _Name);
        }
    }
}