using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Leaderboard;

namespace CRepublic.Magic.Packets.Messages.Client.Leaderboard
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
