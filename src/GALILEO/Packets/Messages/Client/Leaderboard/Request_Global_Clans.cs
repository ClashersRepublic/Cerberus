using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Leaderboard;

namespace BL.Servers.CoC.Packets.Messages.Client.Leaderboard
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
