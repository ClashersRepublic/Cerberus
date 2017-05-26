using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Packets.Commands.Client.Tournaments
{
    internal class Create_Custom_Tournament : Command
    {
        // 531

        public Create_Custom_Tournament(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Card_Seen.
        }

        internal override void Decode()
        {
            Console.WriteLine(BitConverter.ToString(Reader.ReadFully()));
        }
    }
}
