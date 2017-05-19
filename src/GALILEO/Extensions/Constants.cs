using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Extensions
{
    internal class Constants
    {
        internal const bool Local = false;
        internal const int SendBuffer = 4096;
        internal const int MaxPlayers = 10000;
        internal const int ReceiveBuffer = 4096;
        internal const bool PacketCompression = true;
        internal const bool Maintenance = false;

        internal static readonly DBMS Database = (DBMS)Utils.ParseConfigInt("DatabaseMode");
        internal static readonly string UpdateServer = Utils.ParseConfigString("UpdateUrl");

        internal static string[] AuthorizedIP =
        {
            "192.168.0.5",
            "115.133.41.158"
        };

    }
}
