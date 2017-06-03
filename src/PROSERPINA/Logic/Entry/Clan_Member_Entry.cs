using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Logic.Entry
{
    internal class Clan_Member_Entry
    {
        internal Player Player;

        internal Clan Clan;

        internal Clan_Member_Entry(Player Player)
        {
            this.Player = Player;
            this.Clan = Server_Resources.Clans.Get(Player.ClanId);
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(this.Player.UserId);
                _Packet.AddString(this.Player.Facebook.Identifier);
                _Packet.AddString(this.Player.Username);
                _Packet.AddVInt(this.Player.Arena);
                _Packet.AddVInt((int)this.Clan.Members[this.Player.UserId].Role);
                _Packet.AddVInt(this.Player.Experience);
                _Packet.AddVInt(this.Player.Trophies);
                _Packet.AddVInt(this.Clan.Members[this.Player.UserId].Donations);
                _Packet.AddInt(0);
                _Packet.Add(0);
                _Packet.Add(0);
                _Packet.AddVInt(0);
                _Packet.AddVInt(0);
                _Packet.AddVInt(0);
                _Packet.Add(0);
                _Packet.Add(0);
                _Packet.AddLong(this.Player.UserId);

                return _Packet.ToArray();
            }
        }
    }
}
