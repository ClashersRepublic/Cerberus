using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Battle;

namespace BL.Servers.CoC.Packets.Messages.Client.Battle
{
    internal class Search_Opponent : Message
    {
        internal long Enemy_ID;
        internal bool Max_Seed_Achieved;
        internal Level Enemy_Player;

        public Search_Opponent(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Search_Opponent.
        }

        internal override void Process()
        {

            this.Device.Player.Avatar.Last_Attack_Enemy_ID.Clear();
            new Battle_Failed(this.Device).Send(); //No idea how to reply yet

        }
    }
}
