using Magic.ClashOfClans.Core.Database;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Database;
using Magic.ClashOfClans.Logic;
using Magic.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using static Magic.ClashOfClans.Core.Logger;

namespace Magic.ClashOfClans.Core
{
    // This thing should have been a static class since the beginning.

    internal class DatabaseManager
    {
        public static DatabaseManager Instance => s_singleton;

        private static DatabaseManager s_singleton = new DatabaseManager();

        public DatabaseManager()
        {
            // Let them know we like it 1 instance.
            if (s_singleton != null)
                throw new InvalidOperationException("DatabaseManager is a singleton.");

            _connectionString = ConfigurationManager.AppSettings["databaseConnectionName"];
        }

        private readonly string _connectionString;

        public long GetMaxAllianceId()
        {
            try
            {
                return MySQL.GetAllianceSeed();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to retrieve max alliance ID; check config.");
            }
            return -1;
        }

        public long GetMaxPlayerId()
        {
            try
            {
                return MySQL.GetPlayerSeed();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to retrieve max player ID; check config.");
            }
            return -1;
        }

        public void CreateLevel(Level level)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var newPlayer = new player
                    {
                        PlayerId = level.Avatar.Id,
                        AccountStatus = level.AccountStatus,
                        AccountPrivileges = level.AccountPrivileges,
                        LastUpdateTime = level.Time,
                        IPAddress = level.IPAddress,

                        Avatar = level.Avatar.SaveToJson(),
                        GameObjects = level.SaveToJson()
                    };

                    ctx.player.Add(newPlayer);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to create a new player account in database.");
            }
        }

        public void CreateAlliance(Alliance alliance)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var newClan = new clan
                    {
                        ClanId = alliance.AllianceId,
                        LastUpdateTime = DateTime.Now,
                        Data = alliance.SaveToJson()
                    };

                    ctx.clan.Add(newClan);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to create a new alliance in database.");
            }
        }

        public Level GetLevel(long userId)
        {
            var level = default(Level);
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var player = ctx.player.Find(userId);
                    if (player != null)
                    {
                        level = new Level();
                        level.AccountStatus = player.AccountStatus;
                        level.AccountPrivileges = player.AccountPrivileges;
                        level.Time = player.LastUpdateTime;

                        // Load JSON data.
                        level.Avatar.LoadFromJson(player.Avatar);
                        level.LoadFromJson(player.GameObjects);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while trying to get a level {userId} from the database.");

                // In case the level instance was already created before the exception.
                level = null;
            }
            return level;
        }

        public Alliance GetAlliance(long allianceId)
        {
            var alliance = default(Alliance);
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var clan = ctx.clan.Find(allianceId);
                    if (clan != null)
                    {
                        alliance = new Alliance();
                        alliance.LoadFromJson(clan.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to get alliance from database.");

                // In case it fails to LoadFromJSON.
                alliance = null;
            }

            return alliance;
        }

        // Used whenever the clients searches for an alliance however no alliances is loaded in memory.
        public List<Alliance> GetAllAlliances()
        {
            var alliances = new List<Alliance>();
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    var clans = ctx.clan;
                    Parallel.ForEach(clans, c =>
                    {
                        Alliance alliance = new Alliance();
                        alliance.LoadFromJson(c.Data);
                        alliances.Add(alliance);
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while trying to get all alliances from database.");
            }

            return alliances;
        }

        public void RemoveAlliance(Alliance alliance)
        {
            long allianceId = alliance.AllianceId;
            using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
            {
                var clan = ctx.clan.Find(allianceId);

                ctx.clan.Remove(clan);
                ctx.SaveChanges();
            }

            ObjectManager.RemoveInMemoryAlliance(allianceId);
        }

        public async Task Save(Alliance alliance)
        {
            using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
            {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Configuration.ValidateOnSaveEnabled = false;
                clan c = await ctx.clan.FindAsync((int)alliance.AllianceId);
                if (c != null)
                {
                    c.LastUpdateTime = DateTime.Now;
                    c.Data = alliance.SaveToJson();
                    ctx.Entry(c).State = EntityState.Modified;
                }
                //else
                //{
                //    context.clan.Add(
                //        new clan
                //        {
                //            ClanId = alliance.GetAllianceId(),
                //            LastUpdateTime = DateTime.Now,
                //            Data = alliance.SaveToJSON()
                //        });
                //}
                await ctx.SaveChangesAsync();
            }
        }

        public void Save(Level level)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    ctx.Configuration.AutoDetectChangesEnabled = false;

                    var player = ctx.player.Find(level.Avatar.Id);
                    if (player != null)
                    {
                        player.LastUpdateTime = level.Time;
                        player.AccountStatus = level.AccountStatus;
                        player.AccountPrivileges = level.AccountPrivileges;
                        player.IPAddress = level.IPAddress;
                        player.Avatar = level.Avatar.SaveToJson();
                        player.GameObjects = level.SaveToJson();

                        ctx.Entry(player).State = EntityState.Modified;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionLogger.Log(ex, $"Exception while trying to save a level {level.Avatar.Id} from the database. Check error for more information.");
                foreach (var entry in ex.EntityValidationErrors)
                {
                    foreach (var errs in entry.ValidationErrors)
                        Logger.Error($"{errs.PropertyName}:{errs.ErrorMessage}");
                }
                throw;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, $"Exception while trying to save a level {level.Avatar.Id} from the database.");
                throw;
            }
        }

        public async Task Save(List<Level> levels)
        {
            try
            {
                using (var ctx = new ucsdbEntities(_connectionString))
                {
                    ctx.Configuration.AutoDetectChangesEnabled = false;
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    foreach (Level pl in levels)
                    {
                        lock (pl)
                        {
                            player p = ctx.player.Find(pl.Avatar.Id);
                            if (p != null)
                            {
                                p.LastUpdateTime = pl.Time;
                                p.AccountStatus = pl.AccountStatus;
                                p.AccountPrivileges = pl.AccountPrivileges;
                                p.IPAddress = pl.IPAddress;
                                p.Avatar = pl.Avatar.SaveToJson();
                                p.GameObjects = pl.SaveToJson();
                                ctx.Entry(p).State = EntityState.Modified;
                            }
                            //else
                            //    context.player.Add(
                            //        new player
                            //        {
                            //            PlayerId = pl.GetPlayerAvatar().GetId(),
                            //            AccountStatus = pl.GetAccountStatus(),
                            //            AccountPrivileges = pl.GetAccountPrivileges(),
                            //            LastUpdateTime = pl.GetTime(),
                            //            IPAddress = pl.GetIPAddress(),
                            //            Avatar = pl.GetPlayerAvatar().SaveToJSON(),
                            //            GameObjects = pl.SaveToJSON()
                            //        });
                        }
                    }
                    await ctx.SaveChangesAsync();
                }
            }
            catch
            {
                // 0 Actual fucks given.
            }
        }

        public async Task Save(List<Alliance> alliances)
        {
            try
            {
                using (ucsdbEntities ctx = new ucsdbEntities(_connectionString))
                {
                    ctx.Configuration.AutoDetectChangesEnabled = false;
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    foreach (Alliance alliance in alliances)
                    {
                        lock (alliance)
                        {
                            clan c = ctx.clan.Find((int)alliance.AllianceId);
                            if (c != null)
                            {
                                c.LastUpdateTime = DateTime.Now;
                                c.Data = alliance.SaveToJson();
                                ctx.Entry(c).State = EntityState.Modified;
                            }
                            //else
                            //{
                            //    context.clan.Add(
                            //        new clan
                            //        {
                            //            ClanId = alliance.GetAllianceId(),
                            //            LastUpdateTime = DateTime.Now,
                            //            Data = alliance.SaveToJSON(),

                            //        });
                            //}
                        }
                    }
                    await ctx.SaveChangesAsync();
                }
            }
            catch
            {
                // 0 Actual fucks given.
            }
        }
    }
}
