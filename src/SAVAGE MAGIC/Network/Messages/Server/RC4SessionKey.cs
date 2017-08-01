using System.Collections.Generic;
using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network;

namespace Magic.Packets.Messages.Server
{
    internal class RC4SessionKey : Message
    {
        public RC4SessionKey(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20000;
            Key = Utils.CreateRandomByteArray();
        }
        public override void Encode()
        {
            List<byte> pack = new List<byte>();
            pack.AddByteArray(Key);
            pack.AddInt32(1);
            Encrypt(pack.ToArray());
        }
        public byte[] Key { get; set; }

        public override void Process(Level level)
        {
            Client.UpdateKey(Key);
        }
    }
}