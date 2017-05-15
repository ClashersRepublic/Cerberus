using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Commands.Server
{
    internal class Receive_Troop_Callback : Command
    {
        internal string DonatorName;
        internal int TroopLevel;
        internal int TroopGlobalId;

        public Receive_Troop_Callback(Device _client) : base(_client)
        {
            this.Identifier = 5;
        }

        internal override void Encode()
        {
            this.Data.AddString(this.DonatorName);
            this.Data.AddInt(0);
            this.Data.AddInt(this.TroopGlobalId);
            this.Data.AddInt(this.TroopLevel);
            this.Data.AddInt(1);
            this.Data.AddInt(0);
        }
    }
}