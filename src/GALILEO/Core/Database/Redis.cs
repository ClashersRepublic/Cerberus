using System;
using BL.Servers.CoC.Extensions;
using StackExchange.Redis;

namespace BL.Servers.CoC.Core.Database
{
    internal class Redis
    {
        internal static IDatabase Players;
        internal static IDatabase Clans;
        internal static IDatabase Battles;

        internal Redis()
        {
            ConfigurationOptions Configuration = new ConfigurationOptions();

            Configuration.EndPoints.Add(Utils.ParseConfigString("RedisIPAddress"), Utils.ParseConfigInt("RedisPort"));

            Configuration.Password = Utils.ParseConfigString("RedisPassword");
            Configuration.ClientName = this.GetType().Assembly.FullName;
            Configuration.HighPrioritySocketThreads = true;
            Configuration.SyncTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

            ConnectionMultiplexer Connection = ConnectionMultiplexer.Connect(Configuration);

            Redis.Players = Connection.GetDatabase((int)Logic.Enums.Database.Players);
            Redis.Clans = Connection.GetDatabase((int)Logic.Enums.Database.Clans);
            Redis.Battles = Connection.GetDatabase((int)Logic.Enums.Database.Battles);

            Loggers.Log("Redis Database has been succesfully loaded.", true);
        }
    }
}