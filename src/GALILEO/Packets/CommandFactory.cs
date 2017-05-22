using System;
using System.Collections.Generic;
using BL.Servers.CoC.Packets.Commands.Client;
using BL.Servers.CoC.Packets.Commands.Client.Battle;
using BL.Servers.CoC.Packets.Commands.Client.Clan;

namespace BL.Servers.CoC.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {500, typeof(Buy_Building)},
                {501, typeof(Move_Building)},
                {502, typeof(Upgrade_Building)},
                {504, typeof(SpeedUp_Construction)},
                {508, typeof(Train_Unit)},
                {511, typeof(Request_Alliance_Troops)},
                {518, typeof(Buy_Resource)},
                {519, typeof(Mission_Progress)},
                {520, typeof(Unlock_Building)},
                {537, typeof(Send_Mail)},
                {538, typeof(My_League)},
                {549, typeof(Upgrade_Multiple_Buildings)},
                {574, typeof(Request_Amical_Challenge)},
                {577, typeof(Swap_Buildings)},
                {600, typeof(Place_Attacker)},
                {601, typeof(Place_Alliance_Attacker)},
                {603, typeof(Surrender_Attack)},
                {700, typeof(Search_Opponent)}
            };
        }
    }
}
