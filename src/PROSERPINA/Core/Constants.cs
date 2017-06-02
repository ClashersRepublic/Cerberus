namespace BL.Servers.CR.Core
{
    using BL.Servers.CR.Logic.Enums;
    using BL.Servers.CR.Extensions;
    using BL.Servers.CR.Logic.Manager;

    internal class Constants
    {
        internal const bool Local = true;

        internal const int PRE_ALLOC_SEA = 10000;

        internal const int SendBuffer = 2048;

        internal const int ReceiveBuffer = 2048;

        internal const int MaxPlayers = 1000;

        internal static bool Maintenance_Enabled = Utils.ParseConfigBoolean("MaintenanceEnabled");

        internal static int Maintenace_Duration = 0;

        internal static Maintenance_Timer Maintenance_Timer = null;

        internal static bool BattlesDisabled = Utils.ParseConfigBoolean("BattlesDisabled");

        internal static bool CustomPatch = Utils.ParseConfigBoolean("CustomPatch");

        internal const bool PacketCompression = true;

        internal static bool IsMono = false;

        internal const DBMS Database = DBMS.Both;

        internal const Crypto Encryption = Crypto.RC4;

        internal const Server_Mode Mode = Server_Mode.PRODUCTION;

        internal const int ServerPort = 9339;

        internal static string[] AuthorizedIP =
        {
            "192.168.0.65",
            "115.133.41.158",
            "70.68.246.228",
            "64.114.207.71",
            "192.168.0.26",
            "217.182.63.73"
        };
    }
}
