using System;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Client.Battles
{
    internal class Place_Troop : Command
    {

        internal int SenderHigh;
        internal int SenderLow;
        internal int TroopID;
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
            //Console.WriteLine(BitConverter.ToString(Reader.ReadFully()));

            this.Reader.ReadVInt();
            this.Reader.ReadVInt();

            this.Reader.ReadVInt();

            this.TroopID = this.Reader.ReadVInt();

            this.Reader.ReadInt16();

            this.Reader.ReadVInt();
            this.Reader.ReadInt16();

            this.X = this.Reader.ReadVInt();
            this.Y = this.Reader.ReadVInt();
        }

        internal override void Encode()
        {

            this.Data.AddRange("01-A8-01-7F".HexaToBytes()); // Pretty sure this is check sum actually xD
            this.Data.AddVInt(this.SenderHigh); // 11-BB-AE-F2-
            this.Data.AddVInt(this.SenderLow); // 11-BB-AE-F2-06
            this.Data.AddRange("05-83-EA-E5-18-01".HexaToBytes());
            this.Data.AddRange("1A-03".HexaToBytes());
            this.Data.AddRange("00-00-01-00-09-00-94-46-8C-EF-02".HexaToBytes());

        }

        internal override void Process()
        {
            //ShowValues(); //Nothing will show because there is no constructor
        }
    }
}
