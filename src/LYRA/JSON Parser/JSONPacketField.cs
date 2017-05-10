using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BL.Proxy.Lyra
{
    /// <summary>
    /// Field types
    /// </summary>
    enum FieldType
    {
        String,
        SupercellString,
        LittleEndianInt,
        LittleEndianInt64,
        LittleEndianUInt,
        Int,
        Bytes,
        Int64,
        LittleEndianUInt64,
        UInt,
        UInt64,
        VInt,
        VInt64,
        LittleEndianShort,
        LittleEndianUShort,
        Short,
        UShort
    }

    class JSONPacketField
    {
        /// <summary>
        /// Fieldname
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Byte count
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BytesToRead { get; set; }

        /// <summary>
        /// Field datatype
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }
    }
}