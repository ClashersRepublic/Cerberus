using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server;

namespace CRepublic.Magic.Packets.Messages.Client
{
    internal class News_Seen : Message
    {
        internal int Unknown;

        public News_Seen(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.Unknown = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            new News_Seen_Response(Device).Send();
        }
    }
}