using Republic.Magic.Core;
using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.Clans;

namespace Republic.Magic.Packets.Messages.Client.Clans
{
    internal class Ask_Alliance : Message
    {
        internal long ClanID;

        public Ask_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            this.ClanID = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            new Alliance_Data(this.Device) {Clan = Resources.Clans.Get(this.ClanID)}.Send();
        }
    }
}