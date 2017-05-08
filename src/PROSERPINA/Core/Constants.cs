namespace BL.Servers.CR.Core
{
    using BL.Servers.CR.Logic.Enums;
    internal class Constants
    {
        internal const bool Local = false;

        internal const int SendBuffer = 2048;

        internal const int ReceiveBuffer = 2048;

        internal const int MaxPlayers = 1000;

        internal static bool Maintenance = false;

        internal static bool BattlesDisabled = false;

        internal const bool PacketCompression = true;

        internal const DBMS Database = DBMS.Both;

        internal const string ServerAddr = "192.168.0.5";
        internal const int ServerPort = 9339;
        internal static string[] AuthorizedIP =
        {
            "192.168.0.26",
            "192.168.0.5"
        };
    }
}
