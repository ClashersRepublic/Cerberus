namespace BL.Servers.CR.Packets.Messages.Client.Authentication
{
    using BL.Servers.CR.Core;
    using BL.Servers.CR.Core.Network;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Files;
    using BL.Servers.CR.Logic;
    using BL.Servers.CR.Logic.Enums;
    using BL.Servers.CR.Packets.Messages.Server.Authentication;
    using System;

    internal class Pre_Authentification : Message
    {
        internal int AppStore;
        internal int DeviceSO;
        internal int KeyVersion;
        internal int Protocol;
        internal int Major;
        internal int Minor;
        internal int Revision;

        internal string Hash;

        public Pre_Authentification(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.Device.PlayerState = Client_State.SESSION;
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
            if (string.Equals(this.Hash, Fingerprint.Sha))
            {
                if (this.Major == (int)Server_Version.Major && this.Minor == (int)Server_Version.Minor)
                {
                    new Pre_Authentification_OK(Device).Send();
                }
                else
                    new Authentification_Failed(Device, LoginFailed_Reason.Update).Send();
            }
            else
            {
                new Authentification_Failed(this.Device, LoginFailed_Reason.Patch).Send();
            }
        }
    }
}
