using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Core.Interface;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Structure.Slots;
using Newtonsoft.Json;
using Player = CRepublic.Magic.Core.Database.Player;

namespace CRepublic.Magic.Core.Resource
{
    internal class Database_Manager
    {
        public static Database_Manager Instance { get; } = new Database_Manager();
        
        internal JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
        };

        public Database_Manager()
        {
            // Let them know we like it 1 instance.
            if (Instance != null)
                throw new InvalidOperationException("DatabaseManager is a singleton.");
        }

        internal void AddLevel(Level level)
        {
            using (var Ctx = new MysqlEntities())
            {
                var player = new Player
                {
                    ID = level.Avatar.UserId,
                    Avatar = JsonConvert.SerializeObject(level.Avatar, this.Settings),
                };
                Ctx.Player.Add(player);
                Ctx.SaveChanges();
            }
        }

        internal async void Save(List<Level> Levels)
        {
            using (var Ctx = new MysqlEntities())
            {
                foreach (Level pl in Levels)
                {
                    lock (pl)
                    {
                        Player p = Ctx.Player.Find(pl.Avatar.UserId);
                        if (p != null)
                        {
                            p.Avatar = JsonConvert.SerializeObject(pl.Avatar, this.Settings);
                            p.Village = Home.Starting_Home;
                            p.Trophies = 0;
                            p.FacebookID = "Hi";
                        }
                    }
                }
                await Ctx.BulkSaveChangesAsync();
            }
        }
    }
}
