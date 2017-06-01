using System;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Commands.Client
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
