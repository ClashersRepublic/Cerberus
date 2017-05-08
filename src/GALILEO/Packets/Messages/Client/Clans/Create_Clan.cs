using BL.Servers.CoC.Core;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Packets.Commands.Server;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Messages.Client.Clans
{
    internal class Create_Clan : Message
    {
        internal Clan Clan = Resources.Clans.New(0, Constants.Database, true);
        public Create_Clan(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal override void Decode()
        {
            this.Clan.Name = this.Reader.ReadString();
            this.Clan.Description = this.Reader.ReadString();
            this.Clan.Badge = this.Reader.ReadInt32();
            this.Clan.Type = (Hiring)this.Reader.ReadInt32();
            this.Clan.Required_Trophies = this.Reader.ReadInt32();
            this.Clan.War_Frequency = this.Reader.ReadInt32();
            this.Clan.Origin = this.Reader.ReadInt32();
            this.Clan.War_History = this.Reader.ReadByte() > 0;
            this.Clan.War_Amical = this.Reader.ReadByte() > 0;
        }
        internal override void Process()
        {
            this.Device.Player.Avatar.ClanId = Clan.Clan_ID;

            this.Clan.Members.Add(this.Device.Player.Avatar);
            new Server_Commands(this.Device) { Command = new Joined_Alliance(this.Device) { Clan = this.Clan }.Handle() }.Send();
            new Server_Commands(this.Device) { Command = new Role_Update(this.Device) { Clan = this.Clan, Role = (int)Role.Leader }.Handle() }.Send();

        }
    }
}
