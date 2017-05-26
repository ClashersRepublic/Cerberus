using System;
using System.Linq;

namespace BL.Servers.CoC.Packets.Cryptography
{
    internal class RC4Core
    {
        internal const string InitialKey = "gd3owc3ru26uqgfipjzy4z2bkhqe7rclmqz474"; 
        internal const string InitialNonce = "3ru26"; 
        internal RC4 Encryptor { get; set; }
        internal RC4 Decryptor { get; set; }

        internal RC4Core()
        {
            this.InitializeCiphers(RC4Core.InitialKey + RC4Core.InitialNonce);
        }

        internal RC4Core(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            this.InitializeCiphers(key);
        }

        internal void Encrypt(ref byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            for (var k = 0; k < data.Length; k++)
                data[k] ^= this.Encryptor.PRGA();
        }

        internal void Decrypt(ref byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            for (var k = 0; k < data.Length; k++)
                data[k] ^= this.Decryptor.PRGA();
        }

        public void UpdateCiphers(uint clientSeed, byte[] serverNonce)
        {
            if (serverNonce == null)
                throw new ArgumentNullException(nameof(serverNonce));

            var newNonce = RC4Core.ScrambleNonce((ulong) clientSeed, serverNonce);
            var key = RC4Core.InitialKey + newNonce;
            this.InitializeCiphers(key);
        }

        internal void InitializeCiphers(string key)
        {
            this.Encryptor = new RC4(key);
            this.Decryptor = new RC4(key);

            for (var k = 0; k < key.Length; k++)
            {
                this.Encryptor.PRGA();
                this.Decryptor.PRGA();
            }
        }

        internal static byte[] GenerateNonce()
        {
            var buffer = new byte[Core.Resources.Random.Next(15, 25)];
            Core.Resources.Random.NextBytes(buffer);
            return buffer;
        }

        internal static string ScrambleNonce(ulong clientSeed, byte[] serverNonce)
        {
            var scrambler = new Scrambler(clientSeed);
            var byte100 = 0;
            for (var i = 0; i < 100; i++)
                byte100 = scrambler.GetByte();
            return serverNonce.Aggregate(string.Empty,
                (current, t) => current + (char) (t ^ (scrambler.GetByte() & byte100)));
        }
    }
}
