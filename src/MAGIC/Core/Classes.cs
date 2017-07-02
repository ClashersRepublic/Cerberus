using System;
using System.Data.Entity.Infrastructure;
using CRepublic.Magic.Core.Database;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets;
using CRepublic.Magic.Core.Events;

namespace CRepublic.Magic.Core
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
        internal Game_Events Game_Events;
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
            this.Game_Events = new Game_Events();
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
        
            MySQL_V2.GetAllSeed();
        }

        void IDisposable.Dispose()
        {
            this.Loggers.Dispose();
        }
    }
}
