using Republic.Magic.Extensions;
using Republic.Magic.Logic;
using Republic.Magic.Extensions.List;

namespace Republic.Magic.Packets.Messages.Server.Authentication
{
    internal class Session_Key : Message
    {
        internal byte[] Key;
        public Session_Key(Device Device) : base(Device)
        {
            this.Identifier = 20000;
            Key = Utils.CreateRandomByteArray();
        }
        internal override void Encode()
        { 
            this.Data.AddByteArray(Key);
            this.Data.AddInt(1);
        }

        internal override void Process()
        {
           this.Device.RC4.UpdateCiphers(this.Device.ClientSeed, this.Key);
        }
    }
}
