
using System;
using System.Linq;
using System.Text;
using Blake2Sharp;

namespace BL.Proxy.Lyra
{
    internal class EnDecrypt
    {
        private const int KeyLength = 32, NonceLength = 24, SessionLength = 24;

        private static readonly KeyPair CustomKeyPair = new KeyPair();
        
        private static byte[] _10101_PublicKey = new byte[KeyLength];

        private static byte[] _10101_SessionKey = new byte[SessionLength];

        private static byte[] _10101_Nonce = new byte[NonceLength];

        private static byte[] _20103_20104_Nonce = new byte[NonceLength];

        private static byte[] _20103_20104_SharedKey = new byte[KeyLength];

        private static readonly Hasher Blake2b = Blake2B.Create(new Blake2BConfig() { OutputSizeInBytes = 24 });

        private enum Status
        {
            Disconnected = 0,
            Authentification = 1,
            Authentified = 2
        };

        private static Status State = Status.Disconnected;

        public static byte[] EncryptPacket(Packet p)
        {
            int packetID = p.ID;
            byte[] decryptedPayload = p.DecryptedPayload;
            byte[] encryptedPayload;

            if (packetID == 10100 || packetID == 20100)
            {
                return decryptedPayload;
            }
            else if (packetID == 10101)
            {
                Blake2b.Init();
                Blake2b.Update(CustomKeyPair.PublicKey);
                Blake2b.Update(Keys.OriginalPublicKey);
                var tmpNonce = Blake2b.Finish();

                decryptedPayload = _10101_SessionKey.Concat(_10101_Nonce).Concat(decryptedPayload).ToArray();
                encryptedPayload = CustomNaCl.CreatePublicBox(decryptedPayload, tmpNonce, CustomKeyPair.SecretKey, Keys.OriginalPublicKey);
                encryptedPayload = CustomKeyPair.PublicKey.Concat(encryptedPayload).ToArray();
                State = Status.Authentification;
            }
            else if (packetID == 20103 || packetID == 20104)
            {
                if (State >= Status.Authentification)
                {      
                    Blake2b.Init();
                    Blake2b.Update(_10101_Nonce);
                    Blake2b.Update(_10101_PublicKey);
                    Blake2b.Update(Keys.ModdedPublicKey);
                    var tmpNonce = Blake2b.Finish();
                    
                    decryptedPayload = _20103_20104_Nonce.Concat(_20103_20104_SharedKey).Concat(decryptedPayload).ToArray();
                    encryptedPayload = CustomNaCl.CreatePublicBox(decryptedPayload, tmpNonce, Keys.GeneratedPrivateKey, _10101_PublicKey);
                    State = Status.Authentified;
                }
                else
                {
                    encryptedPayload = decryptedPayload;
                }
            }
            else
            {
                encryptedPayload = p.Destination == PacketDestination.FROM_CLIENT ? CustomNaCl.CreateSecretBox(decryptedPayload, _10101_Nonce, _20103_20104_SharedKey).Skip(16).ToArray() : CustomNaCl.CreateSecretBox(decryptedPayload, _20103_20104_Nonce, _20103_20104_SharedKey).Skip(16).ToArray();
            }
            return encryptedPayload;
        }

        public static byte[] DecryptPacket(Packet p)
        {
            int packetID = p.ID;
            byte[] encryptedPayload = p.Payload;
            byte[] decryptedPayload;

            if (packetID == 10100 || packetID == 20100)
            { 
                return encryptedPayload;
            }
            else if (packetID == 10101)
            {
                _10101_PublicKey = encryptedPayload.Take(32).ToArray();
                Blake2b.Init();
                Blake2b.Update(_10101_PublicKey);
                Blake2b.Update(Keys.ModdedPublicKey);
                var tmpNonce = Blake2b.Finish();
                
                decryptedPayload = CustomNaCl.OpenPublicBox(encryptedPayload.Skip(32).ToArray(), tmpNonce, Keys.GeneratedPrivateKey, _10101_PublicKey);
                _10101_SessionKey = decryptedPayload.Take(24).ToArray();
                _10101_Nonce = decryptedPayload.Skip(24).Take(24).ToArray();
                decryptedPayload = decryptedPayload.Skip(48).ToArray();

                State = Status.Authentification;
            }
            else if (packetID == 20103 || packetID == 20104)
            {
                if (State >= Status.Authentification)
                {            
                    Blake2b.Init();
                    Blake2b.Update(_10101_Nonce);
                    Blake2b.Update(CustomKeyPair.PublicKey);
                    Blake2b.Update(Keys.OriginalPublicKey);
                    var tmpNonce = Blake2b.Finish();

                    decryptedPayload = CustomNaCl.OpenPublicBox(encryptedPayload, tmpNonce, CustomKeyPair.SecretKey, Keys.OriginalPublicKey);
                    _20103_20104_Nonce = decryptedPayload.Take(24).ToArray();
                    _20103_20104_SharedKey = decryptedPayload.Skip(24).Take(32).ToArray();
                    decryptedPayload = decryptedPayload.Skip(56).ToArray();

                    State = Status.Authentified;
                }
                else
                {
                    return encryptedPayload;
                }
            }
            else
            {
                if (p.Destination == PacketDestination.FROM_CLIENT)
                {
                    _10101_Nonce.Increment();
                    decryptedPayload = CustomNaCl.OpenSecretBox(new byte[16].Concat(encryptedPayload).ToArray(), _10101_Nonce, _20103_20104_SharedKey);
                }
                else
                {
                    _20103_20104_Nonce.Increment();
                    decryptedPayload = CustomNaCl.OpenSecretBox(new byte[16].Concat(encryptedPayload).ToArray(), _20103_20104_Nonce, _20103_20104_SharedKey);
                }
            }
            return decryptedPayload;
        }
    }
}