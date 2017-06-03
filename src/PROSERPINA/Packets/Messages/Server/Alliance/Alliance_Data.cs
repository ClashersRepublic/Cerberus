using BL.Servers.CR.Core;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Logic.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Packets.Messages.Server.Alliance
{
    internal class Alliance_Data : Message
    {
        internal long ClanID;
        internal Clan Clan;

        public Alliance_Data(Device Device, long ClanID) : base(Device)
        {
            this.Identifier = 24301;
            this.ClanID = ClanID;
        }

        internal override void Encode()
        {
            Clan Clan = Core.Server_Resources.Clans.Get(this.ClanID, Constants.Database, false);

            this.Data.AddRange(Clan.FullHeader);
            this.Data.AddString(Clan.Description);

            //this.Data.AddRange(MemberHeaderTest);
            //this.Data.AddVInt(1);

            //foreach (var Member in this.Clan.Members)
            //{
            //    this.Data.AddRange(new byte[] { 0x00 });
            //}

            this.Data.AddVInt(0);

            this.Data.AddRange(new byte[] { 0x00 });
            this.Data.AddVInt(0);

            this.Data.AddByte(0);
            this.Data.AddByte(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddVInt(0);
            this.Data.AddByte(0);
            this.Data.AddByte(0);
        }

        internal byte[] MemberHeaderTest
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddLong(0); // UserID
                _Packet.AddString(""); // FacebookID
                _Packet.AddString("Test"); // Name
                _Packet.AddSCID(21); // Arena
                _Packet.AddByte(1); // Role
                _Packet.AddVInt(1); // Level
                _Packet.AddVInt(0); // Trophies
                _Packet.AddVInt(0); // Donations
                _Packet.AddInt(0); // Unknown
                _Packet.AddByte(1); // Current Rank
                _Packet.AddByte(2); // Previous Rank
                _Packet.AddVInt(0); // Crown Chest Count
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(0); // Unknown
                _Packet.AddVInt(0); // Unknown
                _Packet.AddByte(0); // Unknown
                _Packet.AddByte(0); // Unknown
                _Packet.AddLong(0); // HomeID

                return _Packet.ToArray();
            }
        }
    }
}
