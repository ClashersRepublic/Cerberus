using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

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
            int Count = 20;
            #region Ultra Mode
            this.Data.AddBool(false);

            this.Data.AddVInt(0);
            this.Data.AddVInt(21);
            this.Data.AddVInt((int)TimeUtils.ToUnixTimestamp(DateTime.Now));
            this.Data.AddVInt(11);
            this.Data.AddVInt(0);
            this.Data.AddHexa("D0B5AFB4E6A8D9EF");// Checksum?
            this.Data.AddHexa("0601");
            this.Data.AddHexa("7F7F7F7F0000");//Enemy id
            this.Data.AddString("COL'S Mum");
            this.Data.AddByte(11); //Arena?
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

            this.Data.AddVInt(7);
            this.Data.AddVInt(0); //Resource?
            
            this.Data.AddHexa("0000000000000B0000000000000000000004");
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddString(this.Device.Player.Avatar.Username);
            this.Data.AddHexa("12823BAC04000AA38909BC33001E919133B82E000000070C0501B17B0502870E050300050400050CBF13050D00050E00050FAF100516811605199BEFFF8601051A0B051D8688D5440000000505068B3E0507AF05050B1E051408051B0B3D1A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D001A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A001A1B001A1C001A1D001A1E001A1F001A20001A22001A23001A25001A27001A2A0E1B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00001C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B00000A008D30AB10008D17A3147EB90122000002");
            this.Data.AddRange("022C01".HexaToBytes());
            this.Data.AddRange("7F7F".HexaToBytes());
            this.Data.AddVInt(0);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(0);
            this.Data.AddHexa("000000000000000000");
            this.Data.AddBool(true);//IsTrainer
            this.Data.AddHexa("0000008E02F27D0000");

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

            this.Data.AddVInt(35);
            this.Data.AddVInt(0);

            this.Data.AddVInt(35);
            this.Data.AddVInt(0);

            this.Data.AddVInt(35);
            this.Data.AddVInt(0);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(1);

            this.Data.AddVInt(35);
            this.Data.AddVInt(16);

            this.Data.AddVInt(35);
            this.Data.AddVInt(16);

            this.Data.AddVInt(35);
            this.Data.AddVInt(16);

            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);
            this.Data.AddVInt(1);

            for (int Index = 0; Index < Count; Index++)
            {
                this.Data.AddVInt(5);
                this.Data.AddVInt(Index);
            }

            this.Data.AddVInt(9); //Level Tower R
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower R Z
            this.Data.AddVInt(25500); //Tower R Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(11); //Level Tower R Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500);  //Tower R Enemy X
            this.Data.AddVInt(6500);  //Tower R Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500); //Tower L X
            this.Data.AddVInt(25500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(11); //Level Tower L Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower L Enemy X
            this.Data.AddVInt(6500); //Tower L Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(11); //Enemy Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Crown Tower Enemy X
            this.Data.AddVInt(3000); //Tower Tower Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("05027D020401030200".HexaToBytes());
            this.Data.AddRange("007F7F000000".HexaToBytes());
            this.Data.AddVInt(5); //elixir bar 
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(9); //Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(7100); //Crown Tower  X
            this.Data.AddVInt(29000); //Crown Tower Tower  Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("067B067E0403000204".HexaToBytes());
            this.Data.AddRange("007F7F000000".HexaToBytes());
            this.Data.AddVInt(400); //elixir bar 
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(9); //Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(11100); //Crown Tower  X
            this.Data.AddVInt(29000); //Crown Tower Tower  Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("067B067E0403000204".HexaToBytes());
            this.Data.AddRange("007F7F000000".HexaToBytes());
            this.Data.AddVInt(400); //elixir bar 
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(9); //Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3100); //Crown Tower  X
            this.Data.AddVInt(29000); //Crown Tower Tower  Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("067B067E0403000204".HexaToBytes());
            this.Data.AddRange("007F7F000000".HexaToBytes());
            this.Data.AddVInt(400); //elixir bar 
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(9); //Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(15100); //Crown Tower  X
            this.Data.AddVInt(29000); //Crown Tower Tower  Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("067B067E0403000204".HexaToBytes());
            this.Data.AddRange("007F7F000000".HexaToBytes());
            this.Data.AddVInt(400); //elixir bar 
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Tower L X
            this.Data.AddVInt(25500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Tower L X
            this.Data.AddVInt(20500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower L X
            this.Data.AddVInt(20500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500); //Tower L X
            this.Data.AddVInt(20500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower Middle2
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(6250); //Tower Bottom Middle2 X
            this.Data.AddVInt(25500); //Tower Bottom Middle2 Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower Bottom Middle3
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(11750); //Tower Bottom Middle3 X
            this.Data.AddVInt(25500); //Tower Bottom Middle3 Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());


            this.Data.AddVInt(9); //Level Tower Upper Middle2
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(6250); //Tower Upper Middle2 X
            this.Data.AddVInt(20500); //TowerUpper Middle2 Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower Upper Middle3
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(11750); //Tower Upper Middle3 X
            this.Data.AddVInt(20500); //Tower Upper Middle3 Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(0); //Wall Level
            this.Data.AddVInt(9); //Prefixed
            this.Data.AddVInt(9000); //Wall X
            this.Data.AddVInt(29000); //Wall Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("a40100000000000000000000000000".HexaToBytes());

            this.Data.AddVInt(0); //Wall Level
            this.Data.AddVInt(9); //Prefixed
            this.Data.AddVInt(5000); //Wall X
            this.Data.AddVInt(29000); //Wall Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("a40100000000000000000000000000".HexaToBytes());

            this.Data.AddVInt(0); //Wall Level
            this.Data.AddVInt(9); //Prefixed
            this.Data.AddVInt(13000); //Wall X
            this.Data.AddVInt(29000); //Wall Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("a40100000000000000000000000000".HexaToBytes());

            for (int Index = 0; Index < Count; Index++)
            {
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
            }

            this.Data.AddVInt(2786);
            this.Data.AddVInt(3052);//Enemy

            this.Data.AddVInt(2786);

            this.Data.AddVInt(3052);//Enemy
            this.Data.AddVInt(4824);//Enemy

            this.Data.AddVInt(4392);
            this.Data.AddVInt(4392);
            this.Data.AddVInt(4392);
            this.Data.AddVInt(4392);

            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);
            this.Data.AddVInt(2786);

            for (int Index = 0; Index < Count; Index++)
            {
                this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            }

            this.Data.AddHexa("FF"); //Deck - Card Type-ID-Level 
            this.Data.AddRange(this.Device.Player.Avatar.Decks.Hand());
            this.Data.AddHexa("FE03");
            this.Data.AddRange(this.Device.Player.Avatar.Decks.Hand());
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddRange("050602020402010300000000000000060901010000000000000000000000010000000000000000000000000C00000000000000000000000080A1B0A80F002A002B".HexaToBytes());
            
            #endregion

            #region Normal Mode

           /* this.Data.AddBool(false);
            this.Data.AddRange("00217F0B00D0B5AFB4E6A8D9EF06017F7F7F7F00000000000012983B00000000000000000000000007000000000000000B0000000000000000000004".HexaToBytes());
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddString(this.Device.Player.Avatar.Username);
            this.Data.AddRange("12823BAC04000AA38909BC33001E919133B82E000000070C0501B17B0502870E050300050400050CBF13050D00050E00050FAF100516811605199BEFFF8601051A0B051D8688D5440000000505068B3E0507AF05050B1E051408051B0B3D1A00001A01001A02001A03001A04001A05001A06001A07001A08001A09001A0A001A0B001A0C001A0D001A0E001A0F001A10001A11001A12001A13001A14001A15001A16001A17001A18001A19001A1A001A1B001A1C001A1D001A1E001A1F001A20001A22001A23001A25001A27001A2A0E1B00001B01001B02001B03001B04001B05001B06001B07001B08001B09001B0A001C00001C01001C02001C03001C04001C05001C06001C07001C08001C09001C0A001C0B00000A008D30AB10008D17A3147EB90122000002022C017F7F00".HexaToBytes());
            this.Data.AddVInt(this.Device.Player.Avatar.UserHighId);
            this.Data.AddVInt(this.Device.Player.Avatar.UserLowId);
            this.Data.AddRange("00000000000000000000010000008E02F27D00000606230123012301230123002300010001000001050005010502050305040505".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower R
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower R Z
            this.Data.AddVInt(25500); //Tower R Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(11); //Level Tower R Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500);  //Tower R Enemy X
            this.Data.AddVInt(6500);  //Tower R Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(9); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500); //Tower L X
            this.Data.AddVInt(25500); //Tower L Y
            this.Data.AddRange("0000C07C00".HexaToBytes());
            this.Data.AddRange("A40100000000010000000000000000".HexaToBytes());

            this.Data.AddVInt(11); //Level Tower L Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower L Enemy X
            this.Data.AddVInt(6500); //Tower L Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000020000000000000000".HexaToBytes());

            this.Data.AddVInt(11); //Enemy Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Crown Tower Enemy X
            this.Data.AddVInt(3000); //Tower Tower Enemy Y
            this.Data.AddRange("0000800400".HexaToBytes());
            this.Data.AddRange("A40100000000000000000000000000000504".HexaToBytes());
            this.Data.AddRange("05027D020401030200007F7F0000000500000000007F7F7F7F7F7F7F7F".HexaToBytes());

            this.Data.AddVInt(9); //Crown Tower Level 
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
            this.Data.AddVInt(2786);
            this.Data.AddVInt(3052);//Enemy
            this.Data.AddVInt(2786);
            this.Data.AddVInt(3052);//Enemy
            this.Data.AddVInt(4824);//Enemy
            this.Data.AddVInt(4392);

            this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            this.Data.AddRange("00000000000000A401A401".HexaToBytes());
            this.Data.AddRange("00000000000000A401A401".HexaToBytes());

            this.Data.AddRange("FF0184010A2A0B1F0B190B2509020B0707060B00FE03".HexaToBytes()); //Deck - Card Type-ID-Level 
            this.Data.AddRange(this.Device.Player.Avatar.Decks.Hand());
            this.Data.AddRange("0000050602020402010300000000000000060901010000000000000000000000010000000000000000000000000C00000080A1B0A80F002A002B".HexaToBytes());
            */
#endregion
        }
    }
}