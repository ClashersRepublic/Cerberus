namespace BL.Servers.CR.Core
{
    using BL.Servers.CR.Logic.Enums;
    using BL.Servers.CR.Extensions;

    internal class Constants
    {
        internal const bool Local = true;

        internal const int SendBuffer = 2048;

        internal const int ReceiveBuffer = 2048;

        internal const int MaxPlayers = 1000;

        internal static bool Maintenance = Utils.ParseConfigBoolean("MaintenanceEnabled");

        internal static bool BattlesDisabled = Utils.ParseConfigBoolean("BattlesDisabled");

        internal static bool CustomPatch = Utils.ParseConfigBoolean("CustomPatch");

        internal const bool PacketCompression = true;

        internal static bool IsMono = false;

        internal const DBMS Database = DBMS.Both;

        internal const int ServerPort = 9339;

        internal static string[] AuthorizedIP =
        {
            "192.168.0.65",
            "115.133.41.158"
        };
    }
}
