using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Cryptography;

namespace CRepublic.Magic.Packets.Messages.Server.Authentication
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