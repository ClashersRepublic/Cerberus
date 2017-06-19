using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Leaderboard;

namespace Republic.Magic.Packets.Messages.Client.Leaderboard
{
    internal class Request_Global_Clans : Message
    {
        internal int Unknown;

        public Request_Global_Clans(Device Device, Reader Reader) : base(Device)
        {

        }
        internal override void Decode()
        {
            this.Unknown = this.Data.Count == 10 ? this.Data[9] : this.Reader.Read();
        }

        internal override void Process()
        {
            /*if (this.Unknown == 0)
                new Global_Clans(this.Device).Send();
            else
                new Local_Clans(this.Device).Send();*/
        }
    }
}
