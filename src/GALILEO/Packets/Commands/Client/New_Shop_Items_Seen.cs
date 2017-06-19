using System;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;

namespace CRepublic.Magic.Packets.Commands.Client
{
    internal class New_Shop_Items_Seen : Command
    {
        public New_Shop_Items_Seen(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }
    }
}