using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Logic.Slots.Items;

namespace BL.Servers.CR.Packets.Messages.Server.Battle
{
    internal class Sector_NPC : Message
    {
        internal Sector_NPC(Device Device) : base(Device)
        {
            this.Identifier = 21903;
        }

        internal override void Encode()
        {
            int Count = 6;

            this.Data.AddBool(false);

            this.Data.AddVInt(0);

            this.Data.AddVInt(21);
            this.Data.AddVInt((int) TimeUtils.ToUnixTimestamp(DateTime.Now)); // Timestamp

            this.Data.AddVInt(11);
            this.Data.AddVInt(0);

            this.Data.AddRange("D0B5AFB4E6A8D9EF".HexaToBytes()); // Checksum?

            this.Data.AddRange("0601".HexaToBytes());

            this.Data.AddRange("7F7F7F7F0000".HexaToBytes()); // Enemy ID

            this.Data.AddString(string.Empty);

            this.Data.AddVInt(21);
            this.Data.AddVInt(9999);

            this.Data.AddRange("000000000000000000000000".HexaToBytes());

            this.Data.AddRange("07000000000000000B0000000000000000000004".HexaToBytes());

            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);

            this.Data.AddString(this.Device.Player.Avatar.Username);

            this.Data.AddVInt(this.Device.Player.Avatar.Arena);
            this.Data.AddVInt(this.Device.Player.Avatar.Trophies);

            this.Data.AddRange("AC04000AA38909BC33001E919133B82E000000".HexaToBytes());

            this.Data.AddVInt(this.Device.Player.Avatar.Resources.Count);
            this.Data.AddVInt(this.Device.Player.Avatar.Resources.Count);

            foreach (Resource _Resource in this.Device.Player.Avatar.Resources.OrderBy(r => r.Data))
            {
                this.Data.AddVInt(_Resource.Type);
                this.Data.AddVInt(_Resource.Data);
                this.Data.AddVInt(_Resource.Value);
            }

            this.Data.AddVInt(0); // Count

            this.Data.AddVInt(this.Device.Player.Avatar.Achievements.Count); // Achievement Count

            foreach (Achievement _Achievement in this.Device.Player.Avatar.Achievements)
            {
                this.Data.AddVInt(_Achievement.Type);
                this.Data.AddVInt(_Achievement.Data);
                this.Data.AddVInt(_Achievement.Value);
            }

            this.Data.AddVInt(this.Device.Player.Avatar.Achievements.Completed.Count); 

            this.Data.AddRange("0505068B3E0507AF05050B1E051408051B0B3D1A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D001A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A001A1B001A1C001A1D001A1E001A1F001A20001A22001A23001A25001A27001A2A0E1B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00001C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B00000A008D30AB10008D17A3147EB90122000002022C01".HexaToBytes());

            this.Data.AddRange("7F7F".HexaToBytes()); // Enemy ID

            this.Data.AddVInt(0);

            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);

            this.Data.AddRange("00000000000000000000".HexaToBytes());

            this.Data.AddBool(true); // IsTrainer

            this.Data.AddRange("000000".HexaToBytes());

            this.Data.AddVInt(142);
            this.Data.AddVInt(62077);

            this.Data.AddRange("0000".HexaToBytes());

            this.Data.AddVInt(Count);
            this.Data.AddVInt(Count);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(0);

            this.Data.AddVInt(35);
            this.Data.AddVInt(0);

            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(5);
            this.Data.AddVInt(0);
            this.Data.AddVInt(5);
            this.Data.AddVInt(1);
            this.Data.AddVInt(5);
            this.Data.AddVInt(2);
            this.Data.AddVInt(5);
            this.Data.AddVInt(3);
            this.Data.AddVInt(5);
            this.Data.AddVInt(4);

            this.Data.AddVInt(5);
            this.Data.AddVInt(5);

            this.Data.AddVInt(12); //Level Tower R
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower R Z
            this.Data.AddVInt(25500); //Tower R Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(12); //Level Tower R Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500);  //Tower R Enemy X
            this.Data.AddVInt(6500);  //Tower R Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(12); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500); //Tower L X
            this.Data.AddVInt(25500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(12); //Level Tower L Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower L Enemy X
            this.Data.AddVInt(6500); //Tower L Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(12); //Enemy Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Crown Tower Enemy X
            this.Data.AddVInt(3000); //Tower Tower Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("05027D020401030200007F7F0000000500000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(12); //Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Crown Tower  X
            this.Data.AddVInt(29000); //Crown Tower Tower  Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("067B067E0403000204".HexaToBytes());
            this.Data.AddRange("007F7F000000".HexaToBytes());
            this.Data.AddVInt(5); //elixir bar start ?
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());
            this.Data.AddRange("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000".HexaToBytes());

            this.Data.AddVInt(3668); // Player R
            this.Data.AddVInt(3668); // Enemy R
            this.Data.AddVInt(3668); // Player L
            this.Data.AddVInt(3668); // Enemy L
            this.Data.AddVInt(5832); // Enemy Crown
            this.Data.AddVInt(5832); // Player Crown

            for (int Index = 0; Index < Count; Index++)
            {
                this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            }

            this.Data.AddRange("FF0184010A2A0B1F0B190B2509020B0707060B00FE0316053606040617090E0709091B018101070000".HexaToBytes()); //Deck - Card Type-ID-Level 
            this.Data.AddRange("050602020402010300000000000000060901010000000000000000000000010000000000000000000000000C00000080A1B0A80F002A002B".HexaToBytes());
        }
    }
}