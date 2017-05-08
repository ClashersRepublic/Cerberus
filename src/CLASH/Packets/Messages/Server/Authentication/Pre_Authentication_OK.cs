using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Packets.Messages.Server.Authentication
{
    internal class Pre_Authentication_OK : Message
    {

        internal Pre_Authentication_OK(Device Device) : base(Device)
        {
            this.Identifier = 20100;
            this.Device.State = State.SESSION_OK;
        }
        
        internal override void Encode()
        {
            this.Data.AddInt(24);
            this.Data.AddRange(Key.NonceKey);
        }
    }
}