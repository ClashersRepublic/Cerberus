using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Leaderboard;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class My_League : Command
    {
        public My_League(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }

        internal override void Process()
        {
            //new League_Players(this.Device).Send();
        }
    }
}
