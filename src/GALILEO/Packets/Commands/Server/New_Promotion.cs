using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;

namespace Republic.Magic.Packets.Commands.Server
{
    internal class New_Promotion : Command
    {
        public New_Promotion(Device _Client) : base(_Client)
        {
            this.Identifier = 21;
        }

        internal override void Encode()
        {
            this.Data.AddInt(1);
            this.Data.AddString(null);
            this.Data.AddInt(0);
        }
    }
}