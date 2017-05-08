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
                {504, typeof(SpeedUp_Construction)},
                {508, typeof(Train_Unit) },
                {519, typeof(Mission_Progress) },
                {538, typeof(My_League) },
                {600, typeof(Place_Attacker) },
            };
        }
    }
}
