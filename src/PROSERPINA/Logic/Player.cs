using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Files;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Logic.Slots;
using BL.Servers.CR.Logic.Slots.Items;
using BL.Servers.CR.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Google = BL.Servers.CR.Logic.Slots.Items.Google;
using Resources = BL.Servers.CR.Logic.Slots.Resources;
using BL.Servers.CR.Logic.Components;

namespace BL.Servers.CR.Logic
{
    internal class Player
    {
        [JsonIgnore]
        internal Device Device;

        [JsonIgnore]
        internal Profile Profile;

        [JsonIgnore]
        internal Component Component;

        [JsonIgnore]
        internal Components.Battle Battle;


        [JsonIgnore]
        internal long BattleID;

        #region Long Ids

        internal long UserId
        {
            get => (((long) this.UserHighId << 32) | (this.UserLowId & 0xFFFFFFFFL));
            set
            {
                this.UserHighId = Convert.ToInt32(value >> 32);
                this.UserLowId = (int) value;
            }
        }

        internal long ClanId
        {
            get => (((long) this.ClanHighID << 32) | (this.ClanLowID & 0xFFFFFFFFL));
            set
            {
                this.ClanHighID = Convert.ToInt32(value >> 32);
                this.ClanLowID = (int) value;
            }
        }

        #endregion

        [JsonProperty("acc_hi")] internal int UserHighId;
        [JsonProperty("acc_lo")] internal int UserLowId;

        [JsonProperty("clan_hi")] internal int ClanHighID;
        [JsonProperty("clan_lo")] internal int ClanLowID;

        [JsonProperty("token")] internal string Token;
        [JsonProperty("password")] internal string Password;

        [JsonProperty("name")] internal string Username = String.Empty;
        [JsonProperty("IpAddress")] internal string IpAddress;
        [JsonProperty("region")] internal string Region;

        [JsonProperty("lvl")] internal int Level = 13;
        [JsonProperty("xp")] internal int Experience = 0;
        [JsonProperty("arena")] internal int Arena = 21;
        [JsonProperty("tutorials")] internal byte Tutorial = 8;
        [JsonProperty("changes")] internal byte Changes = 0;
        [JsonProperty("nameset")] internal byte NameSet = 0;

        [JsonProperty("wins")] internal int Wins = 0;
        [JsonProperty("loses")] internal int Loses = 0;
        [JsonProperty("games_played")] internal int Games_Played = 0;

        [JsonProperty("trophies")] internal int Trophies = 9999;
        [JsonProperty("legendary_trophies")] internal int Legendary_Trophies = 0;

        [JsonProperty("resources")] internal Resources Resources;
        [JsonProperty("resources_cap")] internal Resources Resources_Cap;
        [JsonProperty("decks")] internal Decks Decks;
        [JsonProperty("achievements")] internal Achievements Achievements;
        [JsonProperty("chests")] internal Chests Chests;

        [JsonProperty("account_locked")] internal bool Locked = false;

        [JsonProperty("last_tick")] internal DateTime LastTick = DateTime.UtcNow;
        [JsonProperty("update_date")] internal DateTime Update = DateTime.UtcNow;
        [JsonProperty("creation_date")] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty("ban_date")] internal DateTime BanTime = DateTime.UtcNow;

        [JsonProperty("google")] internal Slots.Items.Google Google;

        [JsonProperty("facebook")] internal Slots.Items.Facebook Facebook;


        internal bool Banned => this.BanTime > DateTime.UtcNow;

        internal Player()
        {
            this.Profile = new Profile(this);
            this.Component = new Component(this);
            this.Battle = new Components.Battle(this);

            this.Resources = new Resources(this);
            this.Resources_Cap = new Resources(this);
            this.Decks = new Decks(this);
            this.Google = new Slots.Items.Google(this);
            this.Facebook = new Slots.Items.Facebook(this);

            this.Achievements = new Achievements();
            this.Chests = new Chests();
        }

        internal Player(Device Device, long UserId)
        {
            this.UserId = UserId;

            this.Device = Device;

            this.Profile = new Profile(this);
            this.Component = new Component(this);
            this.Battle = new Components.Battle(this);

            this.Resources = new Resources(this, true);
            this.Resources_Cap = new Resources(this, false);
            this.Decks = new Decks(this);
            this.Google = new Slots.Items.Google(this);
            this.Facebook = new Slots.Items.Facebook(this);

            this.Achievements = new Achievements();
            this.Chests = new Chests();
        }

        public bool HasEnoughResources(Enums.Resource resource, int buildCost) => this.Resources.Get(resource) >= buildCost;

        public bool HasEnoughResources(int globalId, int buildCost) => this.Resources.Get(globalId) >= buildCost;
    }
}
