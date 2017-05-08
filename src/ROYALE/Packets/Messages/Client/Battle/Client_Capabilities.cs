namespace BL.Servers.CR.Packets.Messages.Client.Battle
{
    using System;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Logic;
    internal class Client_Capabilities : Message
    {
        public Client_Capabilities(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Client_Capabilities.
        }
        internal override void Decode()
        {
            this.Device.Ping = this.Reader.ReadRRInt32();
            this.Device.Interface = this.Reader.ReadString();

            Console.WriteLine($"Ping {this.Device.Ping}");
            Console.WriteLine($"Interface {this.Device.Interface}");
        }
        
    }
}
