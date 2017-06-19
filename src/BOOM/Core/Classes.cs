namespace CRepublic.Boom.Core
{
    using System;
    using System.Data.Entity.Infrastructure;
    using CRepublic.Boom.Database;
    using CRepublic.Boom.Files;
    using CRepublic.Boom.Logic.Enums;
    using CRepublic.Boom.Packets;

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
            switch (Constants.Database)
            {
                case DBMS.Redis:
                    throw new UnintentionalCodeFirstException();
                case DBMS.Both:
                    this.Redis = new Redis();
                    break;
            }
            this.Checker = new Timers();
#if DEBUG
            Console.WriteLine("We loaded " + MessageFactory.Messages.Count + " messages, " + CommandFactory.Commands.Count + " commands, and 0 debug commands.\n");
#endif
           //this.Test = new Test();
        }
    }
}
