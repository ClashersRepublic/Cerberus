using BL.Servers.BB.Logic;

namespace BL.Servers.BB.Packets.Messages.Client
{
    using System;
    using BL.Servers.BB.Core.Network;
    using BL.Servers.BB.Extensions.Binary;
    using BL.Servers.BB.Packets.Commands.Server;
    using BL.Servers.BB.Packets.Messages.Server;
    internal class Change_Name : Message
    {
        internal string Name; 
        public Change_Name(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Change_Name.
        }

        /// <summary>
        /// Decodes this message.
        /// </summary>
        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
            this.Reader.Read();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (!string.IsNullOrEmpty(this.Name) && this.Name.Length < 16)

                //if (!GameTools.Badwords.Contains(this.Name))
            {
                this.Device.Player.Avatar.Username = this.Name;

                new Server_Commands(this.Device, new Name_Change_Callback(this.Device).Handle()).Send();
            }
          //  else
            //    new Change_Name_Failed(this.Device).Send();
        }
    }
}
