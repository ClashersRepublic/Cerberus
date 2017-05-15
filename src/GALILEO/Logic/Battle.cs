using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Logic.Structure.Slots;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic
{
    internal class Battle
    {
        internal double Last_Tick;

        internal double Preparation_Time = 30;
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
                    Console.WriteLine("Preparation Time : " + TimeSpan.FromSeconds(this.Preparation_Time).TotalSeconds);
                }
                else
                {
                    this.Attack_Time -= (value - this.Last_Tick) / 63;
                    Console.WriteLine("Attack Time      : " + TimeSpan.FromSeconds(this.Attack_Time).TotalSeconds);
                }
                this.Last_Tick = value;
                this.End_Tick = (int)value;
            }
        }
        [JsonProperty("end_tick")]
        internal int End_Tick;

        [JsonProperty("timestamp")]
        internal int TimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            
        [JsonProperty("cmd")]
        internal Battle_Commands Commands = new Battle_Commands();

        [JsonProperty("prep_skip")]
        internal int Preparation_Skip = 0;
    }
}
