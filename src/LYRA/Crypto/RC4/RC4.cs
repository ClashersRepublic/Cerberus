using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Proxy.Lyra
{
    class RC4
    {
        public const int KeyBoxLength = 256;
        public static byte[] Key => "fhsd6f86f67rt8fw78fw789we78r9789wer6re".ToByteArray();

        /// <summary>
        /// Encrypts a packet with RC4
        /// </summary>
        /// <param name="RC4_Key">The public RC4 key</param>
        /// <param name="packet">The packet to encrypt</param>
        /// <returns>Encrypted data</returns>
        public static byte[] Encrypt(byte[] RC4_Key, byte[] packet)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[KeyBoxLength];
            box = new int[KeyBoxLength];
            cipher = new byte[packet.Length];

            for (i = 0; i < KeyBoxLength; i++)
            {
                key[i] = RC4_Key[i % RC4_Key.Length];
                box[i] = i;
            }
            for (j = i = 0; i < KeyBoxLength; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < packet.Length; i++)
            {
                a++;
                a %= KeyBoxLength;
                j += box[a];
                j %= KeyBoxLength;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % KeyBoxLength)];
                cipher[i] = (byte)(packet[i] ^ k);
            }
            return cipher;
        }

        /// <summary>
        /// Decrypts a packet with RC4
        /// </summary>
        /// <param name="RC4_Key">The public RC4 key</param>
        /// <param name="packet">The packet to decrypt</param>
        /// <returns>Decrypted data</returns>
        public static byte[] Decrypt(byte[] RC4_Key, byte[] packet)
        {
            return Encrypt(RC4_Key, packet);
        }

        /// <summary>
        /// An old nonce used by Supercell.
        /// </summary>
        public static byte[] Nonce
        {
            get
            {
                return Encoding.UTF8.GetBytes("nonce");
            }
        }
    }
}
