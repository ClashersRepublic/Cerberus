
using System.Reflection;

namespace CRepublic.Magic.Extensions
{
    internal class Constants
    {
        internal const int ID = 0;
        internal const int MaxCommand  = 0;
        internal const int Buffer = 4096;
        internal const int PRE_ALLOC_SEA = 128;
        internal const int Verbosity = 3;

        internal const bool PacketCompression = true;
        internal const bool UseRC4 = true;
        internal const bool WriteLog = true;
        internal const bool Local = false;
        internal const bool CloudLogging = false;

        internal static string Title = "Clashers Republic Clash Server - ©Clashers Repbulic | ";

        internal static string[] AuthorizedIP =
        {
            "192.168.0.5",
            "192.168.0.144",
            "115.133.41.158"
        };

    }
}
