using CRepublic.Magic.Logic;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Packets.Messages.Server.Clans;

namespace CRepublic.Magic.Packets.Messages.Client.Clans
{
    internal class Joinable_Alliance : Message
    {
        public Joinable_Alliance(Device device) : base(device)
        {

        }

        internal override void Process()
        {
            new Alliance_Joinable_Data(this.Device).Send();
        }
    }
}
