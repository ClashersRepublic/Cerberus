using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.API;

namespace CRepublic.Magic.Packets.Messages.Client.API
{
    internal class Facebook_Connect : Message
    {
        internal byte Unknown;

        internal string FBIdentifier;
        internal string FBToken;


        public Facebook_Connect(Device Device, Reader Reader) : base(Device, Reader)
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

            if(this.Device.Player.Avatar.Facebook.Connected)
            new Facebook_Connect_OK(this.Device).Send();
        }
    }
}
