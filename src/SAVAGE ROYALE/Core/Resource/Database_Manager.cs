using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Database;
using CRepublic.Royale.Core.Interface;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Structure.Slots;
using Newtonsoft.Json;
using Player = CRepublic.Royale.Core.Database.Player;

namespace CRepublic.Royale.Core.Resource
{
    internal class Database_Manager
    {
        public static Database_Manager Instance { get; } = new Database_Manager();
        
        internal JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,                        MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,             NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
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

        internal Level FetchLevel(long userId)
        {
            var level = default(Level);
            try
            {
                using (var Ctx = new MysqlEntities())
                {
                    var Player = Ctx.Player.Find(userId);
                    if (Player != null)
                    {
                        level = new Level
                        {
                            Avatar = JsonConvert.DeserializeObject<Logic.Player>(Player.Avatar, this.Settings),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.Log(ex, $"Exception while trying to get a level {userId} from the database.");
                level = null;
            }
            return level;
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
                        }
                    }
                }
                await Ctx.BulkSaveChangesAsync(false);
            }
        }

    }
}
