using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Client.Battle
{
    internal class Battle_End : Message
    {
        public Battle_End(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            Console.WriteLine(BitConverter.ToString(Reader.ReadFully()));
        }

        internal override void Process()
        {
            Console.WriteLine("Spell ICUP NIGGGERRR!");
        }
    }
}
