namespace BL.Servers.CoC.Packets.Cryptography
{
    internal class RC4
    {
        internal RC4(byte[] key)
        {
            Key = KSA(key);
        }

        internal RC4(string key)
        {
            Key = KSA(StringToByteArray(key));
        }

        internal byte[] Key { get; set; }

        internal byte i { get; set; }
        internal byte j { get; set; }

        internal byte PRGA()
        {
            var temp = (byte) 0;

            i = (byte) ((i + 1) % 256);
            j = (byte) ((j + Key[i]) % 256);
            
            temp = Key[i];
            Key[i] = Key[j];
            Key[j] = temp;

            return Key[(Key[i] + Key[j]) % 256];
        }

        internal static byte[] KSA(byte[] key)
        {

            var keyLength = key.Length;
            var S = new byte[256];

            for (var i = 0; i != 256; i++) S[i] = (byte) i;

            var j = (byte) 0;

            for (var i = 0; i != 256; i++)
            {
                j = (byte) ((j + S[i] + key[i % keyLength]) % 256);

                var temp = S[i];
                S[i] = S[j];
                S[j] = temp;
            }
            return S;
        }

        internal static byte[] StringToByteArray(string str)
        {
            var bytes = new byte[str.Length];
            for (var i = 0; i < str.Length; i++) bytes[i] = (byte) str[i];
            return bytes;
        }
    }
}
