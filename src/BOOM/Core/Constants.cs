namespace BL.Servers.BB.Core
{
    using BL.Servers.BB.Logic.Enums;
    internal class Constants
    {
        internal const bool Local = false;
        internal const int SendBuffer = 2048;
        internal const int ReceiveBuffer = 2048;
        internal const int MaxPlayers = 1000;
        internal static bool Maintenance = false;
        internal const bool PacketCompression = true;
        internal static bool IsMono = false;
        internal const DBMS Database = DBMS.Both;
        internal static string[] AuthorizedIP =
        {
            "192.168.0.26",
            "192.168.0.5"
        };
    }
}
