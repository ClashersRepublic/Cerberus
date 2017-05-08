namespace BL.Servers.BB.Core
{
    using System;
    using System.Data.Entity.Infrastructure;
    using BL.Servers.BB.Database;
    using BL.Servers.BB.Files;
    using BL.Servers.BB.Logic.Enums;
    using BL.Servers.BB.Packets;

    internal class Classes
    {

        internal MessageFactory MFactory;
        internal CommandFactory CFactory;
        internal Loggers Loggers;
        internal Test Test;
        internal CSV CSV;
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
            if (Constants.Database == DBMS.Redis)
                throw new UnintentionalCodeFirstException();
            else if (Constants.Database == DBMS.Both)
                this.Redis = new Redis();
            this.Checker = new Timers();
#if DEBUG
            Console.WriteLine("We loaded " + MessageFactory.Messages.Count + " messages, " + CommandFactory.Commands.Count + " commands, and 0 debug commands.\n");
#endif
           this.Test = new Test();
        }
    }
}
