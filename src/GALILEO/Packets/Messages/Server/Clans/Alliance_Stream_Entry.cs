using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Packets.Messages.Server.Clans
{
    internal class Alliance_Stream_Entry : Message
    {

        internal Entry Message = null;
        internal Alliance_Stream_Entry(Device _Device, Entry message) : base(_Device)
        {
            this.Identifier = 24312;
            this.Message = message;
        }

        internal override void Encode()
        {
            this.Data.AddRange(this.Message.ToBytes());
        }
    }
}
