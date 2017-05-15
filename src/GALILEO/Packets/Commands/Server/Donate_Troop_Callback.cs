using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;
using System;

namespace BL.Servers.CoC.Packets.Commands.Server
{
    internal class Donate_Troop_Callback : Command
    {
        internal int SteamHighId;
        internal int StreamLowId;
        internal int TroopGlobalId;

        public Donate_Troop_Callback(Device _client) : base(_client)
        {
            this.Identifier = 4;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.SteamHighId);
            this.Data.AddInt(this.StreamLowId);
            this.Data.AddInt(0);
            this.Data.AddInt(this.TroopGlobalId);
            this.Data.AddInt(4);
            this.Data.AddInt((int)DateTime.UtcNow.Subtract(this.Device.Player.Avatar.LastTick).TotalSeconds + 1); // Tick
        }
    }
}

