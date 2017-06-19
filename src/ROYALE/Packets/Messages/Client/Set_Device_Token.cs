using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Library.Blake2B;
using CRepublic.Royale.Library.Sodium;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Client
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
