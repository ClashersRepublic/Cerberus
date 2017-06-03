using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions;

namespace BL.Servers.CR.Database
{
    using StackExchange.Redis;
    using System;
    using Database = Logic.Enums.Database;

    internal class Redis : IDisposable
    {
        internal bool _Disposed = false;

        internal static IDatabase Players;
        internal static IDatabase Clans;
        internal static IDatabase ClanWars;
        internal static IDatabase Battles;

        internal Redis()
        {
            ConfigurationOptions Configuration = new ConfigurationOptions();
            
            Configuration.EndPoints.Add(Utils.ParseConfigString("RedisIPAddress"), Utils.ParseConfigInt("RedisPort"));

            Configuration.Password = Utils.ParseConfigString("RedisPassword");
            Configuration.ClientName = this.GetType().Assembly.FullName;

            ConnectionMultiplexer Connection = ConnectionMultiplexer.Connect(Configuration);

            Redis.Players  = Connection.GetDatabase((int) Database.Players);
            Redis.Clans    = Connection.GetDatabase((int) Database.Clans);

            Loggers.Log("Redis Database has been succesfully loaded.", true);
        }

        ~Redis()
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