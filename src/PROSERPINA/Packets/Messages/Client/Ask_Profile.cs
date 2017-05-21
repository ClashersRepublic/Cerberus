using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Packets.Messages.Client
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
