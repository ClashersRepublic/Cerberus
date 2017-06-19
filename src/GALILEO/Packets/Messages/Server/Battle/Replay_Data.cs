using System;
using Republic.Magic.Logic;
using Republic.Magic.Extensions;
using Republic.Magic.Extensions.List;

namespace Republic.Magic.Packets.Messages.Server.Battle
{
    internal class Replay_Data : Message
    {
        internal long Battle_ID = 0;

        public Replay_Data(Device _Device) : base(_Device)
        {
            this.Identifier = 24114;
        }

        internal override void Encode()
        {
            var Battle = Core.Resources.Battles.Get(this.Battle_ID, Constants.Database, false);
            if (Battle != null)
                this.Data.AddCompressed(Battle.Json, false);
        }
    }
}
