using CRepublic.Royale.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Database;
using CRepublic.Royale.Logic.Enums;
using Clan = CRepublic.Royale.Logic.Clan;

namespace CRepublic.Royale.Core.Server_Components
{
    internal class Clans : Dictionary<long, Logic.Clan>
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
            this.Seed = Mysql_Backup.GetClanSeed() + 1;
        }

        internal void Add(Logic.Clan Clan)
        {
            lock (this.GateAdd)
            {
                if (this.ContainsKey(Clan.ClanID))
                {
                    this[Clan.ClanID] = Clan;
                    Clan = null;
                }
                else
                {
                    this.Add(Clan.ClanID, Clan);
                }
            }
        }

        internal void Remove(Logic.Clan Clan)
        {
            if (this.Remove(Clan.ClanID))
            {
                this.Save(Clan);
                Clan = null;
            }
        }

        internal Clan Get(long ClanID, Logic.Enums.DBMS DBMS = Constants.Database, bool Store = true)
        {
            if (!this.ContainsKey(ClanID))
            {
                Clan Clan = null;

                switch (DBMS)
                {
                    case Logic.Enums.DBMS.MySQL:
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var Data = Database.ClanDB.Find(ClanID);

                            if (!string.IsNullOrEmpty(Data?.Data))
                            {

                                Clan = new Logic.Clan(ClanID);

                                JsonConvert.PopulateObject(Data.Data, Clan, this.Settings);

                                if (Store)
                                {
                                    this.Add(Clan);
                                }

                            }
                        }
                        break;
                    case Logic.Enums.DBMS.Redis:
                        string Property = Redis.Clans.StringGet(ClanID.ToString()).ToString();

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
                    case Logic.Enums.DBMS.Both:
                        Clan = this.Get(ClanID, Logic.Enums.DBMS.Redis, Store);

                        if (Clan == null)
                        {
                            Clan = this.Get(ClanID, Logic.Enums.DBMS.MySQL, Store);
                            if (Clan != null)
                                this.Save(Clan, Logic.Enums.DBMS.Redis);

                        }
                        break;
                }
                return Clan;
            }
            return this[ClanID];
        }


        internal Logic.Clan New(long ClanId = 0, Logic.Enums.DBMS DBMS = Constants.Database, bool Store = true)
        {
            Logic.Clan Clan = null;

            if (ClanId == 0)
            {
                lock (this.Gate)
                {
                    Clan = new Logic.Clan(this.Seed++);
                }
            }
            else
            {
                Clan = new Logic.Clan(ClanId);
            }

            while (true)
            {
                switch (DBMS)
                {
                    case Logic.Enums.DBMS.MySQL:
                        {
                            using (MysqlEntities Database = new MysqlEntities())
                            {
                                Database.ClanDB.Add(new Database.Clan
                                {
                                    ID = Clan.ClanID,
                                    Data = JsonConvert.SerializeObject(Clan, this.Settings)
                                });

                                Database.SaveChangesAsync();
                            }

                            if (Store)
                            {
                                this.Add(Clan);
                            }
                            break;
                        }

                    case Logic.Enums.DBMS.Redis:
                        {
                            this.Save(Clan, DBMS);

                            if (Store)
                            {
                                this.Add(Clan);
                            }
                            break;
                        }

                    case Logic.Enums.DBMS.Both:
                        {
                            this.Save(Clan, DBMS);
                            DBMS = Logic.Enums.DBMS.MySQL;

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

        internal void Save(Logic.Clan Clan, Logic.Enums.DBMS DBMS = Constants.Database)
        {
            while (true)
            {
                switch (DBMS)
                {
                    case Logic.Enums.DBMS.MySQL:
                    {
                        using (MysqlEntities Database = new MysqlEntities())
                        {
                            var Data = Database.ClanDB.Find(Clan.ClanID);

                            if (Data != null)
                            {
                                Data.Data = JsonConvert.SerializeObject(Clan, this.Settings);
                                Database.SaveChanges();
                            }
                        }
                        break;
                    }

                    case Logic.Enums.DBMS.Redis:
                    {
                        Redis.Clans.StringSet(Clan.ClanID.ToString(), JsonConvert.SerializeObject(Clan, this.Settings), TimeSpan.FromHours(4));
                        break;
                    }

                    case Logic.Enums.DBMS.Both:
                    {
                        this.Save(Clan, DBMS.MySQL);
                        DBMS = Logic.Enums.DBMS.Redis;
                        continue;
                    }
                }
                break;
            }
        }
    }
}
