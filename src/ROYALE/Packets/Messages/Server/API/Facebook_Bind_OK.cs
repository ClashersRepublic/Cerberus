using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Server.API
{
    internal class Facebook_Bind_OK : Message
    {
        public Facebook_Bind_OK(Device Device) : base(Device)
        {
            this.Identifier = 24201;
        }

        internal override void Encode()
        {
            this.Data.Add(1);
        }
    }
}
