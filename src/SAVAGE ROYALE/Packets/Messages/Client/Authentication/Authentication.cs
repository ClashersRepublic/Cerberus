using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.External.Blake2B;
using CRepublic.Royale.External.Sodium;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Packets.Messages.Server;
using CRepublic.Royale.Packets.Messages.Server.Authentication;

namespace CRepublic.Royale.Packets.Messages.Client.Authentication
{
    internal class Authentication : Message
    {
        internal long UserId;

        internal string Token, MasterHash, Language, UDID;

        internal int Major, Minor, Revision, Locale;

        internal string[] ClientVersion;

        public Authentication(Device device) : base(device)
        {
            this.Device.State = State.LOGIN;
        }
        internal override void Decrypt()
        {
            byte[] Buffer = this.Reader.ReadBytes(this.Length);
            this.Device.PublicKey = Buffer.Take(32).ToArray();

            Blake2BHasher Blake = new Blake2BHasher();

            Blake.Update(this.Device.PublicKey);
            Blake.Update(Key.PublicKey);

            this.Device.RNonce = Blake.Finish();

            Buffer = Sodium.Decrypt(Buffer.Skip(32).ToArray(), this.Device.RNonce, Key.PrivateKey, this.Device.PublicKey);
            this.Device.SNonce = Buffer.Skip(24).Take(24).ToArray();
            this.Reader = new Reader(Buffer.Skip(48).ToArray());

            this.Length = (ushort)Buffer.Length;

        }

        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();

            this.Token = this.Reader.ReadString();

            this.Device.Major = this.Reader.ReadVInt();
            this.Device.Minor = this.Reader.ReadVInt();
            this.Device.Revision = this.Reader.ReadVInt();

            this.MasterHash = this.Reader.ReadString();

            this.UDID = this.Reader.ReadString();

            this.Device.OpenUDID = this.Reader.ReadString();
            this.Device.MACAddress = this.Reader.ReadString();
            this.Device.Model = this.Reader.ReadString();
            this.Device.AdvertiseID = this.Reader.ReadString();

            this.Device.OSVersion = this.Reader.ReadString();

            this.Reader.ReadByte();

            this.Reader.Seek(4);

            this.Device.AndroidID = this.Reader.ReadString();
            this.Language = this.Reader.ReadString();

            this.Reader.ReadByte();
            this.Reader.ReadByte();

            this.Reader.ReadString();

            this.Reader.ReadByte();

            this.Reader.Seek(4);

            this.Reader.ReadByte();

            this.Reader.Seek(17);
        }

       
        internal override void Process()
        {
            if (!string.IsNullOrEmpty(Fingerprint.Json) && !string.Equals(this.MasterHash, Fingerprint.Sha))
            {
                new Authentication_Failed(this.Device, Logic.Enums.Reason.Patch).Send();
                return;
            }


        }

        internal void Login()
        {
            this.Device.Player.Device = this.Device;
        }
    }
}
