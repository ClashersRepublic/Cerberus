using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Commands.Server;
using BL.Servers.CoC.Packets.Messages.Server;

namespace BL.Servers.CoC.Packets.Messages.Client
{
    internal class Change_Name : Message
    {
        internal string Name;
        public Change_Name(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Change_Name.
        }
        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
            this.Reader.Read();
        }
        internal override void Process()
        {
            if (!String.IsNullOrEmpty(this.Name) && this.Name.Length < 16)
                //if (!GameTools.Badwords.Contains(this.Name))
            {
                this.Device.Player.Avatar.Name = this.Name;
                this.Device.Player.Avatar.NameState += 1;

                new Server_Commands(this.Device, new Name_Change_Callback(this.Device)).Send();
            }
           // else
             //   new Change_Name_Failed(this.Device).Send();
        }
    }
}
