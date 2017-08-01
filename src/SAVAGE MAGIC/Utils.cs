using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans.Logic;
using Magic.Utilities.ZLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Magic.ClashOfClans
{
    internal static class GamePlayUtil
    {
        public static int CalculateResourceCost(int sup, int inf, int supCost, int infCost, int amount) =>
            (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;

        public static int CalculateSpeedUpCost(int sup, int inf, int supCost, int infCost, int amount) =>
            (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;

        public static int GetResourceDiamondCost(int resourceCount, ResourceData resourceData) =>
            Globals.GetResourceDiamondCost(resourceCount, resourceData);

        public static int GetSpeedUpCost(int seconds) =>
            Globals.GetSpeedUpCost(seconds);
    }

    internal static class Utils
    {
        private static Random m_Random = new Random();
        public static Random Random { get { return m_Random; } }
        public static byte[] CreateRandomByteArray()
        {
            var buffer = new byte[Random.Next(20)];
            Random.NextBytes(buffer);
            return buffer;
        }

        public static void AddDataSlots(this List<byte> list, List<DataSlot> data)
        {
            list.AddInt32(data.Count);
            foreach (var dataSlot in data)
            {
                list.AddRange(dataSlot.Encode());
            }
        }

        public static void AddByteArray(this List<byte> list, byte[] data)
        {
            if (data == null)
                list.AddInt32(-1);
            else
            {
                list.AddInt32(data.Length);
                list.AddRange(data);
            }
        }

        public static void AddInt32(this List<byte> list, int data) => list.AddRange(BitConverter.GetBytes(data).Reverse());

        public static void AddInt32WithSkip(this List<byte> list, int data, int skip) => list.AddRange(BitConverter.GetBytes(data).Reverse().Skip(skip));

        public static void AddUInt16(this List<byte> list, ushort data) => list.AddRange(BitConverter.GetBytes(data).Reverse());

        public static void AddInt64(this List<byte> list, long data) => list.AddRange(BitConverter.GetBytes(data).Reverse());

        public static void AddString(this List<byte> list, string data)
        {
            if (data == null)
                list.AddRange(BitConverter.GetBytes(-1).Reverse());
            else
            {
                list.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(data)).Reverse());
                list.AddRange(Encoding.UTF8.GetBytes(data));
            }
        }

        public static void AddCompressedString(this List<byte> list, string data)
        {
            if (data == null)
                list.AddInt32(-1);
            else
            {

                var mem = new MemoryStream();
                using (var bw = new BinaryWriter(mem))
                {
                    var compressedString = ZlibStream.CompressString(data);

                    bw.Write(data.Length);
                    bw.Write(compressedString);
                }

                list.AddInt32(mem.ToArray().Length);
                list.AddRange(mem.ToArray());
            }
        }

        public static string ByteArrayToHex(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", " ").ToUpper();
        }

        public static void Increment(this byte[] nonce, int timesToIncrease = 2)
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

        public static int ParseConfigInt(string str) => int.Parse(ConfigurationManager.AppSettings[str]);

        public static bool ParseConfigBoolean(string str) => Boolean.Parse(ConfigurationManager.AppSettings[str]);

        public static string ParseConfigString(string str) => ConfigurationManager.AppSettings[str];

        public static byte[] ReadAllBytes(this BinaryReader br)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[bufferSize];
                int count;
                while ((count = br.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static Data ReadDataReference(this BinaryReader br) => CsvManager.DataTables.GetDataById(br.ReadInt32WithEndian());

        [Obsolete]
        public static int ReadInt32WithEndian(this BinaryReader br)
        {
            var a32 = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }

        [Obsolete]
        public static long ReadInt64WithEndian(this BinaryReader br)
        {
            var a64 = br.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a64);
            return BitConverter.ToInt64(a64, 0);
        }

        [Obsolete]
        public static string ReadScString(this BinaryReader br)
        {
            var stringLength = br.ReadInt32WithEndian();
            string result;

            if (stringLength > -1)
            {
                if (stringLength > 0)
                {
                    var astr = br.ReadBytes(stringLength);
                    result = Encoding.UTF8.GetString(astr);
                }
                else
                {
                    result = string.Empty;
                }
            }
            else
                result = null;
            return result;
        }

        public static ushort ReadUInt16WithEndian(this BinaryReader br)
        {
            var a16 = br.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a16);
            return BitConverter.ToUInt16(a16, 0);
        }

        public static uint ReadUInt32WithEndian(this BinaryReader br)
        {
            var a32 = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a32);
            return BitConverter.ToUInt32(a32, 0);
        }

        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key)
        {
            TValue ignored;
            return self.TryRemove(key, out ignored);
        }

        public static byte[] ToBytes(this string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static bool Contains(this string str, string substring, StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring", "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison", "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
