using System.Collections.Generic;
using Newtonsoft.Json;

namespace BL.Proxy.Lyra
{
    class ParsedPacket
    {
        /// <summary>
        /// PacketID
        /// </summary>
        public int PacketID { get; set; }

        /// <summary>
        /// The Payload length
        /// </summary>
        public int PayloadLength { get; set; }

        /// <summary>
        /// The packet name
        /// </summary>
        public string PacketName { get; set; }

        /// <summary>
        /// Parsed fields
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ParsedField<object>> ParsedFields { get; set; }
    }
}