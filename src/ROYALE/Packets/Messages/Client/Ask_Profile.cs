using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;

namespace CRepublic.Royale.Packets.Messages.Client
{
    internal class Ask_Profile : Message
    {
        internal long UserID;
        public Ask_Profile(Device _Client, Reader Reader) : base(_Client, Reader)
        {
            // Avatar_Stream.
        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            new Profile_Data(this.Device).Send();
        }
    }
}
