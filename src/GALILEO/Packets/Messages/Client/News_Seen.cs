using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server;

namespace Republic.Magic.Packets.Messages.Client
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