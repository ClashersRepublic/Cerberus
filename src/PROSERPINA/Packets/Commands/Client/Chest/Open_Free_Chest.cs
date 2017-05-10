using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CR.Core.Network;
using BL.Servers.CR.Extensions.Binary;
using BL.Servers.CR.Logic;
using BL.Servers.CR.Packets.Commands.Server.Chest;
using BL.Servers.CR.Packets.Messages.Server;

namespace BL.Servers.CR.Packets.Commands.Client.Chest
{
    internal class Open_Free_Chest : Command
    {
        internal int Chest_ID;
        internal int Tick;
        internal int Tick2;
        public Open_Free_Chest(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {

        }
        internal override void Decode()
        {
            this.Tick = this.Reader.ReadRRInt32();
            this.Tick2 = this.Reader.ReadRRInt32();
            this.Chest_ID = this.Reader.ReadRRInt32();
        }
        internal override void Process()
        {
            ShowValues();
            new Server_Commands(this.Device)
            {
                Command = new Buy_Chest_Callback(this.Device)
                {
                    ChestID = 225,
                }.Handle()
            }.Send();
        }

    }
}
