using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server.Alliance
{
    internal class Response_Create_Alliance : Message
    {
        internal Clan Clan;
        internal int Type;
        internal bool Adv;

        internal Response_Create_Alliance(Device Device, Clan Clan, int Type, bool Adv) : base(Device)
        {
            this.Identifier = 24111;
            this.Clan = Clan;
            this.Type = Type;
            this.Adv = Adv;
        }

        internal override void Encode()
        {
            this.Data.AddVInt(this.Type);
            this.Data.Add(3);
            this.Data.AddVInt(this.Clan.ClanID);

            if (this.Type != 141)
            {
                this.Data.AddString(this.Clan.Name);
                this.Data.Add(16);
                this.Data.AddVInt(this.Clan.Badge);
            }

            this.Data.AddBool(this.Adv);
        }
    }
}
