using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Client.Alliance
{
    internal class Send_Clan_Message : Message
    {
        internal string Message;

        public Send_Clan_Message(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
        }

        internal override void Process()
        {

        }
    }
}
