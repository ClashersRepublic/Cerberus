using System.Collections.Generic;
using BL.Networking.Lyra.Enums;

namespace BL.Networking.Lyra.Core.Network.Packets
{
    internal class PacketTypes
    {
        internal static Dictionary<int, string> CR_Packets = new Dictionary<int, string>()
        {
            
        };

        internal static Dictionary<int, string> CoC_Packets = new Dictionary<int, string>()
        {

        };

        internal static Dictionary<int, string> BB_Packets = new Dictionary<int, string>()
        {

        };

        internal static Dictionary<int, string> HD_Packets = new Dictionary<int, string>()
        {

        };

        internal static string GetPacket(int ID)
        {
            string Unknown_Packet = "Unknown Packet";

            switch (Constants.Game)
            {
                case Game.CLASH_ROYALE:
                    if (CR_Packets.ContainsKey(ID))
                        CR_Packets.TryGetValue(ID, out Unknown_Packet);
                    break;

                case Game.CLASH_OF_CLANS:
                    if (CoC_Packets.ContainsKey(ID))
                        CoC_Packets.TryGetValue(ID, out Unknown_Packet);
                    break;

                case Game.BOOM_BEACH:
                    if (BB_Packets.ContainsKey(ID))
                        BB_Packets.TryGetValue(ID, out Unknown_Packet);
                    break;

                case Game.HAY_DAY:
                    if (HD_Packets.ContainsKey(ID))
                        HD_Packets.TryGetValue(ID, out Unknown_Packet);
                    break;
            }

            return Unknown_Packet;
        }
    }
}
