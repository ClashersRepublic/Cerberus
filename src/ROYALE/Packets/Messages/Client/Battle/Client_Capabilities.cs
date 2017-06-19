namespace CRepublic.Royale.Packets.Messages.Client.Battle
{
    using System;
    using CRepublic.Royale.Extensions.Binary;
    using CRepublic.Royale.Logic;
    internal class Client_Capabilities : Message
    {
        public Client_Capabilities(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Client_Capabilities.
        }
        internal override void Decode()
        {
            this.Device.Ping = this.Reader.ReadVInt();
            this.Device.Interface = this.Reader.ReadString();
        }
        
    }
}
