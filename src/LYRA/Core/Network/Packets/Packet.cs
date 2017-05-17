using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Networking.Lyra.Core.Network.Packets
{
    internal class Packet
    {
        internal byte[] RawPacket;
        internal byte[] Payload;
        internal byte[] EncrypedPayload;
        internal byte[] DecryptedPayload;

        internal int PacketID;
        internal int Length;
        internal int Version;

        internal string Type;

        internal PacketDestination Destination;

        internal Packet(byte[] Buf, PacketDestination Dest)
        {
            using (var Reader = new BinaryReader(new MemoryStream(Buf)))
            {
                this.RawPacket = Buf;
                this.Destination = Dest;

                this.Type = PacketTypes.GetPacket(this.PacketID);
            }
        }

        internal byte[] Rebuilt
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddRange(BitConverter.GetBytes(this.PacketID).Reverse().Skip(2));
                Packet.AddRange(BitConverter.GetBytes(this.EncrypedPayload.Length).Reverse().Skip(1));
                Packet.AddRange(BitConverter.GetBytes(this.Version).Reverse().Skip(2));

                Packet.AddRange(this.EncrypedPayload);

                return Packet.ToArray();
            }
        }

        internal void Export()
        {
            File.WriteAllBytes($@"Raw_Packets\\ {Constants.Game}_{this.PacketID}_{string.Format("{0:dd-MM_hh-mm-ss}", DateTime.Now)}", this.DecryptedPayload);
        }

        public override string ToString()
        {
            StringBuilder Sb = new StringBuilder();

            Sb.AppendLine($"Direction: {this.Destination}");
            Sb.AppendLine($"ID: {this.PacketID}");
            Sb.AppendLine($"Length: {this.DecryptedPayload.Length}");
            Sb.AppendLine($"Data: {Encoding.UTF8.GetString(this.DecryptedPayload)}");

            return Sb.ToString();
        }
    }
}
