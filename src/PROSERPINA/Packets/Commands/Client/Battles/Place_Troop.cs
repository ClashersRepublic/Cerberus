using System;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Battles
{
    internal class Place_Troop : Command
    {

        internal long SenderID;

        internal int TroopID;
        internal int TroopType;

        internal int X;
        internal int Y;

        public Place_Troop(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }
        public Place_Troop(Device _Client) : base(_Client)
        {
            this.Identifier = 1;
        }

        internal override void Decode()
        {
            this.Reader.ReadVInt(); // 429
            this.Reader.ReadVInt(); // 449
            
            this.Reader.ReadVInt(); // 0

            this.SenderID = this.Reader.ReadVInt(); // 5 - UserID
            this.SenderID = this.Reader.ReadVInt(); // 5 - UserID

            this.TroopType = this.Reader.ReadVInt(); // 26
            this.TroopID = this.Reader.ReadVInt(); // 3

            this.Reader.ReadVInt(); // 63
            this.Reader.ReadVInt(); // 0

            this.X = this.Reader.ReadVInt();
            this.Y = this.Reader.ReadVInt();
        }

        internal override void Encode()
        {
            this.Data.AddVInt(0); // Checksum

            this.Data.AddVInt(this.SenderID); // Sender ID (High)
            this.Data.AddVInt(this.SenderID); // Sender ID (Low)

            this.Data.AddVInt(5); // Unknown

            this.Data.AddVInt(this.TroopType * 1000000 + this.TroopID); // Troop ID (GlobalID)

            this.Data.AddVInt(1); // Unknown

            this.Data.AddVInt(this.TroopType); // Troop Type
            this.Data.AddVInt(this.TroopID); // Troop ID?

            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(1);
            this.Data.AddVInt(0);
            this.Data.AddVInt(9);
            this.Data.AddVInt(0);

            this.Data.AddVInt(this.X); // X Placement
            this.Data.AddVInt(this.Y); // Y Placement
        }

        internal override void Process()
        {
        }
    }
}
