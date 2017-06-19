using CRepublic.Boom.Logic;

namespace CRepublic.Boom.Packets.Messages.Client
{
    using System;
    using CRepublic.Boom.Core.Network;
    using CRepublic.Boom.Extensions.Binary;
    using CRepublic.Boom.Packets.Commands.Server;
    using CRepublic.Boom.Packets.Messages.Server;
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
