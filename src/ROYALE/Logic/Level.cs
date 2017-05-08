using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CR.Logic
{
    internal class Level
    {

        internal Device Client;
        internal Avatar Avatar;
        internal Home Home;

        internal Level()
        {
            this.Avatar = new Avatar();
        }

        internal Level(long id)
        {
            this.Avatar = new Avatar(id);
        }
        internal void LoadFromJSON(string jsonString)
        {
        }

        internal string SaveToJSON()
        {
            return "lol";
        }

        public void Tick()
        {
            this.Avatar.LastTick = DateTime.UtcNow;
        }
        
    }
}
