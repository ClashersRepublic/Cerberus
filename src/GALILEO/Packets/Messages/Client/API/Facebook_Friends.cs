using Republic.Magic.Core.Networking;
using Republic.Magic.Extensions.Binary;
using Republic.Magic.Logic;
using Republic.Magic.Packets.Messages.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Republic.Magic.Packets.Messages.Client.API
{
    internal class Facebook_Friends : Message
    {
        internal List<string> Friends;

        public Facebook_Friends(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Facebook_Friends.
        }

        internal override void Decode()
        {
            int Count = this.Reader.ReadInt32();
            this.Friends = new List<string>(Count);

            for (int i = 0; i < Count; i++)
            {
                this.Friends.Add(this.Reader.ReadString());
            }
        }
        internal override void Process()
        {
            new Friend_List_Data(this.Device, this.Friends).Send();
        }
    }
}