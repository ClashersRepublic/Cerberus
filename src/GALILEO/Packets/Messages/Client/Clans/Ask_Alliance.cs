using CRepublic.Magic.Core;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Clans;

namespace CRepublic.Magic.Packets.Messages.Client.Clans
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