using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BL.Proxy.Lyra
{
    class ParsedField<T>
    {
        /// <summary>
        /// Name of the field
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Length of the field in bytes
        /// </summary>
        public int FieldLength { get; set; }

        /// <summary>
        /// Datatype of the field
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        /// <summary>
        /// The parsed Value 
        /// </summary>
        public T FieldValue { get; set; }
    }
}