using CRepublic.Magic.Extensions;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Extensions.List;

namespace CRepublic.Magic.Packets.Messages.Server.Authentication
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
