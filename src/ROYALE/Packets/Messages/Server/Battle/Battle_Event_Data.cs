using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.List;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server.Battle
{
    internal class Battle_Event_Data : Message
    {
        internal int CommandSenderHigh;
        internal int CommandSenderLow;
        internal int CommandID;
        internal int CommandValue;
        internal int CommandTick;
        internal int CommandUnk;
        internal int CommandUnk2;

        internal Battle_Event_Data(Device Device) : base(Device)
        {
            this.Identifier = 22952;
        }

        internal override void Encode()
        {
            /* this.Writer.AddVInt(this.CommandID);     // 01
            this.Writer.AddVInt(this.CommandSender); // 07-AA-C6-F7-01
            this.Writer.AddVInt(this.CommandUnk);    // 01
            this.Writer.AddVInt(this.CommandTick);   // A3-32
            this.Writer.AddVInt(this.CommandUnk2);   // 00-01
            this.Writer.AddVInt(this.CommandValue);  // 01 */

            this.Data.AddVInt(this.CommandID);
            this.Data.AddVInt(this.CommandSenderHigh);
            this.Data.AddVInt(this.CommandSenderLow);
            this.Data.AddVInt(1);
            this.Data.AddRange("A3-32".HexaToBytes());
            this.Data.AddRange("00-01".HexaToBytes());
            this.Data.AddVInt(this.CommandValue);
        }
    }
}
