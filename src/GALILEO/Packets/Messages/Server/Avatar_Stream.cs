using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server
{
    internal class Avatar_Stream : Message
    {
        public Avatar_Stream(Device client) : base(client)
        {
            this.Identifier = 24411;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.Device.Player.Avatar.Stream.Count);

            foreach (long[] Stream in this.Device.Player.Avatar.Stream)
            {
                if (Stream[1] == 2 || Stream[1] == 7)
                {
                    var Battle = Resources.Battles.Get(Stream[0], Constants.Database, false);

                    if (Battle != null)
                    {
                        this.Data.AddInt((int) Stream[1]); // 2 : Défense PVP    3 : Demande refusé      4 : Invitation    5 : Exclusion du clan       6 : Message Clan         7 : Attaque PVP
                        this.Data.AddLong(Battle.Battle_ID); // ?
                        this.Data.AddBool(true);

                        if (Stream[1] == 7)
                        {
                            this.Data.AddLong(Battle.Defender.UserId); // Enemy ID
                            this.Data.AddString(Battle.Defender.Name);
                        }
                        else
                        {
                            this.Data.AddLong(Battle.Attacker.UserId); // Enemy ID
                            this.Data.AddString(Battle.Attacker.Name);
                        }

                        this.Data.AddInt(1);
                        this.Data.AddInt(0);
                        this.Data.AddInt(446);

                        this.Data.AddByte(0);
                        Console.WriteLine(Battle.Replay_Info.Json);
                        this.Data.AddString(Battle.Replay_Info.Json);
                        this.Data.AddInt(0);

                        this.Data.AddBool(true);
                        this.Data.AddInt(8);
                        this.Data.AddInt(709);
                        this.Data.AddInt(0);

                        this.Data.AddBool(true);
                        this.Data.AddLong(Battle.Battle_ID);
                        this.Data.AddInt(int.MaxValue);
                    }
                }
            }
        }
    }
}
