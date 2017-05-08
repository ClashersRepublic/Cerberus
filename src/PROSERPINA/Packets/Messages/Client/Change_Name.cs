using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Messages.Client
{
    using System;
    using BL.Servers.CR.Core.Network;
    using BL.Servers.CR.Extensions.Binary;
    using BL.Servers.CR.Packets.Commands.Server;
    using BL.Servers.CR.Packets.Messages.Server;
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
            this.Reader.ReadBoolean();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            if (!string.IsNullOrEmpty(this.Name) && this.Name.Length < 16)
            {
                this.Device.Player.Avatar.Username = this.Name;

                new Server_Commands(this.Device, new Name_Change_Callback(this.Device).Handle()).Send();
            }
          //  else
            //    new Change_Name_Failed(this.Device).Send();
        }
    }
}
