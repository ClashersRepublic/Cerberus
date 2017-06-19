using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server;

namespace CRepublic.Magic.Packets.Messages.Client
{
    internal class Avatar_Profile : Message
    {
        internal long UserID;

        public Avatar_Profile(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Avatar_Profile.
        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();

            this.Reader.ReadByte();

            this.Reader.ReadInt32(); //HomeId High
            this.Reader.ReadInt32(); //HomeId Low
        }

        internal override void Process()
        {
            new Avatar_Profile_Data(this.Device) {UserID = this.UserID}.Send();
        }
    }

}