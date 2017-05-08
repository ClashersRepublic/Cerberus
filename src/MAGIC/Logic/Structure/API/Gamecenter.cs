using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.API
{
    internal class Gamecenter
    {
        internal Player Player;

        [JsonProperty("gc_id")] internal string Identifier;
        [JsonProperty("gc_cert")] internal string Certificate;
        [JsonProperty("gc_bun")] internal string AppBundle;

        internal Gamecenter()
        {
            // Gamecenter.
        }
        internal Gamecenter(Player Player)
        {
            this.Player = Player;
        }
        internal bool Filled => !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Certificate) && !string.IsNullOrEmpty(this.AppBundle);
    }
}
