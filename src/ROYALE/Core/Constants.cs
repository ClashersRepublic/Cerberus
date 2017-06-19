namespace CRepublic.Royale.Core
{
    using CRepublic.Royale.Logic.Enums;
    using CRepublic.Royale.Extensions;
    using CRepublic.Royale.Logic.Manager;

    internal class Constants
    {
        internal const bool Local = false;

        internal const int PRE_ALLOC_SEA = 10000;

        internal const int SendBuffer = 2048;

        internal const int ReceiveBuffer = 2048;

        internal const int MaxPlayers = 1000;

        internal static int Maintenace_Duration = 0;

        internal static Maintenance_Timer Maintenance_Timer = null;

        internal static bool BattlesDisabled = Utils.ParseConfigBoolean("BattlesDisabled");

        internal const bool PacketCompression = true;

        internal static bool IsMono = false;

        internal const DBMS Database = DBMS.Both;

        internal const Server_Crypto Encryption = Server_Crypto.RC4;

        internal const Server_Mode Mode = Server_Mode.PRODUCTION;

        internal const int ServerPort = 9339;

        internal static string[] AuthorizedIP =
        {
            "70.68.246.228"
        };
    }
}