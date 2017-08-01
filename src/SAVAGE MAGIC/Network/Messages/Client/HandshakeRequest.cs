using System;
using System.Configuration;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic.Core.Settings;
using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class HandshakeRequest : Message
    {

        public HandshakeRequest(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public string Hash;
        public int MajorVersion;
        public int MinorVersion;
        public int Protocol;
        public int KeyVersion;
        public int Unknown;
        public int Device;
        public int Store;

        public override void Decode()
        {
            using (var reader = new PacketReader(new MemoryStream(Data)))
            {
                Protocol = reader.ReadInt32();
                KeyVersion = reader.ReadInt32();
                MajorVersion = reader.ReadInt32();
                Unknown = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                Hash = reader.ReadString();
                Device = reader.ReadInt32();
                Store = reader.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            if (Constants.IsRc4)
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["patchingServer"]))
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL(ConfigurationManager.AppSettings["UpdateUrl"]);
                    p.Send();
                }
                else
                    throw new NullReferenceException("Patching server cannot be nulled in RC4");
            }
            else
                new HandshakeSuccess(Client, this).Send();
        }

    }
}