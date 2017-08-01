using Savage.Magic;
using Savage.Magic.Logic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace Savage.Magic.Core.Database
{
    internal class MySQL
    {
        internal static readonly string Credentials = "server=localhost;user id={0}{1};CharSet=utf8mb4;persistsecurityinfo=True;database=ucsdb";

        static MySQL()
        {
            var id = Utils.ParseConfigString("id");
            var pwd = Utils.ParseConfigString("pwd");

            if (pwd != string.Empty)
                pwd = ";pwd=" + pwd;

            Credentials = string.Format(Credentials, id, pwd);
        }

        internal static long GetPlayerSeed()
        {
            const string SQL = "SELECT coalesce(MAX(PlayerId), 0) FROM player";
            long Seed = -1;

            using (var conn = new MySqlConnection(Credentials))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(SQL, conn))
                {
                    cmd.Prepare();
                    Seed = (long)cmd.ExecuteScalar();
                    Logger.Say("Successfully retrieved max player ID: " + Seed + " player(s).");
                }
            }

            return Seed;
        }

        internal static long GetAllianceSeed()
        {
            const string SQL = "SELECT coalesce(MAX(ClanId), 0) FROM clan";
            long Seed = -1;
               
            using (var conn = new MySqlConnection(Credentials))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(SQL, conn))
                {
                    cmd.Prepare();
                    Seed = (long)cmd.ExecuteScalar();
                    Logger.Say("Successfully retrieved max alliance ID: " + Seed + " alliance(s).");
                }
            }

            return Seed;
        }
    }
}