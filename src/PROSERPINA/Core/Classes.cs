using BL.Servers.CR.Core.Events;

namespace BL.Servers.CR.Core
{
    using System;
    using System.Data.Entity.Infrastructure;
    using BL.Servers.CR.Database;
    using BL.Servers.CR.Files;
    using BL.Servers.CR.Logic.Enums;
    using BL.Servers.CR.Packets;

    internal class Classes
    {

        internal MessageFactory MFactory;
        internal CommandFactory CFactory;
        internal Loggers Loggers;
        internal EventsHandler EventsHandler;
        internal Test Test;
        internal CSV CSV;
        internal Fingerprint Fingerprint;
        internal Home Home;
        internal Redis Redis;
        internal Timers Checker;
        internal Classes()
        {
            this.MFactory = new MessageFactory();
            this.CFactory = new CommandFactory();
            this.Loggers = new Loggers();
            this.CSV = new CSV();
            this.Home = new Home();
            this.Fingerprint = new Fingerprint();

            switch (Constants.Database)
            {
                case DBMS.Both:
                    this.Redis = new Redis();
                    break;
            }

            this.EventsHandler = new EventsHandler();

            this.Checker = new Timers();

#if DEBUG
            Console.WriteLine("We loaded " + MessageFactory.Messages.Count + " messages, " + CommandFactory.Commands.Count + " commands, and 0 debug commands.\n");
            this.Test = new Test();
#endif
        }
    }
}
