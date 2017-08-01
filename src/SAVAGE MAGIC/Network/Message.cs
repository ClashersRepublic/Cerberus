using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Crypto;
using Magic.ClashOfClans.Core.Crypto.Blake2b;
using Magic.ClashOfClans.Core.Crypto.CustomNaCl;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.Packets.Messages.Server;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network
{
    internal class Message
    {
        // NOTE: Maybe make disposable.

        private PacketReader _reader;
        private byte[] _data;
        private int _length;

        public Message()
        {
            // Space
        }

        public Message(Client client)
        {
            Client = client;
            MessageType = 0;
            MessageVersion = 0;

            _length = -1;
            _data = null;
        }

        public Message(Client client, PacketReader reader)
        {
            Client = client;
            _reader = reader;
        }

        public Client Client { get; set; }
        public PacketReader Reader => _reader;

        public byte[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                _length = value.Length;
            }
        }

        public int Length => _length;
        public ushort MessageType { get; set; }
        public ushort MessageVersion { get; set; }

        public virtual void Decode()
        {
            // Space
        }

        public virtual void Encode()
        {
            // Space
        }

        public virtual void Process(Level level)
        {
            // Space
        }

        public void Decrypt()
        {
            try
            {
                if (Constants.IsRc4)
                {
                    Client.Decrypt(_data);

                    if (MessageType == 10101)
                        Client.State = Client.ClientState.Login;
                }
                else
                {
                    if (MessageType == 10101)
                    {
                        var cipherText = _data;
                        Client.CPublicKey = cipherText.Take(32).ToArray();

                        var blake = Blake2B.Create(new Blake2BConfig
                        {
                            OutputSizeInBytes = 24
                        });
                        blake.Init();
                        blake.Update(Client.CPublicKey);
                        blake.Update(Key.Crypto.PublicKey);

                        Client.CRNonce = blake.Finish();

                        cipherText = CustomNaCl.OpenPublicBox(cipherText.Skip(32).ToArray(), Client.CRNonce, Key.Crypto.PrivateKey, Client.CPublicKey);

                        Client.CSharedKey = Client.CPublicKey;
                        Client.CSessionKey = cipherText.Take(24).ToArray();
                        Client.CSNonce = cipherText.Skip(24).Take(24).ToArray();
                        Client.State = Client.ClientState.Login;

                        Data = cipherText.Skip(48).ToArray();
                    }
                    else
                    {
                        if (MessageType != 10100)
                        {
                            if (Client.State == Client.ClientState.LoginSuccess)
                            {
                                Client.CSNonce.Increment();
                                Data = CustomNaCl.OpenSecretBox(new byte[16].Concat(_data).ToArray(), Client.CSNonce, Client.CSharedKey);
                            }
                        }
                    }
                }
            }
            catch
            {
                Client.State = Client.ClientState.Exception;
                throw;
            }
        }

        public void Encrypt(byte[] plainText)
        {
            try
            {
                if (Constants.IsRc4)
                {
                    Client.Encrypt(plainText);
                    if (MessageType == 20104)
                        Client.State = Client.ClientState.LoginSuccess;

                    Data = plainText;
                }
                else
                {
                    if (MessageType == 20104 || MessageType == 20103)
                    {
                        Hasher b = Blake2B.Create(new Blake2BConfig
                        {
                            OutputSizeInBytes = 24
                        });
                        b.Init();
                        b.Update(Client.CSNonce);
                        b.Update(Client.CPublicKey);
                        b.Update(Key.Crypto.PublicKey);
                        Data = CustomNaCl.CreatePublicBox(Client.CRNonce.Concat(Client.CSharedKey).Concat(plainText).ToArray(), b.Finish(), Key.Crypto.PrivateKey, Client.CPublicKey);
                        if (MessageType == 20104)
                            Client.State = Client.ClientState.LoginSuccess;
                    }
                    else
                    {
                        Client.CRNonce.Increment();
                        Data = CustomNaCl.CreateSecretBox(plainText, Client.CRNonce, Client.CSharedKey).Skip(16).ToArray();
                    }
                }
            }
            catch (Exception)
            {
                Client.State = Client.ClientState.Exception;
                throw;
            }
        }

        public byte[] GetRawData()
        {
            var encodedMessage = new List<byte>(7 + _length);
            encodedMessage.AddUInt16(MessageType);
            encodedMessage.AddInt32WithSkip(_length, 1);
            encodedMessage.AddUInt16(MessageVersion);

            if (_data == null)
                Logger.Error("_data was null when getting raw data of message.");

            encodedMessage.AddRange(_data);

            return encodedMessage.ToArray();
        }
    }
}
