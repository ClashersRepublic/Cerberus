using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace BL.Proxy.Lyra
{
    static class Extensions
    {
        /// <summary>
        /// Truncates a string
        /// </summary>
        public static string Truncate(this string str, int maxLen)
        {
            return (str.Length > maxLen) ? str.Substring(0, maxLen) + "..." : str;
        }

        /// <summary>
        /// Returns a socket's IP
        /// </summary>
        public static string GetIP(this Socket s) => ((IPEndPoint)s.RemoteEndPoint).Address.ToString();

        /// <summary>
        /// Makes the enum values readable
        /// </summary>
        public static string ReadableName(this Game g)
        {
            switch (g)
            {
                case Game.BOOM_BEACH:
                    return "Boom Beach";
                case Game.CLASH_OF_CLANS:
                    return "Clash of Clans";
                case Game.CLASH_ROYALE:
                    return "Clash Royale";
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// Uses LINQ to convert a hexlified string to a byte array
        /// </summary>
        public static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x%2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        /// <summary>
        /// Converts a byte array to a hexlified string
        /// </summary>
        public static string ToHexString(this byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "").ToLower();
        }

        public static object ReadField(this BinaryReader br, FieldType type)
        {
            object field = null;

            switch (type)
            {
                case FieldType.Int:
                    field = br.ReadInt32();
                    break;
                case FieldType.Int64:
                    field = br.ReadInt64();
                    break;
                case FieldType.LittleEndianInt:
                    field = br.ReadIntWithEndian();
                    break;
                case FieldType.LittleEndianInt64:
                    field = br.ReadLongWithEndian();
                    break;
                case FieldType.LittleEndianShort:
                    field = br.ReadShortWithEndian();
                    break;
                case FieldType.Short:
                    field = br.ReadInt16();
                    break;
                case FieldType.String:
                    field = br.ReadString();
                    break;
                case FieldType.SupercellString:
                    field = br.ReadSupercellString();
                    break;
                case FieldType.VInt:
                    field = br.ReadVInt();
                    break;
                case FieldType.VInt64:
                    field = br.ReadVInt64();
                    break;
                case FieldType.UInt:
                    field = br.ReadUInt32();
                    break;
                case FieldType.LittleEndianUInt:
                    field = br.ReadUIntWithEndian();
                    break;
                case FieldType.LittleEndianUInt64:
                    field = br.ReadULongWithEndian();
                    break;
                case FieldType.UInt64:
                    field = br.ReadUInt64();
                    break;
                case FieldType.UShort:
                    field = br.ReadUInt16();
                    break;
                case FieldType.LittleEndianUShort:
                    field = br.ReadUShortWithEndian();
                    break;
            }
            return field;
        }

        /// <summary>
        /// Increments a sodium generated nonce         
        /// --> https://github.com/jedisct1/libsodium/blob/aff4aaeabf406044e90954593b3d378fc088020a/src/libsodium/sodium/utils.c#L187
        /// </summary>
        /// <param name="nonce">Nonce to increment</param>
        public static void Increment(this byte[] nonce, int timesToIncrease = 2)
        {
            for (int j = 0; j < timesToIncrease; j++)
            {
                ushort c = 1;
                for (UInt32 i = 0; i < nonce.Length; i++)
                {
                    c += (ushort) nonce[i];
                    nonce[i] = (byte) c;
                    c >>= 8;
                }
            }
        }

        /// <summary>
        /// Returns if a socket disconnected
        /// </summary>
        public static bool Disconnected(this Socket socket)
        {
            try
            {
                return (socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return true;
            }
        }

        // Add datatypes to a generic list of bytes
        public static void AddShort(this List<byte> list, short data)
        {
            list.AddRange(BitConverter.GetBytes(data).Reverse());
        }

        public static void AddInt(this List<byte> list, int data)
        {
            list.AddRange(BitConverter.GetBytes(data).Reverse());
        }

        public static void AddLong(this List<byte> list, long data)
        {
            list.AddRange(BitConverter.GetBytes(data).Reverse());
        }

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

        // Read datatypes from a byte array
        public static short ReadShortWithEndian(this BinaryReader br)
        {
            var a16 = br.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a16);
            return BitConverter.ToInt16(a16, 0);
        }

        public static int ReadIntWithEndian(this BinaryReader br)
        {
            var a32 = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }

        //TODO!!!
        public static int ReadVInt(this BinaryReader br)
        {
            int v5;
            byte b = br.ReadByte();
            v5 = b & 0x80;
            int _LR = b & 0x3F;

            if ((b & 0x40) != 0)
            {
                if (v5 != 0)
                {
                    b = br.ReadByte();
                    v5 = ((b << 6) & 0x1FC0) | _LR;
                    if ((b & 0x80) != 0)
                    {
                        b = br.ReadByte();
                        v5 = v5 | ((b << 13) & 0xFE000);
                        if ((b & 0x80) != 0)
                        {
                            b = br.ReadByte();
                            v5 = v5 | ((b << 20) & 0x7F00000);
                            if ((b & 0x80) != 0)
                            {
                                b = br.ReadByte();
                                _LR = (int) (v5 | (b << 27) | 0x80000000);
                            }
                            else
                            {
                                _LR = (int) (v5 | 0xF8000000);
                            }
                        }
                        else
                        {
                            _LR = (int) (v5 | 0xFFF00000);
                        }
                    }
                    else
                    {
                        _LR = (int) (v5 | 0xFFFFE000);
                    }
                }
            }
            else
            {
                if (v5 != 0)
                {
                    b = br.ReadByte();
                    _LR |= (b << 6) & 0x1FC0;
                    if ((b & 0x80) != 0)
                    {
                        b = br.ReadByte();
                        _LR |= (b << 13) & 0xFE000;
                        if ((b & 0x80) != 0)
                        {
                            b = br.ReadByte();
                            _LR |= (b << 20) & 0x7F00000;
                            if ((b & 0x80) != 0)
                            {
                                b = br.ReadByte();
                                _LR |= b << 27;
                            }
                        }
                    }
                }
            }

            return _LR;
        }

        //TODO!!!
        public static long ReadVInt64(this BinaryReader br)
        {
            byte temp = br.ReadByte();
            long i = 0;
            int Sign = (temp >> 6) & 1;
            i = temp & 0x3FL;

            do
            {
                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << 6;

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = br.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7 + 7 + 7 + 7);
            } while (false);

            br.ReadByte();
            i ^= -Sign;
            return i;
        }


        public static long ReadLongWithEndian(this BinaryReader br)
        {
            var a64 = br.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a64);
            return BitConverter.ToInt64(a64, 0);
        }

        public static string ReadSupercellString(this BinaryReader br)
        {
            int lengthOfUTF8Str = br.ReadIntWithEndian();
            string UTF8Str;

            if (lengthOfUTF8Str > -1)
            {
                if (lengthOfUTF8Str > 0)
                {
                    var tmp = br.ReadBytes(lengthOfUTF8Str);
                    UTF8Str = Encoding.UTF8.GetString(tmp);
                }
                else
                {
                    UTF8Str = string.Empty;
                }
            }
            else
                UTF8Str = null;
            return UTF8Str;
        }

        public static int ReadMedium(this BinaryReader br)
        {
            var tmp = br.ReadBytes(3);
            return (0x00 << 24) | (tmp[0] << 16) | (tmp[1] << 8) | tmp[2];
        }

        public static ushort ReadUShortWithEndian(this BinaryReader br)
        {
            var a16 = br.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a16);
            return BitConverter.ToUInt16(a16, 0);
        }


        public static uint ReadUIntWithEndian(this BinaryReader br)
        {
            var a32 = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a32);
            return BitConverter.ToUInt32(a32, 0);
        }

        public static ulong ReadULongWithEndian(this BinaryReader br)
        {
            var a64 = br.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(a64);
            return BitConverter.ToUInt64(a64, 0);
        }
    }
}