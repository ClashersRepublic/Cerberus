using System;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;

namespace BL.Servers.CR.Packets.Commands.Server
{
    using BL.Servers.CR.Extensions.List;

    internal class Name_Change_Callback : Command
    {
        public string Name = string.Empty;
        public string Previous = string.Empty;

        public Name_Change_Callback(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
            // Name_Change_Callback.
        }


        public Name_Change_Callback(Device _Client) : base(_Client)
        {
            this.Identifier = 201;
        }
        
        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
            this.Previous = Name;
        }

        internal override void Encode()
        {
            //this.Data.AddHexa("00-00-00-03-61-73-64-00-00-00-01-00-00-86-02-86-02-00-11-FF-FF-FF-FF".Replace("-", ""));

            this.Data.AddString(this.Device.Player.Username);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddBool(true);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0);
            this.Data.AddVInt(0); // Tick
            this.Data.AddVInt(0); // Tick
            this.Data.AddUShort(0);
        }

        internal override void Process()
        {
            Console.WriteLine(this.Device.Player.Username);
        }
    }
}
