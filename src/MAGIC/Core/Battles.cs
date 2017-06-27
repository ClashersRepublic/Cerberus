using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using Newtonsoft.Json;
using Battle = CRepublic.Magic.Logic.Battle;

namespace CRepublic.Magic.Core
{
    internal class Battles : ConcurrentDictionary<long, Battle>
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
                    this.TryAdd(Battle.Battle_ID, Battle);
                }
            }
        }

        internal Battle Get(long _BattleID, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            if (!this.ContainsKey(_BattleID))
            {
                Battle _Battle = null;

                switch (DBMS)
                {
                    case DBMS.Mysql:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Battle Data = Database.Battle.Find(_BattleID);

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
                        string Property = Redis.Battles.StringGet(_BattleID.ToString());

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
                        _Battle = this.Get(_BattleID, DBMS.Redis, Store);

                        if (_Battle == null)
                        {
                            _Battle = this.Get(_BattleID, DBMS.Mysql, Store);
                            if (_Battle != null)
                                this.Save(_Battle, DBMS.Redis).Wait();

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
                        this.Save(_Battle, DBMS.Redis).Wait();

                        if (Store)
                        {
                            this.Add(_Battle);
                        }
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(_Battle, DBMS.Mysql).Wait();
                        DBMS = DBMS.Redis;

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

        internal async Task Save(Battle _Battle, DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {

                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            Database.Configuration.AutoDetectChangesEnabled = false;
                            Database.Configuration.ValidateOnSaveEnabled = false;
                            var Data = Database.Battle.Find(_Battle.Battle_ID);

                            if (Data != null)
                            {
                                Data.Data = JsonConvert.SerializeObject(_Battle, this.Settings);
                                Database.Entry(Data).State = EntityState.Modified;
                                }
                            await Database.SaveChangesAsync();
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        await Redis.Battles.StringSetAsync(_Battle.Battle_ID.ToString(), JsonConvert.SerializeObject(_Battle, this.Settings), TimeSpan.FromHours(4));
                        break;
                    }

                    case DBMS.Both:
                    {
                        await this.Save(_Battle);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }

        internal async Task Save(DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                        {

                            using (MysqlEntities Database = new MysqlEntities())
                            {
                                Database.Configuration.AutoDetectChangesEnabled = false;
                                Database.Configuration.ValidateOnSaveEnabled = false;
                                foreach (var Battle in this.Values.ToList())
                                {
                                    lock (Battle)
                                    {
                                        var Data = Database.Battle.Find(Battle.Battle_ID);

                                        if (Data != null)
                                        {
                                            Data.Data = JsonConvert.SerializeObject(Battle, this.Settings);
                                            Database.Entry(Data).State = EntityState.Modified;
                                        }
                                    }
                                }
                                await Database.SaveChangesAsync();
                            }
                            break;
                        }

                    case DBMS.Redis:
                        {
                            foreach (var Battle in this.Values.ToList())
                            {
                                await Redis.Battles.StringSetAsync(Battle.Battle_ID.ToString(),
                                    JsonConvert.SerializeObject(Battle, this.Settings), TimeSpan.FromHours(4));
                            }
                            break;
                        }

                    case DBMS.Both:
                        {
                            await this.Save();
                            DBMS = DBMS.Redis;
                            continue;
                        }
                }
                break;
            }
        }
    }
}
