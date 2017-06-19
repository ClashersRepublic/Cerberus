using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;
using Republic.Magic.Packets.Cryptography;

namespace Republic.Magic.Packets.Messages.Server.Authentication
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