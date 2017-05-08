using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Extensions
{
    internal class Constants
    {
        internal const bool Local = false;
        internal const int SendBuffer = 2048;
        internal const int MaxPlayers = 1000;
        internal const int ReceiveBuffer = 2048;
        internal const bool PacketCompression = true;

        internal static readonly DBMS Database = (DBMS)Utils.ParseConfigInt("DatabaseMode");
        internal static readonly string UpdateServer = Utils.ParseConfigString("UpdateUrl");
    }
}
