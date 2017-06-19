using System;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Commands.Server
{
    using CRepublic.Royale.Extensions.List;

    internal class Name_Change_Callback : Command
    {
        internal string Name = string.Empty;
        internal string Previous = string.Empty;
        internal int Tick = 0;

        public Name_Change_Callback(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Name_Change_Callback.
        }


        public Name_Change_Callback(Device _Client) : base(_Client)
        {
            this.Identifier = 201;
        }
        
        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
            this.Reader.ReadInt32();
            this.Reader.ReadByte();
            this.Reader.ReadByte();
            this.Tick = this.Reader.ReadVInt();
            this.Tick = this.Reader.ReadVInt();
            this.Reader.ReadInt16();
        }

        internal override void Encode()
        {
            this.Data.AddString(this.Device.Player.Username);
            this.Data.AddInt(1);
        }

        internal override void Process()
        {
        }
    }
}
