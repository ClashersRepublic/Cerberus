namespace CRepublic.Royale.Database
{
    using System;
    using System.Data;
    using CRepublic.Royale.Core;
    using CRepublic.Royale.Extensions;
    using CRepublic.Royale.Logic.Enums;
    using CRepublic.Royale.Logic;
    using Newtonsoft.Json;
    using MySql.Data.MySqlClient;
    using System.Runtime.InteropServices;

    internal class Mysql_Backup : IDisposable
    {
        internal bool _Disposed = false;

        internal JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.Auto,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            PreserveReferencesHandling  = PreserveReferencesHandling.All,   ReferenceLoopHandling   = ReferenceLoopHandling.Ignore,
            Formatting                  = Formatting.Indented,              Converters              = { new Utils.ArrayReferencePreservngConverter() },
        };
        internal MySqlConnection Connections;
        internal static string Credentials;

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

        ~Mysql_Backup()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (!_Disposed)
            {
                if (_Disposed)
                {
                    
                }

                Connections.Dispose();
            }

            _Disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
