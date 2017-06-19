using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Library.Blake2B;
using CRepublic.Royale.Library.Sodium;
using CRepublic.Royale.Library.ZLib;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Logic.Slots.Items;

namespace CRepublic.Royale.Packets.Messages.Server.Battle
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

            this.Data.AddVInt(6);
            this.Data.AddVInt(1);

            this.Data.AddRange("7F7F7F7F0000".HexaToBytes()); // Enemy ID

            this.Data.AddString(string.Empty);

            this.Data.AddVInt(21);
            this.Data.AddVInt(9999);

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

            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddVInt(11);

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

            this.Data.AddVInt(4);

            this.Data.AddRange(this.Device.Player.Battle.ToBytes);

            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            
            this.Data.AddVInt(2);
            this.Data.AddVInt(2); //Player amount

            this.Data.AddVInt(44);

            this.Data.AddVInt(1);

            this.Data.AddHexa("7F7F");

            this.Data.AddVInt(0);

            this.Data.AddVInt(this.Device.Player.UserHighId);
            this.Data.AddVInt(this.Device.Player.UserLowId);

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

            // IsTrainer
            // True = NPC
            // False = PVP

            this.Data.AddBool(true);

            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddVInt(142);
            this.Data.AddVInt(62077);

            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

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

            for (int Index = 0; Index < Count; Index++)
            {
                this.Data.AddVInt(5);
                this.Data.AddVInt(Index);
            }

            this.Data.AddVInt(12); //Level Tower R
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower R Z
            this.Data.AddVInt(25500); //Tower R Y
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(49276);
            this.Data.AddVInt(0);
            this.Data.AddVInt(41985);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(2);

            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddVInt(12); //Level Tower R Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500);  //Tower R Enemy X
            this.Data.AddVInt(6500);  //Tower R Enemy Y
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(32772);
            this.Data.AddVInt(0);
            this.Data.AddVInt(41985);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddVInt(12); //Level Tower L
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(3500); //Tower L X
            this.Data.AddVInt(25500); //Tower L Y
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(49276);
            this.Data.AddVInt(0);
            this.Data.AddVInt(41985);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddVInt(12); //Level Tower L Enemy
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(14500); //Tower L Enemy X
            this.Data.AddVInt(6500); //Tower L Enemy Y
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(32772);
            this.Data.AddVInt(0);
            this.Data.AddVInt(41985);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);

            this.Data.AddVInt(12); //Enemy Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Crown Tower Enemy X
            this.Data.AddVInt(3000); //Tower Tower Enemy Y
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(32772);
            this.Data.AddVInt(0);
            this.Data.AddVInt(41985);
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
            this.Data.AddVInt(5);
            this.Data.AddVInt(4);
            this.Data.AddVInt(5);
            this.Data.AddRange("027D020401030200007F7F000000".HexaToBytes());
            this.Data.AddVInt(5); // Elixir Start
            this.Data.AddHexa("00000000007F7F7F7F7F7F7F7F");

            this.Data.AddVInt(12); //Crown Tower Level 
            this.Data.AddVInt(13); //Prefixed
            this.Data.AddVInt(9000); //Crown Tower  X
            this.Data.AddVInt(29000); //Crown Tower Tower  Y
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(49276);
            this.Data.AddVInt(0);
            this.Data.AddVInt(41985);
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
            this.Data.AddVInt(5);
            this.Data.AddVInt(4);
            this.Data.AddVInt(6);
            this.Data.AddRange("7B067E0403000204007F7F000000".HexaToBytes());
            this.Data.AddVInt(5); // Elixir Start
            this.Data.AddRange("00000000007F7F7F7F7F7F7F7F".HexaToBytes());

            for (int Index = 0; Index < 48; Index++)
            {
                this.Data.AddVInt(0);
            }

            this.Data.AddVInt(3668); // Player R
            this.Data.AddVInt(3668); // Enemy R
            this.Data.AddVInt(3668); // Player L
            this.Data.AddVInt(3668); // Enemy L
            this.Data.AddVInt(5832); // Enemy Crown
            this.Data.AddVInt(5832); // Player Crown

            for (int Index = 0; Index < Count; Index++)
            {
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);
                this.Data.AddVInt(0);

                this.Data.AddVInt(41985);
                this.Data.AddVInt(41985);
            }

            this.Data.AddRange("FF0184010A2A0B1F0B190B2509020B0707060B00".HexaToBytes()); //Deck - Card Type-ID-Level 
            this.Data.AddHexa("FE03");
            this.Data.AddRange(this.Device.Player.Decks.Hand());
            this.Data.AddRange("0000050602020402010300000000000000060901010000000000000000000000010000000000000000000000000C00000080A1B0A80F002A002B".HexaToBytes());

            ZlibStream.CompressBuffer(this.Data.ToArray());
        }
    }
}