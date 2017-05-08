using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Messages.Client
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
