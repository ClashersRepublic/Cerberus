using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CoC.Packets.Messages.Client.API
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
                Console.WriteLine(this.Friends[i]);
            }
        }
        internal override void Process()
        {
            new Friend_List_Data(this.Device, this.Friends).Send();
        }
    }
}