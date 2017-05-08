using System.Diagnostics;
using BL.Servers.CR.Library.ZLib;

namespace BL.Servers.CR.Extensions.Binary
{
    using BL.Servers.CR.Files;
    using BL.Servers.CR.Files.CSV_Helpers;
    using System;
    using System.IO;
    using System.Text;

    public class Reader : BinaryReader
    {
        public Reader(byte[] _Buffer) : base(new MemoryStream(_Buffer))
        {
            // Packet Reader...
        }

        /// <summary>
        /// Gets a value indicating whether [end of stream].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end of stream]; otherwise, <c>false</c>.
        /// </value>
        public bool EndOfStream => this.BaseStream.Length == this.BaseStream.Position;

        /// <summary>
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="_Buffer">The buffer.</param>
        /// <param name="_Offset">The offset.</param>
        /// <param name="_Count">The count.</param>
        /// <returns></returns>
        public override int Read(byte[] _Buffer, int _Offset, int _Count)
        {
            return this.BaseStream.Read(_Buffer, 0, _Count);
        }

        /// <summary>
        /// Reads the byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadArray()
        {
            int _Length = this.ReadInt32();
            if (_Length == -1 || _Length < -1 || _Length > this.BaseStream.Length - this.BaseStream.Position)
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return _Buffer;
        }

        /// <summary>
        /// Reads a Boolean value from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// true if the byte is nonzero; otherwise, false.
        /// </returns>
        /// <exception cref="System.Exception">Error when reading a bool in packet.</exception>
        public override bool ReadBoolean()
        {
            byte state = this.ReadByte();
            switch (state)
            {
                case 0:
                    return false;

                case 1:
                    return true;

                default:
                    throw new NotSupportedException("Attempted to read a byte [" + state +
                                                    "] but and convert it to boolean, but is out of boolean range.");
            }
        }

        /// <summary>
        /// Reads the next byte from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// The next byte read from the current stream.
        /// </returns>
        public override byte ReadByte()
        {
            return (byte)this.BaseStream.ReadByte();
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            int length = this.ReadInt32();
            if (length == -1)
            {
                return null;
            }

            return this.ReadBytes(length);
        }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte signed integer read from the current stream.
        /// </returns>
        public override short ReadInt16()
        {
            return (short)this.ReadUInt16();
        }

        /// <summary>
        /// Reads the 3 bytes integer.
        /// </summary>
        /// <returns></returns>
        public int ReadInt24()
        {
            byte[] _Temp = this.ReadBytesWithEndian(3, false);
            return (_Temp[0] << 16) | (_Temp[1] << 8) | _Temp[2];
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte signed integer read from the current stream.
        /// </returns>
        public override int ReadInt32()
        {
            return (int)this.ReadUInt32();
        }

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte signed integer read from the current stream.
        /// </returns>
        public override long ReadInt64()
        {
            return (long)this.ReadUInt64();
        }

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.
        /// </summary>
        /// <returns>
        /// The string being read.
        /// </returns>
        public override string ReadString()
        {
            int _Length = this.ReadInt32();
            if (_Length == -1 || _Length < -1 || _Length > this.BaseStream.Length - this.BaseStream.Position)
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return Encoding.UTF8.GetString(_Buffer);
        }

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream using little-endian encoding and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte unsigned integer read from this stream.
        /// </returns>
        public override ushort ReadUInt16()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(_Buffer, 0);
        }

        public uint ReadUInt24()
        {
            return (uint)this.ReadInt24();
        }

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte unsigned integer read from this stream.
        /// </returns>
        public override uint ReadUInt32()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(_Buffer, 0);
        }

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte unsigned integer read from this stream.
        /// </returns>
        public override ulong ReadUInt64()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(_Buffer, 0);
        }

        /// <summary>
        /// Seeks to the specified offset.
        /// </summary>
        /// <param name="_Offset">The offset.</param>
        /// <param name="_Origin">The origin.</param>
        /// <returns></returns>
        public long Seek(long _Offset, SeekOrigin _Origin = SeekOrigin.Current)
        {
            return this.BaseStream.Seek(_Offset, _Origin);
        }

        private byte[] ReadBytesWithEndian(int _Count, bool _Endian = true)
        {
            byte[] _Buffer = new byte[_Count];
            this.BaseStream.Read(_Buffer, 0, _Count);

            if (BitConverter.IsLittleEndian && _Endian)
            {
                Array.Reverse(_Buffer);
            }

            return _Buffer;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        internal Data ReadData()
        {
            int Reference = this.ReadInt32();
            int RowIndex = this.ReadInt32();

            DataTable Table = CSV.Tables.Get(Reference);
            Data Data = Table.GetDataWithID(RowIndex);

            return Data;
        }

        internal byte[] ReadFully()
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = this.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        internal string ReadCompressedString()
        {
            var bytes = ReadBytes();

            if (bytes?.Length > 0)
            {
                var decompressedLength = base.ReadInt32();
                var decompressedBytes = new byte[decompressedLength];

                using (var zlib = new ZlibStream(BaseStream, CompressionMode.Decompress))
                {
                    var count = zlib.Read(decompressedBytes, 0, decompressedLength);
                    Debug.Assert(count == decompressedLength);
                }

                return Encoding.UTF8.GetString(decompressedBytes);
            }
            return null;
        }

        internal int ReadRRInt32()
        {
            byte num1 = this.ReadByte();
            int num2 = (int)num1 & 128;
            int num3 = (int)num1 & 63;
            if (((int)num1 & 64) != 0)
            {
                if (num2 != 0)
                {
                    byte num4 = this.ReadByte();
                    int num5 = (int)num4 << 6 & 8128 | num3;
                    if (((int)num4 & 128) != 0)
                    {
                        byte num6 = this.ReadByte();
                        int num7 = num5 | (int)num6 << 13 & 1040384;
                        if (((int)num6 & 128) != 0)
                        {
                            byte num8 = this.ReadByte();
                            int num9 = num7 | (int)num8 << 20 & 133169152;
                            if (((int)num8 & 128) != 0)
                            {
                                byte num10 = this.ReadByte();
                                num3 = (int)((long)(num9 | (int)num10 << 27) | 2147483648L);
                            }
                            else
                                num3 = (int)((long)num9 | 4160749568L);
                        }
                        else
                            num3 = (int)((long)num7 | 4293918720L);
                    }
                    else
                        num3 = (int)((long)num5 | 4294959104L);
                }
            }
            else if (num2 != 0)
            {
                byte num4 = this.ReadByte();
                num3 |= (int)num4 << 6 & 8128;
                if (((int)num4 & 128) != 0)
                {
                    byte num5 = this.ReadByte();
                    num3 |= (int)num5 << 13 & 1040384;
                    if (((int)num5 & 128) != 0)
                    {
                        byte num6 = this.ReadByte();
                        num3 |= (int)num6 << 20 & 133169152;
                        if (((int)num6 & 128) != 0)
                        {
                            byte num7 = this.ReadByte();
                            num3 |= (int)num7 << 27;
                        }
                    }
                }
            }
            return num3;
        }

        public long ReadRRInt64()
        {
            var hi = (long)ReadRRInt32();
            var lo = (uint)ReadRRInt32();
            return hi << 32 | lo;
        }

        internal long ReadVInt64()
        {
            byte temp = this.ReadByte();
            long i = 0;
            int Sign = (temp >> 6) & 1;
            i = temp & 0x3FL;

            while (true)
            {
                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << 6;

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7 + 7 + 7);

                if ((temp & 0x80) == 0)
                {
                    break;
                }

                temp = this.ReadByte();
                i |= (temp & 0x7FL) << (6 + 7 + 7 + 7 + 7 + 7 + 7 + 7);
            }
            i ^= -Sign;
            return i;
        }

        private int ReadRVarInt()
        {
            int value = 0;
            int i = 0;
            int b = 0;

            while (true)
            {
                b = ReadByte();
                if (i == 0)
                {
                    var seventh = (b & 0x40) >> 6;
                    var msb = (b & 0x80) >> 7;

                    b = b << 1;
                    b = b & ~(0x181);
                    b = b | (msb << 7) | (seventh);
                }

                value |= (b & 0x7F) << i;
                i += 7;

                if (i > 35)
                    throw new Exception("Variable length quantity is too long");

                if ((b & 0x80) == 0)
                    break;
            }

            return value;
        }
        public int ReadVarInt32()
        {
            int value = 0;
            int i = 0;
            int b;
            while (((b = ReadByte()) & 0x80) != 0)
            {
                value |= (b & 0x7F) << i;
                i += 7;
                if (i > 35)
                    throw new Exception("Variable length quantity is too long");
            }
            return value | (b << i);
        }

        public uint ReadVarInt()
        {
            return VarInt.ToUInt32(this.ReadBytes(2));
        }
    }
}