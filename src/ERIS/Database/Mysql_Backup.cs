namespace BL.Servers.BB.Database
{
    using System;
    using System.Data;
    using BL.Servers.BB.Core;
    using BL.Servers.BB.Extensions;
    using BL.Servers.BB.Logic.Enums;
    using BL.Servers.BB.Logic;
    using Newtonsoft.Json;
    using MySql.Data.MySqlClient;

    internal static class Mysql_Backup
    {
        internal static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.Auto,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            PreserveReferencesHandling  = PreserveReferencesHandling.All,   ReferenceLoopHandling   = ReferenceLoopHandling.Ignore,
            Formatting                  = Formatting.Indented,              Converters              = { new Utils.ArrayReferencePreservngConverter() },
        };
        internal static MySqlConnection Connections;
        internal static string Credentials;

        public static long GetClanSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ID), 0) FROM clan";
            long Seed = -1;


            using (var CMD = new MySqlCommand(SQL, Connections))
            {
                CMD.Prepare();
                Seed = Convert.ToInt64(CMD.ExecuteScalar());
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
                    Port = (uint) Utils.ParseConfigInt("MysqlPort"),
                    Pooling = false,
                    Database = Utils.ParseConfigString("MysqlDatabase"),
                    MinimumPoolSize = 1
                };

                if (!string.IsNullOrWhiteSpace(Utils.ParseConfigString("MysqlPassword")))
                {
                    builder.Password = Utils.ParseConfigString("MysqlPassword");
                }

                Credentials = builder.ToString();

                Connections = new MySqlConnection(Credentials);
                Connections.Open();


                using (MySqlCommand CMD = new MySqlCommand(SQL, Connections))
                {
                    CMD.Prepare();
                    Seed = Convert.ToInt64(CMD.ExecuteScalar());
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

        internal static Level Get(long UserId)
        {
            Level Player = null;

            const string SQL = "SELECT * FROM `player` WHERE  `ID` = @ID";
            
            using (MySqlCommand CMD = new MySqlCommand(SQL, Connections))
            {

                CMD.Parameters.AddWithValue("@ID", UserId);
                using (var reader = CMD.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            if (!string.IsNullOrEmpty((string) reader["Data"]))
                            {
                                string[] _Datas = Convert.ToString(reader["Data"])
                                    .Split(new string[1] {"#:#:#:#"}, StringSplitOptions.None);

                                if (!string.IsNullOrEmpty(_Datas[0]) && !string.IsNullOrEmpty(_Datas[1]))
                                {
                                    Player = new Level
                                    {
                                        Avatar = JsonConvert.DeserializeObject<Avatar>(_Datas[0], Settings)
                                    };
                                    Player.LoadFromJSON(_Datas[1]);
                                }
                            }
                        }
                    }
                }
            }
            return Player;
        }

        internal static void New(Level _Player)
        {
            const string SQL = "INSERT INTO player (ID, Data) VALUES (@ID, @Data)";

            using (MySqlCommand CMD = new MySqlCommand(SQL, Connections))
            {
                CMD.Parameters.AddWithValue("@ID", _Player.Avatar.UserId);
                CMD.Parameters.AddWithValue("@Data", JsonConvert.SerializeObject(_Player.Avatar, Settings) + "#:#:#:#" + _Player.SaveToJSON());
                CMD.Prepare();
                CMD.ExecuteNonQuery();
            }

        }
        internal static void Save(Level _Player)
        {
            const string SQL = "REPLACE INTO player (ID, Data) VALUES (@ID, @Data)";

            using (MySqlCommand CMD = new MySqlCommand(SQL, Connections))
            {
                CMD.Parameters.AddWithValue("@ID", _Player.Avatar.UserId);
                CMD.Parameters.AddWithValue("@Data",
                    JsonConvert.SerializeObject(_Player.Avatar, Settings) + "#:#:#:#" + _Player.SaveToJSON());
                CMD.Prepare();
                CMD.ExecuteNonQuery();
            }

        }

        /*internal static void Save(Clan _Clan)
        {
            const string SQL = "REPLACE INTO Clans (ID, Data) VALUES (@ID, @Data, )";

            using (MySqlConnection Conn = new MySqlConnection(Credentials))
            {
                Conn.Open();

                using (MySqlCommand CMD = new MySqlCommand(SQL, Conn))
                {
                    CMD.Parameters.AddWithValue("@ID", _Clan.Alliance);
                    CMD.Parameters.AddWithValue("@Data", _Clan.Serialize());
                    CMD.Parameters.AddWithValue("@Objects", _Clan.Objects.Serialize());
                    CMD.Prepare();
                    CMD.ExecuteNonQuery();
                }
            }
        }*/
    }
}
