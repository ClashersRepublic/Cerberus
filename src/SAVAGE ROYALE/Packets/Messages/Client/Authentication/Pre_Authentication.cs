using System;
using System.Threading;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Packets.Messages.Server.Authentication;

namespace CRepublic.Royale.Packets.Messages.Client.Authentication
{
    internal class Pre_Authentication : Message
    {
        internal int AppStore;
        internal int DeviceSO;
        internal int KeyVersion;
        internal int Protocol;
        internal int Major;
        internal int Minor;
        internal int Revision;

        internal string Hash;

        public Pre_Authentication(Device Device) : base(Device)
        {
        }
        internal override void Decrypt()
        {
            var buffer = Data.ToArray();

            this.Reader = new Reader(buffer);
            this.Length = buffer.Length;
        }

        internal override void Decode()
        {
            this.Protocol = this.Reader.ReadInt32();
            this.KeyVersion = this.Reader.ReadInt32();
            this.Major = this.Reader.ReadInt32();
            this.Revision = this.Reader.ReadInt32();
            this.Minor = this.Reader.ReadInt32();
            this.Hash = this.Reader.ReadString();
            this.DeviceSO = this.Reader.ReadInt32();
            this.AppStore = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            new Authentication_Failed(this.Device, Reason.Patch).Send();
        }
    }
}