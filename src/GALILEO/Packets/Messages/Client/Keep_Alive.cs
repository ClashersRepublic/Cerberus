using CRepublic.Magic.Logic;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Packets.Messages.Server;

namespace CRepublic.Magic.Packets.Messages.Client
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
