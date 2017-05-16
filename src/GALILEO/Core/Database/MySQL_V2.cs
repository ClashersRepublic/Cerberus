using System;
using System.Collections.Generic;   
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using MySql.Data.MySqlClient;

namespace BL.Servers.CoC.Core.Database
{
    internal class MySQL_V2
    {
        internal static string Credentials;

        internal static Level GetPlayerViaFID(long ID)
        {
            const string SQL = "SELECT coalesce(MAX(ID), 0) FROM clan";
            Level Level = null;
            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();
                    Level = Resources.Players.Get(Convert.ToInt64(CMD.ExecuteScalar()), Constants.Database, false);
                }

            }
            return Level;

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
                Conn.Close();
            }


            return Seed;
        }

        internal static List<long> GetTopPlayer()
        {
            const string SQL = "SELECT ID FROM player ORDER BY TROPHIES DESC LIMIT 100";
            List<long> Seed = new List<long>(150);

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Prepare();

                    MySqlDataReader reader = CMD.ExecuteReader();
                    while (reader.Read())
                    {
                        Seed.Add(Convert.ToInt64(reader["ID"]));
                    }
                }
                Conn.Close();
            }

            return Seed;
        }

        internal static long GetPlayerSeed()
        {
            try
            {
                const string SQL = "SELECT coalesce(MAX(ID), 0) FROM player";
                long Seed = -1;

                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
                {
                    Server = Utils.ParseConfigString("MysqlIPAddress"),
                    UserID = Utils.ParseConfigString("MysqlUsername"),
                    Port = (uint)Utils.ParseConfigInt("MysqlPort"),
                    Pooling = false,
                    Database = Utils.ParseConfigString("MysqlDatabase"),
                    MinimumPoolSize = 1
                };

                if (!string.IsNullOrWhiteSpace(Utils.ParseConfigString("MysqlPassword")))
                {
                    builder.Password = Utils.ParseConfigString("MysqlPassword");
                }

                Credentials = builder.ToString();
                using (MySqlConnection Connections = new MySqlConnection(Credentials))
                {
                    Connections.Open();
                    using (MySqlCommand CMD = new MySqlCommand(SQL, Connections))
                    {
                        CMD.Prepare();
                        Seed = Convert.ToInt64(CMD.ExecuteScalar());
                    }
                    Connections.Close();
                }
                return Seed;
            }
            catch (Exception ex)
            {
                Loggers.Log("An exception occured when reconnecting to the MySQL Server.", true, Defcon.ERROR);
                Loggers.Log("Please check your database configuration!", true, Defcon.ERROR);
                Loggers.Log(ex.Message, true, Defcon.ERROR);
                Console.ReadKey();
            }
            return 0;
        }

    }
}
