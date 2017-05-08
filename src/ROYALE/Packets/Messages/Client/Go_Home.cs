using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Packets.Messages.Client
{
    internal class Go_Home : Message
    {
        public Go_Home(Device Device, Reader Reader) : base(Device, Reader)
        {           
        }

        internal override void Decode()
        {
            this.Reader.ReadBoolean();
        }

        internal override void Process()
        {
            new Own_Home_Data(Device).Send();
        }
    }
}
