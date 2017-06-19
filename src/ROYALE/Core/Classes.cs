using CRepublic.Royale.Core.Events;

namespace CRepublic.Royale.Core
{
    using System;
    using System.Data.Entity.Infrastructure;
    using CRepublic.Royale.Database;
    using CRepublic.Royale.Files;
    using CRepublic.Royale.Logic.Enums;
    using CRepublic.Royale.Packets;

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
