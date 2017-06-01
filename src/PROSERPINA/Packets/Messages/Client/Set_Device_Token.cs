using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Library.Blake2B;
using BL.Servers.CR.Library.Sodium;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Client
{
    internal class Set_Device_Token : Message
    {
        internal string Password;

        public Set_Device_Token(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Decode()
        {
            Console.WriteLine(this.Password = this.Reader.ReadString());
        }

        internal override void Process()
        {
        }
    }
}
