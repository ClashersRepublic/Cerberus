using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Extensions.List;

namespace BL.Servers.CoC.Packets.Messages.Server.Authentication
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
