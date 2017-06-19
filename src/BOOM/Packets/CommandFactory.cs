using System;
using System.Collections.Generic;
using CRepublic.Boom.Packets.Commands.Client;


namespace CRepublic.Boom.Packets
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
                {508, typeof(Train_Unit) },
                {504, typeof(SpeedUp_Construction) },
                {595, typeof(Mission_Progress) },
                {700, typeof(HackBits) }
            };

            //Commands.Add(0, typeof(UnknownCommand));
            //Commands.Add(1, typeof(JoinAlliance));
            //Commands.Add(2, typeof(LeaveAllianceCommand));
            //Commands.Add(3, typeof(ChangeAvatarCommand));
            //Commands.Add(5, typeof());
            //Commands.Add(550, typeof(RemoveUnitsCommand));
            //Commands.Add(551, typeof(ContinueBarrackBoostCommand));
            //Commands.Add(563, typeof(CollectClanResourcesCommand));
            //Commands.Add(573, typeof(RemoveShieldToAttackCommand));
        }
    }
}
