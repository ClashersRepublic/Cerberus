using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Republic.Magic.Packets.Messages.Server.Errors
{
    internal class Server_Error : Message
    {

        internal StringBuilder Reason = new StringBuilder();
        public Server_Error(Device _Device) : base(_Device)
        {

            this.Identifier = 24115;

            this.Reason.AppendLine("Your game threw an exception on our servers, please contact one of the Barbarianland Staff.");
            this.Reason.AppendLine("Your Player Name : " + this.Device.Player.Avatar.Name + ".");
            this.Reason.AppendLine("Your Player ID : " + this.Device.Player.Avatar.UserId + ".");
            this.Reason.AppendLine("Your IPAddress : " + this.Device.Socket.RemoteEndPoint + ".");
        }

        internal override void Encode()
        {
            this.Data.AddString(this.Reason.ToString());
        }
    }
}