namespace Magic.ClashOfClans
{
    internal partial class Client
    {
        public byte[] CPublicKey { get; set; }
        public byte[] CRNonce { get; set; }
        public byte[] CSessionKey { get; set; }
        public byte[] CSharedKey { get; set; }
        public byte[] CSNonce { get; set; }
    }
}
