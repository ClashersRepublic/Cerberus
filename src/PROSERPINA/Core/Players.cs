using System;
using BL.Servers.CR.Packets;

namespace BL.Servers.CR.Core
{
    using System.Collections.Generic;
    using BL.Servers.CR.Database;
    using BL.Servers.CR.Extensions;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Logic.Enums;
    using Newtonsoft.Json;

    internal class Players : Dictionary<long, Logic.Player>
    {
        internal JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.Auto,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            PreserveReferencesHandling  = PreserveReferencesHandling.All,   ReferenceLoopHandling   = ReferenceLoopHandling.Ignore,
            Formatting                  = Formatting.Indented,              Converters              = { new Utils.ArrayReferencePreservngConverter() },
        };

        internal long Seed;
        internal object Gate = new object();
        internal object GateAdd = new object();

        internal Players()
        {
            this.Seed = Mysql_Backup.GetPlayerSeed() + 1;
        }

        internal void Add(Logic.Player Player)
        {
            lock (this.GateAdd)
            {
                if (this.ContainsKey(Player.UserId))
                {
                    this[Player.UserId] = Player;
                }
                else
                {
                    this.Add(Player.UserId, Player);
                }
            }
        }

        internal void Remove(Logic.Player Player)
        {
            if (this.Remove(Player.UserId))
            {
                this.Save(Player);
                Player.Device.Player = null;
            }

            if (Player.Device != null)
            {
                if (Resources.Devices.ContainsKey(Player.Device.SocketHandle))
                {
                    Resources.Devices.Remove(Player.Device);
                    Player.Device = null;
                }
            }
        }

        internal Logic.Player Get(long UserId, DBMS DBMS = Constants.Database, bool Store = true)
        {
            if (!this.ContainsKey(UserId))
            {
                Logic.Player Player = null;

                switch (DBMS)
                {
                    case DBMS.MySQL:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            BL.Servers.CR.Database.Player Data = Database.Player.Find(UserId);

                            if (!string.IsNullOrEmpty(Data?.Data))
                            {
                                if (!string.IsNullOrEmpty(Data?.Data))
                                {
                                    Player = JsonConvert.DeserializeObject<Logic.Player>(Data.Data, this.Settings);

                                    if (Store)
                                    {
                                        this.Add(Player);
                                    }
                                }
                            }
                        }
                        break;
                    case DBMS.Redis:
                        string Property = Redis.Players.StringGet(UserId.ToString()).ToString();

                        if (!string.IsNullOrEmpty(Property))
                        {
                            Player = JsonConvert.DeserializeObject<Logic.Player>(Property, this.Settings);

                            if (Store)
                            {
                                this.Add(Player);
                            }
                        }
                        break;
                    case DBMS.Both:
                        Player = this.Get(UserId, DBMS.Redis, Store);

                        if (Player == null)
                        {
                            Player = this.Get(UserId, DBMS.MySQL, Store);
                            if (Player != null)
                                this.Save(Player, DBMS.Redis);

                        }
                        break;
                }
                return Player;
            }
            return this[UserId];
        }

        internal Logic.Player New(long UserId = 0, DBMS DBMS = Constants.Database, bool Store = true)
        {
            Logic.Player Player = null;

            if (UserId == 0)
            {
                lock (this.Gate)
                {
                    Player = new Logic.Player(null, this.Seed++);
                }
            }
            else
            {
                Player = new Logic.Player(null, UserId);
            }

            if (string.IsNullOrEmpty(Player.Token))
            {
                for (int i = 0; i < 20; i++)
                {
                    char Letter = (char) Resources.Random.Next('A', 'Z');
                    Player.Token += Letter;
                }
            }
            if (string.IsNullOrEmpty(Player.Password))
            {
                for (int i = 0; i < 6; i++)
                {
                    char Letter = (char) Resources.Random.Next('A', 'Z');
                    char Number = (char) Resources.Random.Next('1', '9');
                    Player.Password += Letter;
                    Player.Password += Number;
                }
            }
            Player.Decks = JsonConvert.DeserializeObject<Logic.Slots.Decks>(Files.Home.Starting_Home, this.Settings);
            //Player.LoadFromJSON(Files.Home.Starting_Home);

            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.MySQL:
                    {

                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Player.Add(new BL.Servers.CR.Database.Player
                            {
                                ID = Player.UserId,
                                Data = JsonConvert.SerializeObject(Player, this.Settings)
                            });

                            Database.SaveChanges();
                        }

                        if (Store)
                        {
                            this.Add(Player);
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        this.Save(Player, DBMS);

                        if (Store)
                        {
                            this.Add(Player);
                        }
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Player, DBMS);
                        DBMS = DBMS.MySQL;

                        if (Store)
                        {
                            this.Add(Player);
                        }

                        continue;
                    }
                }
                break;
            }

            return Player;
        }

        internal void Save(Logic.Player Player, DBMS DBMS = Constants.Database)
        {
            while (true)
            {

                switch (DBMS)
                {
                    case DBMS.MySQL:
                    {

                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var Data = Database.Player.Find(Player.UserId);

                            if (Data != null)
                            {
                                Data.Data = JsonConvert.SerializeObject(Player, this.Settings);
                                Database.SaveChanges();
                            }
                        }

                        break;
                    }

                    case DBMS.Redis:
                    {
                        Redis.Players.StringSet(Player.UserId.ToString(),
                            JsonConvert.SerializeObject(Player, this.Settings),
                            TimeSpan.FromHours(4));
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Player, DBMS.MySQL);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }
    }
}


