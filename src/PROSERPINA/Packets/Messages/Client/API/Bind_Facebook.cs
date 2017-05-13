using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server.API;

namespace BL.Servers.CR.Packets.Messages.Client.API
{
    internal class Bind_Facebook : Message
    {
        internal byte Unknown;

        internal string FBIdentifier;
        internal string FBToken;

        public Bind_Facebook(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Facebook_Connect.
        }

        internal override void Decode()
        {
            this.Unknown = this.Reader.ReadByte();
            this.FBIdentifier = this.Reader.ReadString();
            this.FBToken = this.Reader.ReadString();
        }
 
        internal override void Process()
        {
            this.Device.Player.Avatar.Facebook.Identifier = this.FBIdentifier;
            this.Device.Player.Avatar.Facebook.Token = this.FBToken;

            this.Device.Player.Avatar.Facebook.Connect();

            new Facebook_Bind_OK(this.Device).Send();
        }
    }
}
