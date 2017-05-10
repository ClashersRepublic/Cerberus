using System;
using System.Configuration;
using System.Reflection;
using BL.Proxy.Lyra.Consoles;

namespace BL.Proxy.Lyra.Configuration
{
    internal class Config
    {
        private static readonly System.Configuration.Configuration Configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        public static string Get(string key)
        {
            return Configuration.AppSettings.Settings[key].Value;
        }

        public static void Set(string key, string value)
        {
            Configuration.AppSettings.Settings[key].Value = value;
            Configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }


        public static bool JSON_Logging
        {
            get
            {
                bool ret = false;
                Boolean.TryParse(Get("JSON_Logging"), out ret);
                return ret;
            }
        }
        public static string Host => Get("Host").ToLower();

        public static Game Game
        {
            get
            {
                if (Host == "game.clashofclans.com" || Host == "gamea.clashofclans.com")
                {
                    return Game.CLASH_OF_CLANS;
                }
                else if (Host == "game.clashroyaleapp.com")
                {
                    return Game.CLASH_ROYALE;
                }
                else if(Host == "game.boombeachgame.com")
                {
                    return Game.BOOM_BEACH;
                }
                else
                {
                    Console.Clear();
                    Logger.Log("You configured an invalid host (" + Host + ")!", LogType.WARNING);
                    Logger.Log("Please check your config file!", LogType.WARNING);
                    Program.WaitAndClose();
                    return Game.CLASH_ROYALE;
                 }                    
            }
        }
    }
}
