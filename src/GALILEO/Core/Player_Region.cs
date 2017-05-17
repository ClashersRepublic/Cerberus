using System;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using System.Collections.Generic;
using System.Timers;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Extensions;

namespace BL.Servers.CoC.Core
{
    internal class Player_Region : Dictionary<string, List_Regions>
    {
        internal object Gate = new object();
        internal object GateAdd = new object();
        internal readonly List<Timer> LTimers = new List<Timer>();

        internal Player_Region()
        {
            this.Fetch();
            this.Run();
        }

        internal void Run()
        {
            foreach (Timer Timer in this.LTimers)
            {
                Timer.Start();
            }
        }

        internal void Fetch()
        {

            foreach (var _Id in MySQL_V2.GetTopPlayer())
            {
                this.Add("INTERNATIONAL", Resources.Players.Get(_Id, Constants.Database, true));
            }
            
            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(2).TotalMilliseconds,
                AutoReset = true,
            };


            Timer.Elapsed += (_Sender, _Args) =>
            {
                foreach (var _Id in MySQL_V2.GetTopPlayer())
                {
                    this.Add("INTERNATIONAL", Resources.Players.Get(_Id, Constants.Database, true));
                }
            };

            this.LTimers.Add(Timer);
        }
        internal void Add(string Region, Level Player)
        {
            lock (this.GateAdd)
            {
                if (!string.IsNullOrEmpty(Region))
                {
                    if (this.ContainsKey(Region))
                    {
                        int Index = this[Region].Level.FindIndex(ds => ds.Avatar.UserId == Player.Avatar.UserId);
                        if (Index > -1)
                            this[Region].Level[Index] = Player;
                        else
                            this[Region].Level.Add(Player);
                    }
                    else
                    {
                        this.Add(Region, new List_Regions(Player));
                    }
                }
            }
        }

        internal void Add(Level Player)
        {
            lock (this.GateAdd)
            {
                if (!string.IsNullOrEmpty(Player.Avatar.Region))
                {
                    if (this.ContainsKey(Player.Avatar.Region))
                    {
                        int Index = this[Player.Avatar.Region].Level.FindIndex(ds => ds.Avatar.UserId == Player.Avatar.UserId);
                        if (Index > -1)
                            this[Player.Avatar.Region].Level[Index] = Player;
                        else
                            this[Player.Avatar.Region].Level.Add(Player);
                    }
                    else
                    {
                        this.Add(Player.Avatar.Region, new List_Regions(Player));
                    }
                }
            }
        }

        internal void Remove(Level Player)
        {
            if (!string.IsNullOrEmpty(Player.Avatar.Region))
            {
                if (this.ContainsKey(Player.Avatar.Region))
                {
                    this[Player.Avatar.Region].Remove(Player);
                    if (this[Player.Avatar.Region].Level.Count < 1)
                        this.Remove(Player.Avatar.Region);
                }
            }
        }

        internal List<Level> Get_Region(Level Player)
        {
            if (!string.IsNullOrEmpty(Player.Avatar.Region))
            {
                if (this.ContainsKey(Player.Avatar.Region))
                {
                    return this[Player.Avatar.Region].Level;
                }
                this.Add(Player.Avatar.Region, new List_Regions(Player));
                return this[Player.Avatar.Region].Level;
            }
            return null;
        }

        internal List<Level> Get_Region(string region)
        {
            if (!string.IsNullOrEmpty(region))
            {
                if (this.ContainsKey(region))
                {
                    return this[region].Level;
                }
                this.Add(region, new List_Regions(null));
                return this[region].Level;
            }
            return null;
        }
    }
}
