using System;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;

namespace BL.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Replay_Data : Message
    {
        internal long Battle_ID = 0;

        public Replay_Data(Device _Device) : base(_Device)
        {
            this.Identifier = 24114;
        }

        internal override async void Encode()
        {
            var Battle = await Core.Resources.Battles.Get(this.Battle_ID, Constants.Database, false);
            if (Battle != null)
                this.Data.AddCompressed(Battle.Json, false);
        }
    }
}
