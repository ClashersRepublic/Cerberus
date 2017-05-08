using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class My_League : Command
    {
        public My_League(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Process()
        {
            new League_Players(this.Device).Send();
        }
    }
}
