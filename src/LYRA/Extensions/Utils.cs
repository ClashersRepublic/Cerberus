using System;
using System.Configuration;
using System.Linq;

namespace BL.Networking.Lyra.Extensions
{
    internal static class Utils
    {
        internal static string ToHexString(this byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "").ToLower();
        }

        internal static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        internal static int ParseConfigInt(string str) => int.Parse(ConfigurationManager.AppSettings[str]);

        internal static bool ParseConfigBoolean(string str) => bool.Parse(ConfigurationManager.AppSettings[str]);

        internal static string ParseConfigString(string str) => ConfigurationManager.AppSettings[str];

        internal static ConsoleColor ChooseRandomColor()
        {
            var randomIndex = new Random().Next(0, Enum.GetNames(typeof(ConsoleColor)).Length);
            var color = (ConsoleColor)randomIndex;

            if (color != ConsoleColor.Black)
                return (ConsoleColor)randomIndex;

            return ConsoleColor.Green;
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
    }
}
