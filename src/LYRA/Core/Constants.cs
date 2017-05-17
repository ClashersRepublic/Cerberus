using BL.Networking.Lyra.Enums;
using BL.Networking.Lyra.Extensions;

namespace BL.Networking.Lyra.Core
{
    internal class Constants
    {
        internal static int Backlog = 100;

        internal static int Port = 9339;

        internal static int Header_Size = 7;

        internal static int Key_Length = 32;

        public static string Host = Utils.ParseConfigString("Host");

        public static Game Game
        {
            get
            {
                if (Host == "game.clashofclans.com" || Host == "gamea.clashofclans.com")
                {
                    return Game.CLASH_OF_CLANS;
                }
                else if (Host == "game.clashroyaleapp.com")
                {
                    return Game.CLASH_ROYALE;
                }
                else if (Host == "game.boombeachgame.com")
                {
                    return Game.BOOM_BEACH;
                }
                else
                {
                    return Game.CLASH_OF_CLANS;;
                }
            }
        }
    }
}
