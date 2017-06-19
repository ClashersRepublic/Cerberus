using System;
using Newtonsoft.Json;

namespace CRepublic.Royale.Logic.Slots.Items
{
    internal class Chest
    {
        [JsonProperty("emplacement")] internal byte Emplacement = 0;
        [JsonProperty("id")] internal int ID = 0;
        [JsonProperty("unlocked")] internal bool Unlocked = false;
        [JsonProperty("unlocktime")] internal DateTime UnlockTime = DateTime.Now;
        [JsonProperty("new")] internal bool New = false;

        public Chest()
        {
        }

        public Chest(byte _Emplacement, int _ID, bool _Unlocked, DateTime _UnlockTime, bool _isNew)
        {
            this.Emplacement = _Emplacement;
            this.ID = _ID;
            this.Unlocked = _Unlocked;
            this.UnlockTime = _UnlockTime;
            this.New = _isNew;
        }
    }
}
