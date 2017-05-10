using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace BL.Proxy.Lyra
{
    class JSONPacketManager
    {
        public static Dictionary<int, JSONPacketWrapper> JsonPackets = new Dictionary<int, JSONPacketWrapper>();
        private static List<ParsedField<object>> Fields = new List<ParsedField<object>>();
        private static ParsedPacket pp;
        private static JSONPacketWrapper wrapper;

        /// <summary>
        /// Handles packet
        /// </summary>
        public static void HandlePacket(Packet packet)
        {
            // Known packet => Parse it
            if (JsonPackets.ContainsKey(packet.ID))
            {
                wrapper = JsonPackets[packet.ID];
                pp = JsonParseHelper.ParsePacket(wrapper, packet);
            }
            // Unknown packet => Save payload
            else
            {
                pp = new ParsedPacket();
                pp.PacketID = packet.ID;
                pp.PacketName = "Unknown";
                pp.PayloadLength = packet.DecryptedPayload.Length;
                pp.ParsedFields = Fields;

                // Payload
                pp.ParsedFields.Add(new ParsedField<object>
                {
                    FieldLength = packet.DecryptedPayload.Length,
                    FieldName = "Payload",
                    FieldType = FieldType.String,
                    FieldValue = packet.DecryptedPayload.ToHexString()
                });
            }

            // Check if the packet is known
            if (Config.JSON_Logging && pp.PacketName != "Unknown")
            {                          
                foreach (var v in pp.ParsedFields)
                    Logger.Log(v.FieldName + " : " + v.FieldValue, LogType.JSON);

                var path = @"JsonPackets\\" + Config.Game + "_" + pp.PacketID + "_" +
                           string.Format("{0:dd-MM_hh-mm-ss}", DateTime.Now) + ".json";
                using (var file = File.CreateText(path))
                {
                    var serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, pp);
                }
            }        
        }

        /// <summary>
        /// Loads all packet definitions
        /// </summary>
        public static void LoadDefinitions()
        {
            // Loop
            var files = Directory.GetFiles("JsonDefinitions", "*.json", SearchOption.AllDirectories);
            foreach (var filePath in files)
            {
                // Open
                using (var file = File.OpenText(filePath))
                {
                    // Deserialize
                    var serializer = new JsonSerializer();
                    var wrapper = (JSONPacketWrapper)serializer.Deserialize(file, typeof(JSONPacketWrapper));

                    // Check existence
                    if (!(JsonPackets.ContainsKey(wrapper.PacketID)))
                        JsonPackets.Add(wrapper.PacketID, wrapper);
                }
            }

            Logger.Log("Definitions loaded (" + JsonPackets.Count + " packets)", LogType.JSON);
        }
    }
}