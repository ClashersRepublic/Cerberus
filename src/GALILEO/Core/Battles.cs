using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using Newtonsoft.Json;
using Battle = BL.Servers.CoC.Logic.Battle;

namespace BL.Servers.CoC.Core
{
    internal class Battles : Dictionary<long, Battle>
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

        internal Battles()
        {
            this.Seed = MySQL_V2.GetBattleSeed() + 1;
        }

        internal void Add(Battle Battle)
        {
            lock (this.GateAdd)
            {
                if (this.ContainsKey(Battle.Battle_ID))
                {
                    this[Battle.Battle_ID] = Battle;
                }
                else
                {
                    this.Add(Battle.Battle_ID, Battle);
                }
            }
        }

        internal async Task<Battle> Get(long _BattleID, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            if (!this.ContainsKey(_BattleID))
            {
                Battle _Battle = null;

                switch (DBMS)
                {
                    case DBMS.Mysql:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Battle Data = await Database.Battle.FindAsync(_BattleID);

                            if (!string.IsNullOrEmpty(Data?.Data))
                            {
                                _Battle = JsonConvert.DeserializeObject<Battle>(Data.Data, this.Settings);

                                if (Store)
                                    {
                                        this.Add(_Battle);
                                    }
                                }
                            }
                        break;
                    case DBMS.Redis:
                        string Property = await Redis.Battles.StringGetAsync(_BattleID.ToString());

                        if (!string.IsNullOrEmpty(Property))
                        {

                            _Battle = JsonConvert.DeserializeObject<Battle>(Property, this.Settings);

                            if (Store)
                            {
                                this.Add(_Battle);
                            }
                        }
                        break;
                    case DBMS.Both:
                        _Battle = await this.Get(_BattleID, DBMS.Redis, Store);

                        if (_Battle == null)
                        {
                            _Battle = await this.Get(_BattleID, DBMS.Mysql, Store);
                            if (_Battle != null)
                                this.Save(_Battle, DBMS.Redis);

                        }
                        break;
                }
                return _Battle;
            }
            return this[_BattleID];
        }

        internal Battle New(Level _Attacker, Level _Defender, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            Battle _Battle = null;

            lock (this.Gate)
            {
                _Battle = new Battle(this.Seed++, _Attacker, _Defender);
            }
            _Attacker.Avatar.Battle_ID = _Battle.Battle_ID;

            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Battle.Add(new Database.Battle
                            {
                                ID = _Battle.Battle_ID,
                                Data = JsonConvert.SerializeObject(_Battle, this.Settings)
                            });

                            Database.SaveChanges();
                        }

                        if (Store)
                        {
                            this.Add(_Battle);
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        this.Save(_Battle, DBMS);

                        if (Store)
                        {
                            this.Add(_Battle);
                        }
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(_Battle, DBMS);
                        DBMS = DBMS.Mysql;

                        if (Store)
                        {
                            this.Add(_Battle);
                        }

                        continue;
                    }
                }
                break;
            }

            return _Battle;
        }

        internal void Save(Battle _Battle, DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {

                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var Data = Database.Battle.Find(_Battle.Battle_ID);

                            if (Data != null)
                            {
                                Data.Data = JsonConvert.SerializeObject(_Battle, this.Settings);
                                Database.SaveChanges();
                            }
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        Redis.Battles.StringSet(_Battle.Battle_ID.ToString(), JsonConvert.SerializeObject(_Battle, this.Settings), TimeSpan.FromHours(4));
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(_Battle);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }
        internal async void Save(List<Battle> Battles, DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {

                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            foreach (var Battle in Battles)
                            {
                                lock (Battle)
                                {
                                    var Data = Database.Battle.Find(Battle.Battle_ID);

                                    if (Data != null)
                                    {
                                        Data.Data = JsonConvert.SerializeObject(Battle, this.Settings);
                                        Database.SaveChanges();
                                    }
                                }
                            }
                            await Database.SaveChangesAsync();
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        foreach (var Battle in Battles)
                        {
                            await Redis.Battles.StringSetAsync(Battle.Battle_ID.ToString(),
                                JsonConvert.SerializeObject(Battle, this.Settings), TimeSpan.FromHours(4));
                        }
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Battles);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }

    }
}
