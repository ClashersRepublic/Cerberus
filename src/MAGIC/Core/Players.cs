using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using Newtonsoft.Json;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Core
{
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
            this.Seed = MySQL_V2.GetPlayerSeed() + 1;
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
            if (this.Remove(Player.Avatar.UserId))
            {
                this.Save(Player, Constants.Database);
            }

            if (Player.Client != null)
            {
                if (Resources.Devices.ContainsKey(Player.Client.SocketHandle))
                {
                    Resources.Devices.Remove(Player.Client);
                }
                Resources.GChat.Remove(Player.Client);
            }
        }

        internal Level Get(long UserId, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            if (!this.ContainsKey(UserId))
            {
                Level Player = null;

                switch (DBMS)
                {
                    case DBMS.Mysql:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Player Data = Database.Player.Find(UserId);

                            if (!string.IsNullOrEmpty(Data?.Data))
                            {
                                string[] _Datas =
                                    Data.Data.Split(new string[1] { "#:#:#:#" }, StringSplitOptions.None);

                                if (!string.IsNullOrEmpty(_Datas[0]) && !string.IsNullOrEmpty(_Datas[1]))
                                {
                                    Player = new Level
                                    {
                                        Avatar = JsonConvert.DeserializeObject<Logic.Player>(_Datas[0], this.Settings),
                                        JSON = _Datas[1],
                                    };

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
                            string[] _Datas = Property.Split(new string[1] { "#:#:#:#" }, StringSplitOptions.None);

                            if (!string.IsNullOrEmpty(_Datas[0]) && !string.IsNullOrEmpty(_Datas[1]))
                            {
                                Player = new Level
                                {
                                    Avatar = JsonConvert.DeserializeObject<Logic.Player>(_Datas[0], this.Settings),
                                    JSON = _Datas[1],
                                 };

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
                            Player = this.Get(UserId, DBMS.Mysql, Store);
                            if (Player != null)
                                this.Save(Player, DBMS.Redis);

                        }
                        break;
                }
                return Player;
            }
            return this[UserId];
        }
        internal Level New(long UserId = 0, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            Level Player = null;

            if (UserId == 0)
            {
                lock (this.Gate)
                {
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
                    char Letter = (char)Resources.Random.Next('A', 'Z');
                    Player.Avatar.Token += Letter;
                }
            }
            if (string.IsNullOrEmpty(Player.Avatar.Password))
            {
                for (int i = 0; i < 6; i++)
                {
                    char Letter = (char)Resources.Random.Next('A', 'Z');
                    char Number = (char)Resources.Random.Next('1', '9');
                    Player.Avatar.Password += Letter;
                    Player.Avatar.Password += Number;
                }
            }
            Player.JSON = Files.Home.Starting_Home;

            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Player.Add(new Database.Player
                            {
                                ID = Player.Avatar.UserId,
                                Data = JsonConvert.SerializeObject(Player.Avatar, this.Settings) + "#:#:#:#" + Player.JSON,
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
                        DBMS = DBMS.Mysql;

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

        internal void Save(Level Player, DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {

                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {

                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var Data = Database.Player.Find(Player.Avatar.UserId);

                            if (Data != null)
                            {
                                Data.Data = JsonConvert.SerializeObject(Player.Avatar, this.Settings) + "#:#:#:#" + Player.JSON;
                                    Database.SaveChanges();
                            }
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        Redis.Players.StringSet(Player.Avatar.UserId.ToString(), JsonConvert.SerializeObject(Player.Avatar, this.Settings) + "#:#:#:#" + Player.JSON, TimeSpan.FromHours(4));
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Player);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }
    }
}
