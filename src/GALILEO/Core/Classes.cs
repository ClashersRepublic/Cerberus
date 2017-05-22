using System;
using System.Data.Entity.Infrastructure;
using BL.Servers.CoC.Core.Database;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets;
using BL.Servers.CoC.Core.Events;

namespace BL.Servers.CoC.Core
{
    internal class Classes
    {
        internal MessageFactory MFactory;
        internal CommandFactory CFactory;
        internal Loggers Loggers;

        internal CSV CSV;
        internal Home Home;
        internal NPC Npc;
        internal Timers Timers;
        internal Redis Redis;
        internal Fingerprint Fingerprint;
        internal EventsHandler Events;
        internal Parser Parser;
        internal Test Test;
        internal Classes()
        {
            this.MFactory = new MessageFactory();
            this.CFactory = new CommandFactory();
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
            this.Parser = new Parser();
#if DEBUG
            Console.WriteLine("We loaded " + MessageFactory.Messages.Count + " messages, " + CommandFactory.Commands.Count + " commands, and 0 debug commands.\n");
#endif
            this.Timers = new Timers();
            
            this.Test = new Test();
        }
    }
}
