namespace BL.Servers.BB.Packets.Messages.Server
{
    using BL.Servers.BB.Core.Network;
    using BL.Servers.BB.Extensions.List;
    using BL.Servers.BB.Logic;

    internal class Server_Commands : Message
    {
        internal Command Command = null;
        internal bool ForceExectute;
        public Server_Commands(Device Device, Command Command) : base(Device)
        {
            this.Identifier = 24111;
            this.Command = Command.Handle();
        }
        
        public Server_Commands(Device Device) : base(Device)
        {
            this.Identifier = 24111;
        }
        
        internal override void Encode()
        {
            this.Data.AddInt(this.Command.Identifier);
            this.Data.AddRange(this.Command.Data);
            this.Data.AddBool(this.ForceExectute);
        }
    }
}