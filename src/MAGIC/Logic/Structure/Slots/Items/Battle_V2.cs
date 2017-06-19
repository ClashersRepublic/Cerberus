using System;
using CRepublic.Magic.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Battle_V2
    {
        internal JsonSerializerSettings Client_JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
        };

        internal double Last_Tick;

        internal double Preparation_Time = 60;
        internal double Attack_Time = 180;

        internal double Battle_Tick
        {
            get
            {
                if (this.Preparation_Time > 0) return this.Preparation_Time;
                return this.Attack_Time;
            }
            set
            {
                if (this.Preparation_Time >= 1 && this.Commands.Count < 1)
                {
                    this.Preparation_Time -= (value - this.Last_Tick) / 63;
                    Console.WriteLine($"Preparation Time for {this.Attacker.Name} : " + TimeSpan.FromSeconds(this.Preparation_Time).TotalSeconds);
                }
                else
                {
                    this.Attack_Time -= (value - this.Last_Tick) / 63;
                    Console.WriteLine($"Attack Time for {this.Attacker.Name} : " + TimeSpan.FromSeconds(this.Attack_Time).TotalSeconds);
                }
                this.Last_Tick = value;
                this.End_Tick = (int)value;
            }
        }

        [JsonProperty("level")] internal JObject Level;

        [JsonProperty("attacker")] internal Player Attacker;

        [JsonProperty("defender")] internal Player Defender;

        [JsonProperty("replay_info")] internal Replay_Info_V2 Replay_Info = new Replay_Info_V2();

        [JsonProperty("end_tick")] internal int End_Tick;

        [JsonProperty("timestamp")] internal int TimeStamp = (int)TimeUtils.ToUnixTimestamp(DateTime.UtcNow);

        [JsonProperty("cmd")] internal Battle_Commands Commands = new Battle_Commands();

        [JsonProperty("calendar")] internal Calendar Calendar = new Calendar();

        [JsonProperty("globals")] internal Globals_Replay Globals = new Globals_Replay();

        [JsonProperty("prep_skip")] internal int Preparation_Skip;
        internal Battle_V2()
        {
            //Batle
        }

        internal Battle_V2(Level _Attacker, Level _Enemy)
        {
            this.Attacker = _Attacker.Avatar.Clone();
            this.Defender = _Enemy.Avatar.Clone();
            this.Level = _Enemy.GameObjectManager.JSON;

            this.Attacker.Units = new Units();
            this.Attacker.Spells = new Units();
            this.Attacker.Heroes_Health = new Slots();
            this.Attacker.Heroes_States = new Slots();
        }

        internal void Add_Command(Battle_Command Command)
        {
            this.Commands.Add(this, Command);
        }

        internal void Set_Replay_Info()
        {
            this.Replay_Info.Stats.Home_ID[0] = this.Defender.UserHighId;
            this.Replay_Info.Stats.Home_ID[1] = this.Defender.UserLowId;
            this.Replay_Info.Stats.Original_Attacker_Score = this.Attacker.Trophies;
            this.Replay_Info.Stats.Original_Defender_Score = this.Defender.Trophies;
            this.Replay_Info.Stats.Battle_Time = 180 - (int)this.Attack_Time + 1;
        }

        internal string Json => JsonConvert.SerializeObject(new
        {
            level = this.Level,
            attacker = this.Attacker,
            defender = this.Defender,
            end_tick = this.End_Tick,
            timestamp = this.TimeStamp,
            cmd = this.Commands,
            calendar = this.Calendar,
            globals = this.Globals,
            prep_skip = this.Preparation_Skip
        }, this.Client_JsonSettings);
    }
}
