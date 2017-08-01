using System.Collections.Generic;
using Magic.ClashOfClans.Core.Crypto;
using Magic.ClashOfClans.Core.Crypto.Blake2b;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network.Messages.Client;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 20100
    internal class HandshakeSuccess : Message
    {
        private byte[] _sessionKey;
        private static readonly Hasher Blake = Blake2B.Create(new Blake2BConfig { OutputSizeInBytes = 24 });

        public HandshakeSuccess(ClashOfClans.Client client, HandshakeRequest cka) : base(client)
        {
            MessageType = 20100;
            Blake.Init();
            Blake.Update(Key.Crypto.PrivateKey);
            _sessionKey = Blake.Finish();
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddByteArray(_sessionKey);
            Data = pack.ToArray();
        }
    }
}
