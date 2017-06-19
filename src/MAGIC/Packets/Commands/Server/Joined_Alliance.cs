using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Commands.Server
{
    internal class Joined_Alliance : Command
    {
        internal Clan Clan = null;

        public Joined_Alliance(Device _Client) : base(_Client)
        {
            this.Identifier = 1;
        }
        public Joined_Alliance(Device _Client, Clan Clan) : base(_Client)
        {
            this.Identifier = 1;
            this.Clan = Clan;
        }

        internal override void Encode()
        {
            this.Data.AddRange(this.Clan.ToBytesHeader());
        }
    }
}