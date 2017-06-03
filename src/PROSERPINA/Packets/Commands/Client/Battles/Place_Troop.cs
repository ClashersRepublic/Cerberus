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
        internal int Tick;
        internal int Checksum;
        internal int X;
        internal int Y;
        internal int Unknown1;
        internal int Unknown2;

        internal byte ByteOfDeath;

        public Place_Troop(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }
        public Place_Troop(Device _Client) : base(_Client)
        {
            this.Identifier = 1;
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadVInt(); // 429
            this.Checksum = this.Reader.ReadVInt(); // 449
            
            if (this.Checksum != 127)
            {
                this.Reader.Seek(-1);
                this.Checksum = this.Reader.ReadVInt();
            }

            this.SenderID = this.Reader.ReadVInt(); // 5 - UserID
            this.SenderID = this.Reader.ReadVInt(); // 5 - UserID

            this.Unknown1 = this.Reader.Read();

            this.TroopType = this.Reader.ReadVInt(); // 26
            this.TroopID = this.Reader.ReadVInt(); // 3

            this.Unknown2 = this.Reader.Read();

            if (this.Unknown2 != 127)
            {
                this.Reader.Seek(-1);
                this.Reader.ReadVInt();
            }

            this.ByteOfDeath = this.Reader.ReadByte(); // 0

            if (ByteOfDeath == 3)
            {
                // Mmh
            }
            else if (ByteOfDeath == 2)
            {
                 // Do nothing
            }
            else
            {
                this.X = this.Reader.ReadVInt();
                this.Y = this.Reader.ReadVInt();
            }
        }

        internal override void Encode()
        {
            this.Data.AddVInt(this.Tick);
            this.Data.AddVInt(this.Checksum);

            this.Data.AddVInt(this.SenderID);

            this.Data.AddVInt(this.Unknown1);

            this.Data.AddVInt(this.TroopType);
            this.Data.AddVInt(this.TroopID);

            if (this.Unknown2 != 127)
                this.Data.AddVInt(this.Unknown2);
            else
                this.Data.Add((byte)this.Unknown2);

            this.Data.AddVInt(1);

            this.Data.AddVInt(this.TroopType * 1000000 + this.TroopID);

            this.Data.AddVInt(1); // Troop Level

            if (this.SenderID == this.Device.Player.UserId)
            {
                this.Data.AddVInt(this.X);
                this.Data.AddVInt(this.Y);
            }
            else
            {
                this.Data.AddVInt(17500 - this.X);
                this.Data.AddVInt(31500 - this.Y);
            }
        }

        internal override void Process()
        {
        }
    }
}
