using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [JsonProperty("type")] internal byte Type = 0;

        public Card()
        {
            
        }

        public Card(byte _Type, int _ID, int _Count, int _Level, byte _isNew)
        {
            this.Type = _Type;
            this.ID = _ID;
            this.GId = this.Type * 1000000 + _ID;
            this.Count = _Count;
            this.Level = _Level;
            this.New = _isNew;
        }
    }
}
