using System;
using System.Data.Entity.Infrastructure;
using Republic.Magic.Core.Database;
using Republic.Magic.Extensions;
using Republic.Magic.Files;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Packets;
using Republic.Magic.Core.Events;

namespace Republic.Magic.Core
{
    internal class Classes : IDisposable
    {
        internal MessageFactory MFactory;
        internal CommandFactory CFactory;
        internal DebugFactory DFactory;
        internal Loggers Loggers;

        internal CSV CSV;
        internal Home Home;
        internal NPC Npc;
        internal Timers Timers;
        internal Redis Redis;
        internal Fingerprint Fingerprint;
        internal EventsHandler Events;
        internal Test Test;
        internal Classes()
        {
            this.MFactory = new MessageFactory();
            this.CFactory = new CommandFactory();
            this.DFactory = new DebugFactory();
            this.Loggers = new Loggers();
            this.CSV = new CSV();
            this.Home = new Home();
            this.Npc = new NPC();
            this.Fingerprint = new Fingerprint();
            switch (Constants.Database)
            {
                case DBMS.Redis:
                    throw new UnintentionalCodeFirstException();
                case DBMS.Both:
                    this.Redis = new Redis();
                    break;
            }
            this.Events = new EventsHandler();
#if DEBUG
            Console.WriteLine("We loaded " + MessageFactory.Messages.Count + " messages, " + CommandFactory.Commands.Count + " commands, and " + DebugFactory.Debugs.Count + " debug commands.\n");
#endif
            this.Timers = new Timers();
            
            this.Test = new Test();
        }

        void IDisposable.Dispose()
        {
            this.Loggers.Dispose();
        }
    }
}
