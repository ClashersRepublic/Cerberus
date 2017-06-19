using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;

namespace CRepublic.Royale.Packets.Messages.Client
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
