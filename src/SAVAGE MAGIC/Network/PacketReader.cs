using System;
using System.IO;
using System.Text;

namespace Magic.ClashOfClans.Network
{
    public class PacketReader : BinaryReader
    {
        public PacketReader(byte[] buffer) : base(new MemoryStream(buffer))
        {
            // Space
        }

        public PacketReader(Stream stream) : base(stream)
        {
            // Space
        }

        byte[] ReadBytesWithEndian(int count, bool switchEndian = true)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian)
                Array.Reverse(buffer);
            return buffer;
        }

        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, 0, count);

        public override bool ReadBoolean()
        {
            var state = ReadByte();
            switch (state)
            {
                case 1:
                    return true;

                case 0:
                    return false;

                default:
                    return false;
            }
        }

        public override byte ReadByte() => (byte)BaseStream.ReadByte();

        public byte[] ReadByteArray()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1 || length > BaseStream.Length - BaseStream.Position)
                return null;
            var buffer = ReadBytesWithEndian(length, false);
            return buffer;
        }

        public override short ReadInt16() => (short)ReadUInt16();

        public int ReadInt24()
        {
            var packetLengthBuffer = ReadBytesWithEndian(3, false);
            return (packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8) | packetLengthBuffer[2];
        }

        public override int ReadInt32() => (int)ReadUInt32();

        public override long ReadInt64() => (long)ReadUInt64();

        public override string ReadString()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1 || length > BaseStream.Length - BaseStream.Position)
                return null;
            var buffer = ReadBytesWithEndian(length, false);
            return Encoding.UTF8.GetString(buffer);
        }

        public override ushort ReadUInt16()
        {
            var buffer = ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public uint ReadUInt24() => (uint)ReadInt24();

        public override uint ReadUInt32()
        {
            var buffer = ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public override ulong ReadUInt64()
        {
            var buffer = ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);
    }
}
