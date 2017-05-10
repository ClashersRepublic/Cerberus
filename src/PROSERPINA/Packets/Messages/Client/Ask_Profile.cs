using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

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
            
        }
    }
}
