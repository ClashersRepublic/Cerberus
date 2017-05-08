using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CoC.Extensions
{
    internal static class Utils
    {
        public static int ParseConfigInt(string str) => int.Parse(ConfigurationManager.AppSettings[str]);

        public static bool ParseConfigBoolean(string str) => bool.Parse(ConfigurationManager.AppSettings[str]);

        public static string ParseConfigString(string str) => ConfigurationManager.AppSettings[str];

        internal static string Padding(string _String, int _Limit = 23)
        {
            if (_String.Length > _Limit)
            {
                _String = _String.Remove(_String.Length - (_String.Length - _Limit + 3), _String.Length - _Limit + 3) +
                          "...";
            }
            else if (_String.Length < _Limit)
            {
                int _Length = _Limit - _String.Length;

                for (int i = 0; i < _Length; i++)
                {
                    _String += " ";
                }
            }

            return _String;
        }
        internal static void Increment(this byte[] nonce, int timesToIncrease = 2)
        {
            for (int j = 0; j < timesToIncrease; j++)
            {
                ushort c = 1;
                for (UInt32 i = 0; i < nonce.Length; i++)
                {
                    c += (ushort)nonce[i];
                    nonce[i] = (byte)c;
                    c >>= 8;
                }
            }
        }

        internal static ConsoleColor ChooseRandomColor()
        {
            var randomIndex = new Random().Next(0, Enum.GetNames(typeof(ConsoleColor)).Length);
            var color = (ConsoleColor)randomIndex;

            if (color != ConsoleColor.Black)
                return (ConsoleColor)randomIndex;

            return ConsoleColor.Green;
        }

        public class ArrayReferencePreservngConverter : JsonConverter
        {
            private const string RefProperty = "$ref";
            private const string IdProperty = "$id";
            private const string ValuesProperty = "$values";

            public override bool CanConvert(Type objectType)
            {
                return objectType.IsArray;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Null:
                        return null;
                    case JsonToken.StartArray:
                    {
                        // No $ref.  Deserialize as a List<T> to avoid infinite recursion and return as an array.
                        var elementType = objectType.GetElementType();
                        var listType = typeof(List<>).MakeGenericType(elementType);
                        var list = (IList)serializer.Deserialize(reader, listType);
                        if (list == null)
                            return null;
                        var array = Array.CreateInstance(elementType, list.Count);
                        list.CopyTo(array, 0);
                        return array;
                    }
                    default:
                    {
                        var obj = JObject.Load(reader);
                        var refId = (string)obj[RefProperty];
                        if (refId != null)
                        {
                            var reference = serializer.ReferenceResolver.ResolveReference(serializer, refId);
                            if (reference != null)
                                return reference;
                        }
                        var values = obj[ValuesProperty];
                        if (values == null || values.Type == JTokenType.Null)
                            return null;
                        if (!(values is JArray))
                        {
                            throw new JsonSerializationException($"{values} was not an array");
                        }
                        var count = ((JArray)values).Count;

                        var elementType = objectType.GetElementType();
                        var array = Array.CreateInstance(elementType, count);

                        var objId = (string)obj[IdProperty];
                        if (objId != null)
                        {
                            serializer.ReferenceResolver.AddReference(serializer, objId, array);
                        }

                        var listType = typeof(List<>).MakeGenericType(elementType);
                        using (var subReader = values.CreateReader())
                        {
                            var list = (IList)serializer.Deserialize(subReader, listType);
                            list.CopyTo(array, 0);
                        }

                        return array;
                    }
                }
            }

            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
    