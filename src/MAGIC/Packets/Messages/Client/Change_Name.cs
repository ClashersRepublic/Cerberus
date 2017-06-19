using System;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Commands.Server;
using CRepublic.Magic.Packets.Messages.Server;
using CRepublic.Magic.Logic.Enums;

namespace CRepublic.Magic.Packets.Messages.Client
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
            this.Reader.ReadByte();
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
