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
using Newtonsoft.Json;

namespace BL.Servers.CR.Packets.Commands.Client.Chest
{
    internal class Buy_Chest : Command
    {
        internal int Chest_ID;
        internal int Tick;

        public Buy_Chest(Reader Reader, Device Device, int ID) : base(Reader, Device, ID)
        {

        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadVInt();
            this.Tick = this.Reader.ReadVInt();
            this.Reader.ReadInt16();

            this.Reader.ReadVInt();
            this.Chest_ID = this.Reader.ReadVInt();
        }

        internal override void Process()
        {
            ShowValues();
            new Server_Commands(this.Device)
            {
                Command = new Buy_Chest_Callback(this.Device)
                {
                    ChestID = Chest_ID,
                }.Handle()
            }.Send();
        }
    }
}
