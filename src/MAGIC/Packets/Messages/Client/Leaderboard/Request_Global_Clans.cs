using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Messages.Server.Leaderboard;

namespace CRepublic.Magic.Packets.Messages.Client.Leaderboard
{
    internal class Request_Global_Clans : Message
    {
        internal byte UnknownByte;
        internal int  UnknownInt1;
        internal int  UnknownInt2;
        internal bool Local;
        internal Village_Mode Village_Type;

        public Request_Global_Clans(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            if (this.Device.Player.Avatar.ClanId > 0)
            {
                this.UnknownByte = this.Reader.ReadByte();
                this.UnknownInt1 = this.Reader.ReadInt32();
                this.UnknownInt2 = this.Reader.ReadInt32();
                this.Local = this.Reader.ReadBoolean();
                this.Village_Type = (Village_Mode) this.Reader.ReadInt32();
            }
            else
            {
                this.Local = this.Reader.ReadByte() == 2;
                this.Village_Type = (Village_Mode)this.Reader.ReadInt32();
            }
        }

        internal override void Process()
        {
            if (this.Village_Type == Village_Mode.BUILDER_VILLAGE)
            {

            }
            else
            {
                if (this.Local)
                    new Global_Clans(this.Device).Send();
                else
                    new Local_Clans(this.Device).Send();
            }
        }
    }
}
