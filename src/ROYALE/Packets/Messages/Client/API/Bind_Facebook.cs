using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server.API;

namespace CRepublic.Royale.Packets.Messages.Client.API
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
            this.Device.Player.Facebook.Identifier = this.FBIdentifier;
            this.Device.Player.Facebook.Token = this.FBToken;

            this.Device.Player.Facebook.Connect();

            new Facebook_Bind_OK(this.Device).Send();
        }
    }
}
