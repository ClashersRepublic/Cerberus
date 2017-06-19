using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Messages.Server.Errors;

namespace CRepublic.Magic.Packets.Commands.Client
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
               // Missions Mission = CSV.Tables.Get(Gamefile.Missions).GetDataWithID(Mission_ID) as Missions;
            }
            else
            {
                new Out_Of_Sync(this.Device).Send();
            }
        }
    }
}
