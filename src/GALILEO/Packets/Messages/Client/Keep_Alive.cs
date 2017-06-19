using Republic.Magic.Logic;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Packets.Messages.Server;

namespace Republic.Magic.Packets.Messages.Client
{
   internal class Keep_Alive : Message
    {
        public Keep_Alive(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Keep_Alive.
        }

        internal override void Process()
        {
            new Keep_Alive_OK(this.Device).Send();
        }
    }
}
