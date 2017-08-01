using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 24331
    internal class AllianceWarDataMessage : Message
    {
        public AllianceWarDataMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24331;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt64(1); // Team ID
            data.AddString("Clash of Magic");
            data.AddInt32(0);
            data.AddInt32(1);
            data.Add((byte) 0);
            data.AddRange((IEnumerable<byte>) new List<byte> { 1, 2, 3, 4 });
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt64(1); // Team ID
            data.AddString("Clash of Magic");
            data.AddInt32(0);
            data.AddInt32(1);
            data.Add((byte) 0);
            data.AddRange((IEnumerable<byte>) new List<byte> { 1, 2, 3, 4 });
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt64(11);

            data.AddInt32(0);
            data.AddInt32(0);

            data.AddInt32(1);
            data.AddInt32(3600);
            data.AddInt64(1);
            data.AddInt64(1);
            data.AddInt64(2);
            data.AddInt64(2);

            data.AddString("Ultra");
            data.AddString("Powa");

            data.AddInt32(2);
            data.AddInt32(1);
            data.AddInt32(50);

            data.AddInt32(0);

            data.AddInt32(8);
            data.AddInt32(0);
            data.AddInt32(0);
            data.Add((byte) 0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);

            Encrypt(data.ToArray());
        }
    }
}
