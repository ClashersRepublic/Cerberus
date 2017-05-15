using System;
using System.Collections.Generic;
using BL.Servers.CoC.Packets.Commands.Client;
using BL.Servers.CoC.Packets.Commands.Client.Battle;

namespace BL.Servers.CoC.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {500, typeof(Buy_Building) },
                {502, typeof(Upgrade_Building) },
                {504, typeof(SpeedUp_Construction)},
                {508, typeof(Train_Unit) },
                {511, typeof(Request_Alliance_Troops)},
                {519, typeof(Mission_Progress) },
                {520, typeof(Unlock_Building)},
                {538, typeof(My_League) },
                {574, typeof(Request_Amical_Challenge)},
                {600, typeof(Place_Attacker) },
                {700, typeof(Search_Opponent) }
            };
        }
    }
}
