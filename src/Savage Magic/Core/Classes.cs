using System;

using CRepublic.Magic.Extensions;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets;

namespace CRepublic.Magic.Core
{
    internal class Classes 
    {
        internal static CSV CSV;
        internal static Home Home;
        internal static NPC Npc;
        internal static Game_Events Game_Events;
        internal static Fingerprint Fingerprint;
        internal static void Hey()
        {
            CSV = new CSV();
            Home = new Home();
            Npc = new NPC();
            Game_Events = new Game_Events();
            Fingerprint = new Fingerprint();
           /* switch (Constants.Database)
            {
                case DBMS.Redis:
                case DBMS.Both:
                    this.Redis = new Redis();
                    break;
            }
            */

        }
    }
}
