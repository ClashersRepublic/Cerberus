    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using BL.Servers.CR.Core.Network;
    using BL.Servers.CR.Extensions.List;
    using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Server.Battle
{
    internal class Sector_PC : Message
    {
        internal Logic.Slots.Items.Battle Battle = null;
        internal Sector_PC(Device Device) : base(Device)
        {
            this.Identifier = 21903;
        }

        internal override void Encode()
        {
            this.Data.AddBool(false);
            this.Data.AddHexa("0021A381A2900B0B00F3660944F693DC890701");

            Console.WriteLine(this.Battle.Player1 == this.Device.Player.Avatar);
            Console.WriteLine(this.Battle.Player1.UserId);

            if (this.Battle.Player1 == this.Device.Player.Avatar)
            {
                Console.WriteLine("Sup");
                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);
                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);
                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);

                this.Data.AddString(this.Battle.Player2.Username);
                this.Data.AddByte(21); //Arena?
                this.Data.AddVInt(4000); //Trophy

                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);

                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);

                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);

                this.Data.AddVInt(0);
                this.Data.AddVInt(7);
                this.Data.AddHexa("0C0501B97E0502870E050300050400050CBF13050D00050E00050FAF100516811605199BEFFF8601051A0B051D8688D5440000000505068B3E0507AF05050B1E051408051B0B3D1A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D011A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A011A1B001A1C001A1D001A1E001A1F001A20001A22001A23001A25001A27001A2A0E1B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00011C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B00000A008E30AB10008D17A4147DB901220004");

                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);

                this.Data.AddString(this.Battle.Player1.Username);

                this.Data.AddHexa("08A13AA00500000000001E0000000000070D0501B64605029F12050304050400050C9D1A050D00050E00050FA6150516A7170519BED3EBC50D051A07051C00051D8688D544000000050506933D05079D0C050B1E05140B051B0B88011A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D001A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A001A1B001A1C001A1D001A1E001A1F001A20001A21061A22001A23001A24001A25001A26001A27001A28001A29001A2A001A2B001A2D001A2E001B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00001C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B001C0C001C0D001C1000000B022ABF951800000006E4B88AE6B5B7940293530000BA2187217E0900000009020008");

                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);
                this.Data.AddVInt(0);
                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddVInt(0);
            }
            else
            {
                Console.WriteLine("Suck");
                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddString(this.Battle.Player1.Username);

                this.Data.AddHexa("08A53AAC04000AA38909BC33001E919133B82E000000070C0501B97E0502870E050300050400050CBF13050D00050E00050FAF100516811605199BEFFF8601051A0B051D8688D5440000000505068B3E0507AF05050B1E051408051B0B3D1A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D011A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A011A1B001A1C001A1D001A1E001A1F001A20001A22001A23001A25001A27001A2A0E1B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00011C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B00000A008E30AB10008D17A4147DB901220004");

                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);
                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);
                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);

                this.Data.AddString(this.Battle.Player2.Username);
                this.Data.AddHexa("08A13AA00500000000001E0000000000070D0501B64605029F12050304050400050C9D1A050D00050E00050FA6150516A7170519BED3EBC50D051A07051C00051D8688D544000000050506933D05079D0C050B1E05140B051B0B88011A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D001A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A001A1B001A1C001A1D001A1E001A1F001A20001A21061A22001A23001A24001A25001A26001A27001A28001A29001A2A001A2B001A2D001A2E001B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00001C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B001C0C001C0D001C1000000B022ABF951800000006E4B88AE6B5B7940293530000BA2187217E0900000009020008");

                this.Data.AddVInt(this.Battle.Player1.UserHighId);
                this.Data.AddVInt(this.Battle.Player1.UserLowId);
                this.Data.AddVInt(0);
                this.Data.AddVInt(this.Battle.Player2.UserHighId);
                this.Data.AddVInt(this.Battle.Player2.UserLowId);
                this.Data.AddVInt(0);
            }
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddBool(false);//Is Trainer battle
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddHexa("1C64000006062301230123012301230023000100010000010500050105020503050405050A0DA4E2019C8E030000C07C00A40100000000020000000000000000090DAC36A4650000800400A401000000000100000000000000000A0DAC369C8E030000C07C00A40100000000010000000000000000090DA4E201A4650000800400A40100000000020000000000000000090DA88C01B82E0000800400A40100000000000000000000000000000504037D067E0401050207007F7F0000000500000000007F7F7F7F7F7F7F7F0A0DA88C0188C5030000C07C00A40100000000000000000000000000000400000500000000007F7F7F7F7F7F7F7F000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000AC2FA22BAC2FA22BA844984B00000000000000A401A40100000000000000A401A40100000000000000A401A40100000000000000A401A40100000000000000A401A40100000000000000A401A401FF0116053606040617090E0709091B018101070000050602020402010300000000000000010701010000000000000000000000010000000000000000000000000C000000F3C8DAAF00002A002B");
        }
    }
}
