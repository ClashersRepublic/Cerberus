using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Messages.Server.Errors
{
    internal class Server_Shutdown : Message
    {
        public Server_Shutdown(Device _Device) : base(_Device)
        {
            this.Identifier = 20161;
        }
    }
}