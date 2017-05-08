using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic.Enums;
using MySql.Data.MySqlClient;

namespace BL.Servers.CoC.Core.Database
{
    internal class MySQL_V2
    {
        internal static string Credentials;

        public static long GetClanSeed()
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
        public static long GetPlayerSeed()
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
