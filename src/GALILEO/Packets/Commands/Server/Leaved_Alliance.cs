using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Commands.Server
{
    internal class Leaved_Alliance : Command
    {
        internal long AllianceID;
        internal int Reason;

        public Leaved_Alliance(Device _Client) : base(_Client)
        {
            this.Identifier = 2;
        }

        internal override void Encode()
        {
            this.Data.AddLong(this.AllianceID);
            this.Data.AddInt(this.Reason); //1 = leave, 2 = kick (Not worth it to make an enum for this)
            this.Data.AddInt(-1);
        }
    }
}