using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Packets.Messages.Server.Battle
{
    internal class Pc_Battle_Data_V2 : Message
    {
        internal JsonSerializerSettings Client_JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
        };

        internal Level Enemy;
        internal JObject EnemyObject;

        public Pc_Battle_Data_V2(Device Device, Level Enemy) : base(Device)
        {
            this.Identifier = 25023;
            this.Enemy = Enemy;
            this.EnemyObject = Enemy.GameObjectManager.JSON;
        }

        internal override void Encode()
        {
            this.Data.AddRange(this.Device.Player.Avatar.ToBytes);
            this.Data.AddLong(this.Enemy.Avatar.UserId); //Opponent id
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddCompressed(this.Json);
            this.Data.AddCompressed("{\"event\":[]}");
            this.Data.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
        }

        internal override void Process()
        {
            this.Device.State = Logic.Enums.State.IN_1VS1_BATTLE;
        }

        internal string Json => JsonConvert.SerializeObject(new
        {
            exp_ver = 1,
            buildings = EnemyObject.SelectToken("buildings"),
            obstacles = EnemyObject.SelectToken("obstacles"),
            traps = EnemyObject.SelectToken("traps"),
            decos = EnemyObject.SelectToken("decos"),
            vobjs = EnemyObject.SelectToken("vobjs"),
            buildings2 = EnemyObject.SelectToken("buildings2"),
            obstacles2 = EnemyObject.SelectToken("obstacles2"),
            traps2 = EnemyObject.SelectToken("traps2"),
            decos2 = EnemyObject.SelectToken("decos2"),
            vobjs2 = EnemyObject.SelectToken("vobjs2"),
            avatar_id_high = this.Enemy.Avatar.UserHighId,
            avatar_id_low = this.Enemy.Avatar.UserLowId,
            name = this.Enemy.Avatar.Name,
            alliance_name = this.Enemy.Avatar.Alliance_Name,
            xp_level = this.Enemy.Avatar.Level,
            alliance_id_high = this.Enemy.Avatar.ClanHighID,
            alliance_id_low = this.Enemy.Avatar.ClanLowID,
            badge_id = this.Enemy.Avatar.Badge_ID,
            alliance_exp_level = this.Enemy.Avatar.Alliance_Level,
            alliance_unit_visit_capacity = 0,
            alliance_unit_spell_visit_capacity = 0,
            league_type = this.Enemy.Avatar.League,
            resources = this.Enemy.Avatar.Resources,
            alliance_units = this.Enemy.Avatar.Castle_Units,
            hero_states = this.Enemy.Avatar.Heroes_States,
            hero_health = this.Enemy.Avatar.Heroes_Health,
            hero_upgrade = this.Enemy.Avatar.Heroes_Upgrades,
            hero_modes = this.Enemy.Avatar.Heroes_Modes,
            variables = this.Enemy.Avatar.Variables,
            castle_lvl = this.Enemy.Avatar.Castle_Level,
            castle_total = this.Enemy.Avatar.Castle_Total,
            castle_used = this.Enemy.Avatar.Castle_Used,
            castle_total_sp = this.Enemy.Avatar.Castle_Total_SP,
            castle_used_sp = this.Enemy.Avatar.Castle_Used_SP,
            town_hall_lvl = this.Enemy.Avatar.TownHall_Level,
            th_v2_lvl = this.Enemy.Avatar.Builder_TownHall_Level,
            score = this.Enemy.Avatar.Trophies,
            duel_score = this.Enemy.Avatar.Builder_Trophies,

        }, this.Client_JsonSettings);
    }
}
