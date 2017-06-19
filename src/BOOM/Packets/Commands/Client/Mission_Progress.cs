using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Extensions;
using CRepublic.Boom.Extensions.Binary;
using CRepublic.Boom.Files;
using CRepublic.Boom.Files.CSV_Logic;
using CRepublic.Boom.Logic;
using CRepublic.Boom.Logic.Enums;

namespace CRepublic.Boom.Packets.Commands.Client
{

    internal class Mission_Progress : Command
    {
        internal int Mission_ID;

        public Mission_Progress(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {

        }

        internal override void Decode()
        {
            this.Mission_ID = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            if (this.Device.Player.Avatar.Mission_Finish(Mission_ID))
            {
                Console.WriteLine(Mission_ID);
                Missions Mission = CSV.Tables.Get(Gamefile.Missions).GetDataWithID(Mission_ID) as Missions;

                Console.WriteLine($"Mission {Mission.Name} done");
            }
        }
    }
}
