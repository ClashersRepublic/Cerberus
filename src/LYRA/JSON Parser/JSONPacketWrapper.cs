using System.Collections.Generic;

namespace BL.Proxy.Lyra
{
    class JSONPacketWrapper
    {
        /// <summary>
        /// The ID of the packet
        /// </summary>
        public int PacketID { get; set; }

        /// <summary>
        /// The packetname
        /// </summary>
        public string PacketName { get; set; }

        /// <summary>
        /// Packet fields
        /// </summary>
        public List<JSONPacketField> Fields { get; set; }
    }
}