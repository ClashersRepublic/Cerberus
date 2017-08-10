using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Interface;
using CRepublic.Royale.Core.Resource;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Enums;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CRepublic.Royale.Core.Database
{
    internal class MySQL_V2
    {

        internal static string Credentials;

        internal static List<Level> GetPlayerViaFID(List<string> ID)
        {
            const string SQL = "SELECT ID FROM player WHERE FacebookID=@FacebookID";
            List<Level> Level = new List<Level>();
            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();
                foreach (var _ID in ID)
                {
                    using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                    {
                        CMD.Parameters.AddWithValue("@FacebookID", _ID);
                        CMD.Prepare();
                        long UserID = Convert.ToInt64(CMD.ExecuteScalar());
                        Level User = Players.Get(UserID);
                        if (User != null)
                            Level.Add(User);
                    }
                }
            }
            return Level;
        }

        internal static void GetAllSeed()
        {
            try
            {
                using (var Ctx = new MysqlEntities())
                    Credentials = Ctx.Database.Connection.ConnectionString;

                using (MySqlConnection Conn = new MySqlConnection(Credentials))
                {
                    Conn.Open();

                    using (MySqlCommand CMD = new MySqlCommand("SELECT coalesce(MAX(ID), 0) FROM player", Conn))
                    {
                        CMD.Prepare();
                        Players.Seed = Convert.ToInt64(CMD.ExecuteScalar()) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Control.Error("An exception occured when reconnecting to the MySQL Server.");
                Control.Error("Please check your database configuration");
                Exceptions.Log(ex, "An exception occured when reconnecting to the MySQL Server.");
                Console.ReadKey();
            }
        }

        internal static long GetClanSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ID), 0) FROM clan";
            long Seed = -1;

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();
                    Seed = Convert.ToInt64(CMD.ExecuteScalar());
                }
            }


            return Seed;
        }

        internal static long GetBattleSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ID), 0) FROM battle";
            long Seed = -1;

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();
                    Seed = Convert.ToInt64(CMD.ExecuteScalar());
                }
            }


            return Seed;
        }

        internal static List<long> GetTopPlayer()
        {
            const string SQL = "SELECT ID FROM player ORDER BY TROPHIES DESC LIMIT 100";
            List<long> Seed = new List<long>(100);

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();

                    var reader = CMD.ExecuteReader();
                    while (reader.Read())
                    {
                        Seed.Add(Convert.ToInt64(reader["ID"]));
                    }
                }
            }

            return Seed;
        }

        internal static long GetPlayerSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ID), 0) FROM player";
            long Seed = -1;

            using (MySqlConnection Connections = new MySqlConnection(Credentials))
            {
                Connections.Open();
                using (MySqlCommand CMD = new MySqlCommand(SQL, Connections))
                {
                    CMD.Prepare();
                    Seed = Convert.ToInt64(CMD.ExecuteScalar());
                }
            }
            return Seed;
        }
    }
}
