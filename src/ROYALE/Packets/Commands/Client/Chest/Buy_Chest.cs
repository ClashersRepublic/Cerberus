using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Commands.Server.Chest;
using CRepublic.Royale.Packets.Messages.Server;
using Newtonsoft.Json;

namespace CRepublic.Royale.Packets.Commands.Client.Chest
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
