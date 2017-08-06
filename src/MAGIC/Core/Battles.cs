using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using Newtonsoft.Json;
using NLog;
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

        internal Battles()
        {
        }

        internal void Add(Battle Battle)
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

        internal Battle Get(long _BattleID, bool Store = true)
        {
            if (!this.ContainsKey(_BattleID))
            {
                Battle _Battle = null;

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
                return _Battle;
            }
            return this[_BattleID];
        }

        internal Battle New(Level _Attacker, Level _Defender, bool Store = true)
        {
            _Attacker.Avatar.Battle_ID = this.Seed;
            var _Battle = new Battle(this.Seed++, _Attacker, _Defender);

            if (Store)
            {
                this.Add(_Battle);
            }

            using (MysqlEntities Database = new MysqlEntities())
            {
                Database.Battle.Add(new Database.Battle
                {
                    ID = _Battle.Battle_ID,
                    Data = JsonConvert.SerializeObject(_Battle, this.Settings)
                });

                Database.SaveChanges();
            }
           
            return _Battle;
        }

        internal void Save(Battle _Battle)
        {
            try
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
                    Database.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                Loggers.Log(
                    ex +
                    $" Exception while trying to save a battle {_Battle.Battle_ID} to the database. Check error for more information.");
                foreach (var entry in ex.EntityValidationErrors)
                {
                    foreach (var errs in entry.ValidationErrors)
                        Loggers.Log($"{errs.PropertyName}:{errs.ErrorMessage}");
                }
                throw;
            }
            catch (Exception ex)
            {
                Loggers.Log(ex + $" Exception while trying to save a battle {_Battle.Battle_ID} to the database.");
                throw;
            }
        }

        internal async Task Save()
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
                await Database.BulkSaveChangesAsync();
            }
            /*
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
                                Redis.Battles.StringSet(Battle.Battle_ID.ToString(),
                                    JsonConvert.SerializeObject(Battle, this.Settings), TimeSpan.FromHours(4));
                            }
                            break;
                        }

                    case DBMS.Both:
                        {
                            await this.Save(DBMS.Mysql);
                            DBMS = DBMS.Redis;
                            continue;
                        }
                }
                break;
            }*/
        }
    }
}
