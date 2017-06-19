using System;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Commands.Client
{
    internal class News_Seen : Command
    {
        public News_Seen(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }
    }
}
