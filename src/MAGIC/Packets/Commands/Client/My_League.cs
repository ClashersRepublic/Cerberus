using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Leaderboard;

namespace CRepublic.Magic.Packets.Commands.Client
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
