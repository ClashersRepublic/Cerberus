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
using Clan = BL.Servers.CoC.Logic.Clan;

namespace BL.Servers.CoC.Core
{
    internal class Clans : Dictionary<long, Clan>
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
            Converters = { new Utils.ArrayReferencePreservngConverter() },
        };


        internal long Seed;
        internal object Gate = new object();
        internal object GateAdd = new object();

        internal Clans()
        {
            this.Seed = MySQL_V2.GetClanSeed() + 1;
        }

        internal void Add(Clan Clan)
        {
            lock (this.GateAdd)
            {
                if (this.ContainsKey(Clan.Clan_ID))
                {
                    this[Clan.Clan_ID] = Clan;
                }
                else
                {
                    this.Add(Clan.Clan_ID, Clan);
                }
            }
        }

        internal void Remove(Clan Clan)
        {
            if (this.Remove(Clan.Clan_ID))
            {
                this.Save(Clan, Constants.Database);
            }
        }

        internal void Delete(Clan Clan, DBMS DBMS = DBMS.Mysql)
        {
            if (this.ContainsKey(Clan.Clan_ID))
            {
                this.Remove(Clan.Clan_ID);
            }

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
        }

        internal async Task<Clan> Get(long ClanID, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            if (!this.ContainsKey(ClanID))
            {
                Clan Clan = null;

                switch (DBMS)
                {
                    case DBMS.Mysql:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                           var Data = await Database.Clan.FindAsync(ClanID);

                            if (!string.IsNullOrEmpty(Data?.Data))
                            {

                                Clan = new Clan(ClanID);

                                JsonConvert.PopulateObject(Data.Data, Clan, this.Settings);

                                if (Store)
                                {
                                    this.Add(Clan);
                                }

                            }
                        }
                        break;

                    case DBMS.Redis:
                        string Property = await Redis.Clans.StringGetAsync(ClanID.ToString());

                        if (!string.IsNullOrEmpty(Property))
                        {
                            Clan = new Clan(ClanID);

                            JsonConvert.PopulateObject(Property, Clan, this.Settings);

                            if (Store)
                            {
                                this.Add(Clan);
                            }

                        }
                        break;

                    case DBMS.Both:
                        Clan = await this.Get(ClanID, DBMS.Redis, Store);

                        if (Clan == null)
                        {
                            Clan = await this.Get(ClanID, DBMS.Mysql, Store);
                            if (Clan != null)
                                this.Save(Clan, DBMS.Redis);

                        }
                        break;
                }
                return Clan;
            }
            return this[ClanID];
        }

        internal Clan New(long ClanId = 0, DBMS DBMS = DBMS.Mysql, bool Store = true)
        {
            Clan Clan = null;

            if (ClanId == 0)
            {
                lock (this.Gate)
                {
                    Clan = new Clan(this.Seed++);
                }
            }
            else
            {
                Clan = new Clan(ClanId);
            }

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
                            this.Save(Clan, DBMS);

                            if (Store)
                            {
                                this.Add(Clan);
                            }
                            break;
                        }

                    case DBMS.Both:
                        {
                            this.Save(Clan, DBMS);
                            DBMS = DBMS.Mysql;

                            if (Store)
                            {
                                this.Add(Clan);
                            }

                            continue;
                        }
                }
                break;
            }

            return Clan;
        }

        internal void Save(Clan Clan, DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {

                switch (DBMS)
                {
                    case DBMS.Mysql:
                        {

                            using (MysqlEntities Database = new MysqlEntities())
                            {
                                var Data = Database.Clan.Find(Clan.Clan_ID);

                                if (Data != null)
                                {
                                    Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);
                                    Database.SaveChanges();
                                }
                            }
                            break;
                        }

                    case DBMS.Redis:
                        {
                            Redis.Clans.StringSet(Clan.Clan_ID.ToString(), JsonConvert.SerializeObject(Clan, this.Settings), TimeSpan.FromHours(4));
                            break;
                        }

                    case DBMS.Both:
                        {
                            this.Save(Clan);
                            DBMS = DBMS.Redis;
                            continue;
                        }
                }
                break;
            }
        }

        internal async void Save(List<Clan> Clans, DBMS DBMS = DBMS.Mysql)
        {
            while (true)
            {

                switch (DBMS)
                {
                    case DBMS.Mysql:
                    {
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            foreach (var Clan in Clans)
                            {
                                lock (Clan)
                                {
                                    var Data = Database.Clan.Find(Clan.Clan_ID);

                                    if (Data != null)
                                    {
                                        Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);
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
                        foreach (var Clan in Clans)
                        {
                            await Redis.Clans.StringSetAsync(Clan.Clan_ID.ToString(),
                                JsonConvert.SerializeObject(Clan, this.Settings), TimeSpan.FromHours(4));
                        }
                        break;
                    }

                    case DBMS.Both:
                    {
                        this.Save(Clans);
                        DBMS = DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }
    }
}