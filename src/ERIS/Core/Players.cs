using System;
using BL.Servers.BB.Packets;

namespace BL.Servers.BB.Core
{
    using System.Collections.Generic;
    using BL.Servers.BB.Database;
    using BL.Servers.BB.Extensions;
    using BL.Servers.BB.Logic;
    using BL.Servers.BB.Logic.Enums;
    using Newtonsoft.Json;

    internal class Players : Dictionary<long, Level>
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

        internal void Add(Level Player)
        {
            lock (this.GateAdd)
            {
                if (this.ContainsKey(Player.Avatar.UserId))
                {
                    this[Player.Avatar.UserId] = Player;
                }
                else
                {
                    this.Add(Player.Avatar.UserId, Player);
                }
            }
        }

        internal void Remove(Level Player)
        {
            this.Remove(Player.Avatar.UserId);

            if (Player.Client != null)
            {
                if (Resources.Devices.ContainsKey(Player.Client.SocketHandle))
                {
                    Resources.Devices.Remove(Player.Client.Socket.Handle);
                }
            }

            this.Add(Player);
        }

        internal Level Get(long UserId, DBMS DBMS = Constants.Database, bool Store = true)
        {
            if (!this.ContainsKey(UserId))
            {
                Level Player = null;

                switch (DBMS)
                {
                    case DBMS.MySQL:
                        if (Constants.IsMono)
                        {
                            Player = Mysql_Backup.Get(UserId);
                        }
                        else
                        {
                            using (MysqlEntities Database = new MysqlEntities())
                            {
                                Player Data = Database.Player.Find(UserId);

                                if (!string.IsNullOrEmpty(Data?.Data))
                                {
                                    string[] _Datas =
                                        Data.Data.Split(new string[1] {"#:#:#:#"}, StringSplitOptions.None);

                                    if (!string.IsNullOrEmpty(_Datas[0]) && !string.IsNullOrEmpty(_Datas[1]))
                                    {
                                        Player = new Level
                                        {
                                            Avatar = JsonConvert.DeserializeObject<Avatar>(_Datas[0], this.Settings)
                                        };
                                        Player.LoadFromJSON(_Datas[1]);

                                        if (Store)
                                        {
                                            this.Add(Player);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case DBMS.Redis:
                        string Property = Redis.Players.StringGet(UserId.ToString()).ToString();

                        if (!string.IsNullOrEmpty(Property))
                        {
                            string[] _Datas = Property.Split(new string[1] {"#:#:#:#"}, StringSplitOptions.None);

                            if (!string.IsNullOrEmpty(_Datas[0]) && !string.IsNullOrEmpty(_Datas[1]))
                            {
                                Player = new Level
                                {
                                    Avatar = JsonConvert.DeserializeObject<Avatar>(_Datas[0], this.Settings)
                                };
                                Player.LoadFromJSON(_Datas[1]);

                                if (Store)
                                {
                                    this.Add(Player);
                                }
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
                if (Store)
                {
                    this.Add(Player);
                }
                return Player;
            }
            return this[UserId];
        }

            internal Level New(long UserId = 0, DBMS DBMS = Constants.Database, bool Store = true)
            {
                Level Player = null;

                if (UserId == 0)
                {
                    lock (this.Gate)
                    {
                        Console.WriteLine(Seed);
                        Player = new Level(this.Seed++);
                    }
                }
                else
                {
                    Player = new Level(UserId);
                }

                if (string.IsNullOrEmpty(Player.Avatar.Token))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        char Letter = (char) Resources.Random.Next('A', 'Z');
                        Player.Avatar.Token += Letter;
                    }
                }
                if (string.IsNullOrEmpty(Player.Avatar.Password))
                {
                    for (int i = 0; i < 6; i++)
                    {
                    char Letter = (char) Resources.Random.Next('A', 'Z');
                    char Number = (char) Resources.Random.Next('1', '9');
                    Player.Avatar.Password += Letter;
                    Player.Avatar.Password += Number;
                }
            }
            Player.LoadFromJSON(Files.Home.Starting_Home);

            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.MySQL:
                    {
                        if (Constants.IsMono)
                        {
                            Mysql_Backup.New(Player);
                        }
                        else
                        {
                            using (MysqlEntities Database = new MysqlEntities())
                            {
                                Database.Player.Add(new Player
                                {
                                    ID = Player.Avatar.UserId,
                                    Data = JsonConvert.SerializeObject(Player.Avatar, this.Settings) + "#:#:#:#" +
                                           Player.SaveToJSON(),
                                });

                                Database.SaveChanges();
                            }
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

        internal void Save(Level Player, DBMS DBMS = Constants.Database)
        {
            while (true)
            {

                switch (DBMS)
                {
                    case DBMS.MySQL:
                    {
                        if (Constants.IsMono)
                        {
                            Mysql_Backup.Save(Player);
                        }
                        else
                        {
                            using (MysqlEntities Database = new MysqlEntities())
                            {
                                var Data = Database.Player.Find(Player.Avatar.UserId);

                                if (Data != null)
                                {
                                    Data.Data = JsonConvert.SerializeObject(Player.Avatar, this.Settings) + "#:#:#:#" +
                                                Player.SaveToJSON();
                                    Database.SaveChanges();
                                }
                            }
                        }

                        break;
                    }

                    case DBMS.Redis:
                    {
                        Redis.Players.StringSet(Player.Avatar.UserId.ToString(),
                            JsonConvert.SerializeObject(Player.Avatar, this.Settings) + "#:#:#:#" + Player.SaveToJSON(),
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


