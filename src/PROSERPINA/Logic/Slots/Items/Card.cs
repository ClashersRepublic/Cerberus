using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Files;
using BL.Servers.CR.Files.CSV_Logic;
using BL.Servers.CR.Logic.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BL.Servers.CR.Logic.Slots.Items
{
    internal class Card
    {
       [JsonProperty("cnt")] internal int Count = 0;

        [JsonProperty("id")] internal int ID = 0;

        [JsonProperty("global_id")] internal int GId = 0;
        [JsonProperty("lvl")] internal int Level = 0;
        [JsonProperty("new")] internal byte New = 0;
        [JsonProperty("type")] internal int Type = 0;

        public Card()
        {
            
        }

        public Card(int _Type, int _ID, int _Count, int _Level, byte _isNew)
        {
            this.Type = _Type;
            this.ID = _ID;
            this.GId = this.Type * 1000000 + _ID;
            this.Count = _Count;
            this.Level = _Level;
            this.New = _isNew;
        }

        public void Upgrade()
        {
            if (this.Type == 26)
            {
                Spells_Characters _Card = CSV.Tables.Get(Gamefile.Spells_Characters).GetDataWithID(this.ID) as Spells_Characters;
                Rarities _Rarity = CSV.Tables.Get(Gamefile.Rarities).GetData(_Card.Rarity) as Rarities;

                if (this.Level < _Rarity.LevelCount)
                {
                    this.Count -= _Rarity.UpgradeMaterialCount[this.Level];
                    this.Level++;
                }
            }
            else if (this.Type == 27)
            {
                Spells_Buildings _Card = CSV.Tables.Get(Gamefile.Spells_Buildings).GetDataWithID(this.ID) as Spells_Buildings;
                Rarities _Rarity = CSV.Tables.Get(Gamefile.Rarities).GetData(_Card.Rarity) as Rarities;

                if (this.Level < _Rarity.LevelCount)
                {
                    this.Count -= _Rarity.UpgradeMaterialCount[this.Level];
                    this.Level++;
                }
            }
            else if (this.Type == 28)
            {
                Spells_Others _Card = CSV.Tables.Get(Gamefile.Spells_Others).GetDataWithID(this.ID) as Spells_Others;
                Rarities _Rarity = CSV.Tables.Get(Gamefile.Rarities).GetData(_Card.Rarity) as Rarities;

                if (this.Level < _Rarity.LevelCount)
                {
                    this.Count -= _Rarity.UpgradeMaterialCount[this.Level];
                    this.Level++;
                }
            }
        }
    }
}
