using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Extensions
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

        internal static string LocalNetworkIP
        {
            get
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP))
                {
                    try
                    {
                        socket.Connect("10.0.2.4", 65530);
                        return ((IPEndPoint)socket.LocalEndPoint).Address.ToString();
                    }
                    catch
                    {
                        return "0.0.0.0";
                    }
                }
            }
        }
        internal static byte[] CreateRandomByteArray()
        {
            var r = new Random();
            var buffer = new byte[r.Next(20)];
            r.NextBytes(buffer);
            return buffer;
        }
        internal static string RandomString(int Size)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Program.Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        internal static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            TValue ignored;
            return self.TryRemove(key, out ignored);
        }
    }
}
    