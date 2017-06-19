using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server
{
    using CRepublic.Royale.Extensions.List;

    internal class Shutdown_Started : Message
    {
        internal Shutdown_Started(Device Device) : base(Device)
        {
            this.Identifier = 20161;
        }

        internal override void Encode()
        {
            this.Data.AddVInt(2);
        }
    }
}
