using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Extensions;
using Newtonsoft.Json;
using CRepublic.Magic.Logic.Enums;
using Clan = CRepublic.Magic.Logic.Clan;

namespace CRepublic.Magic.Core
{
    internal class Clans : ConcurrentDictionary<long, Clan>
    {
        internal JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            Converters = {new Utils.ArrayReferencePreservngConverter()},
        };


        internal long Seed;

        internal Clans()
        {
        }

        internal void Add(Clan Clan)
        {
            if (this.ContainsKey(Clan.Clan_ID))
            {
                this[Clan.Clan_ID] = Clan;
            }
            else
            {
                this.TryAdd(Clan.Clan_ID, Clan);

            }
        }

        internal void Remove(Clan Clan)
        {
            if (this.TryRemove(Clan.Clan_ID))
            {
                this.Save(Clan);
            }
        }

        [Obsolete]
        internal void Delete(Clan Clan)
        {
            if (this.ContainsKey(Clan.Clan_ID))
            {
                this.TryRemove(Clan.Clan_ID);
            }

            using (MysqlEntities Database = new MysqlEntities())
            {
                var index = Database.Clan.Find(Clan.Clan_ID);
                if (index != null)
                    Database.Clan.Remove(index);
                Database.SaveChanges();
            }

            #region Old

            /*
            while (true)
            {
                switch (DBMS)
                {
                    case DBMS.Mysql:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var index = Database.Clan.Find(Clan.Clan_ID);
                            if (index != null)
                            Database.Clan.Remove(index);
                            Database.SaveChanges();
                        }
                        break;
                    case DBMS.Redis:
                        Redis.Clans.KeyDelete(Clan.Clan_ID.ToString());
                        break;
                    case DBMS.Both:
                        this.Delete(Clan);
                        DBMS = DBMS.Redis;
                        continue;
                }
        }
                */

            #endregion
        }

        internal Clan Get(long ClanID, bool Store = true)
        {
            if (!this.ContainsKey(ClanID))
            {
                Clan Clan = null;

                using (MysqlEntities Database = new MysqlEntities())
                {
                    var Data = Database.Clan.Find(ClanID);

                    if (!string.IsNullOrEmpty(Data?.Data))
                    {
                        Clan = JsonConvert.DeserializeObject<Clan>(Data.Data, this.Settings);

                        if (Store)
                        {
                            this.Add(Clan);
                        }

                    }
                }
                #region Old

                /*
                switch (DBMS)
                {
                    case DBMS.Mysql:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var Data = Database.Clan.Find(ClanID);

                            if (!string.IsNullOrEmpty(Data?.Data))
                            {
                                Clan = JsonConvert.DeserializeObject<Clan>(Data.Data, this.Settings);

                                if (Store)
                                {
                                    this.Add(Clan);
                                }

                            }
                        }
                        break;

                    case DBMS.Redis:
                        string Property = Redis.Clans.StringGet(ClanID.ToString());

                        if (!string.IsNullOrEmpty(Property))
                        {
                            Clan = JsonConvert.DeserializeObject<Clan>(Property, this.Settings);

                            if (Store)
                            {
                                this.Add(Clan);
                            }

                        }
                        break;

                    case DBMS.Both:
                        Clan = this.Get(ClanID, DBMS.Redis, Store);

                        if (Clan == null)
                        {
                            Clan = this.Get(ClanID, DBMS.Mysql, Store);
                            if (Clan != null)
                                Redis.Clans.StringSet(Clan.Clan_ID.ToString(),
                                    JsonConvert.SerializeObject(Clan, this.Settings), TimeSpan.FromHours(4));

                        }
                        break;
                }*/
                #endregion
                return Clan;
            }
            return this[ClanID];
        }

        internal Clan New(long ClanId = 0, bool Store = true)
        {
            var Clan = ClanId == 0 ? new Clan(this.Seed++) : new Clan(ClanId);

            if (Store)
            {
                this.Add(Clan);
            }

            using (MysqlEntities Database = new MysqlEntities())
            {
                Database.Clan.Add(new Database.Clan
                {
                    ID = Clan.Clan_ID,
                    Data = JsonConvert.SerializeObject(Clan, this.Settings)
                });

                Database.SaveChanges();
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
                            Database.Clan.Add(new Database.Clan
                            {
                                ID = Clan.Clan_ID,
                                Data = JsonConvert.SerializeObject(Clan, this.Settings)
                            });

                            Database.SaveChanges();
                        }

                        if (Store)
                        {
                            this.Add(Clan);
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        this.Save(Clan, DBMS.Redis);

                        if (Store)
                        {
                            this.Add(Clan);
                        }
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Clan, DBMS.Mysql);
                        DBMS = DBMS.Redis;

                        if (Store)
                        {
                            this.Add(Clan);
                        }

                        continue;
                    }
                }
                break;
            }*/

            return Clan;
        }

        internal void Save(Clan Clan)
        {
            using (MysqlEntities Database = new MysqlEntities())
            {
                Database.Configuration.AutoDetectChangesEnabled = false;
                Database.Configuration.ValidateOnSaveEnabled = false;
                var Data = Database.Clan.Find(Clan.Clan_ID);

                if (Data != null)
                {
                    Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);
                    Database.Entry(Data).State = EntityState.Modified;
                }

                Database.SaveChanges();
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
                            var Data = Database.Clan.Find(Clan.Clan_ID);

                            if (Data != null)
                            {
                                Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);
                                Database.Entry(Data).State = EntityState.Modified;
                            }

                            Database.SaveChanges();
                        }
                        break;
                    }

                    case DBMS.Redis:
                    {
                        Redis.Clans.StringSet(Clan.Clan_ID.ToString(), JsonConvert.SerializeObject(Clan, this.Settings),
                            TimeSpan.FromHours(4));
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Clan, DBMS.Mysql);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }*/
        }

        internal async Task Save()
        {
            using (MysqlEntities Database = new MysqlEntities())
            {
                Database.Configuration.AutoDetectChangesEnabled = false;
                Database.Configuration.ValidateOnSaveEnabled = false;
                foreach (var Clan in this.Values.ToList())
                {
                    lock (Clan)
                    {
                        var Data = Database.Clan.Find(Clan.Clan_ID);

                        if (Data != null)
                        {
                            Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);

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
                            foreach (var Clan in this.Values.ToList())
                            {
                                lock (Clan)
                                {
                                    var Data = Database.Clan.Find(Clan.Clan_ID);

                                    if (Data != null)
                                    {
                                        Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);

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
                        foreach (var Clan in this.Values.ToList())
                        {
                            Redis.Clans.StringSet(Clan.Clan_ID.ToString(),
                                JsonConvert.SerializeObject(Clan, this.Settings), TimeSpan.FromHours(4));
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