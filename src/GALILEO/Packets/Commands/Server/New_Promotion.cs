using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Commands.Server
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