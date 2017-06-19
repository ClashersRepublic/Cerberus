using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Core;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Packets.Messages.Client.Battle
{
    internal class Sector_Command : Message
    {
        internal int Command_Count = 0;
        public Sector_Command(Device Device, Reader Reader) : base(Device, Reader)
        {
        }
        
        internal override void Decode()
        {
            // EE-F0-D0-C0-0C  29  00
            //                       01-8F-01-7F-00-12-07-8F-EA-E5-18-00-9C-94-01-84-9E-03
            //                       01-8F-01-7F-00-12-07-8F-EA-E5-18-00-9C-94-01-84-9E-03
            //                       01-8F-01-7F-00-10-02-8D-EA-E5-18-00-B4-07-A4-DC-03
            //                       01-8F-01-7F-00-10-00-80-EA-E5-18-00-94-C3-01-BC-CC-03
            //                     01-01-8F-01-7F-00-10-00-80-EA-E5-18-00-8C-F2-01-A4-DC-03

            this.Reader.ReadVInt();
            this.Reader.ReadVInt();
            this.Command_Count = this.Reader.ReadVInt();

            if (this.Command_Count > 0 && this.Command_Count < 10)
            {
                for (int _Index = 0; _Index < this.Command_Count; _Index++)
                {
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                    this.Reader.ReadVInt();
                }
            }
        }

        internal override void Process()
        {
            Server_Resources.Battles[this.Device.Player.BattleID].Begin();
        }

    }
}
