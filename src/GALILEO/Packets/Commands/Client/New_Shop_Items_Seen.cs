using System;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class New_Shop_Items_Seen : Command
    {
        public New_Shop_Items_Seen(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
        }
    }
}