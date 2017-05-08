namespace BL.Servers.CoC.External.Sodium
{
    public class BLKeyPair
    {
        public BLKeyPair(byte[] _PublicKey, byte[] _PrivateKey)
        {
            this.PublicKey = _PublicKey;
            this.PrivateKey = _PrivateKey;
        }

        public byte[] PrivateKey { get; set; }

        public byte[] PublicKey { get; set; }
    }
}