namespace CRepublic.Boom.Packets.Messages.Server
{
    using CRepublic.Boom.Core.Network;
    using CRepublic.Boom.Extensions.List;
    using CRepublic.Boom.Logic;

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