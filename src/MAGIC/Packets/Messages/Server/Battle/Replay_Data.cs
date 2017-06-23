using System;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;

namespace CRepublic.Magic.Packets.Messages.Server.Battle
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
            if (this.Device.Player.Avatar.Variables.IsBuilderVillage)
            {
                var Battle = Core.Resources.Battles.Get(this.Battle_ID, Constants.Database, false);
                if (Battle != null)
                    this.Data.AddCompressed(Battle.Json, false);
            }
            else
            {
                var Battle = Core.Resources.Battles_V2.GetPlayer(this.Device.Player.Avatar.Battle_ID_V2, this.Device.Player.Avatar.UserId);
                if (Battle != null)
                    this.Data.AddCompressed(Battle.Json, false);
            }
        }
    }
}
