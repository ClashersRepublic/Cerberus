using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Core.Network;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Messages.Server.Authentication;

namespace CRepublic.Magic.Packets.Messages.Client.Authentication
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
        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();

            this.Token = this.Reader.ReadString();

            this.Major = this.Reader.ReadInt32();
            this.Minor = this.Reader.ReadInt32();
            this.Revision = this.Reader.ReadInt32();

            this.MasterHash = this.Reader.ReadString();

            this.Reader.ReadString();

            this.Device.AndroidID = this.Reader.ReadString();
            this.Device.MACAddress = this.Reader.ReadString();
            this.Device.Model = this.Reader.ReadString();

            this.Locale = this.Reader.ReadInt32();    // 2000001

            this.Language = this.Reader.ReadString();
            this.Device.OpenUDID = this.Reader.ReadString();
            this.Device.OSVersion = this.Reader.ReadString();

            this.Device.Android = this.Reader.ReadBoolean();

            this.Reader.ReadString();

            this.Device.AndroidID = this.Reader.ReadString();

            this.Reader.ReadString();

            this.Device.Advertising = this.Reader.ReadBoolean();

            this.Reader.ReadString();

            this.Device.ClientSeed = this.Reader.ReadUInt32();

            this.Reader.ReadByte();
            this.Reader.ReadString();
            this.Reader.ReadString();

            this.ClientVersion = this.Reader.ReadString().Split('.');
        }

        internal override void Process()
        {
            new Session_Key(this.Device).Send();
            new Authentication_OK(this.Device).Send();


        }
    }
}
