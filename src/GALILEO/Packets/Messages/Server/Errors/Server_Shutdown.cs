using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server.Errors
{
    internal class Server_Shutdown : Message
    {
        public Server_Shutdown(Device _Device) : base(_Device)
        {
            this.Identifier = 20161;
        }
    }
}