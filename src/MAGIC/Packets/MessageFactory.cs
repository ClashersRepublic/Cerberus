using System;
using System.Collections.Generic;
using BL.Servers.CoC.Packets.Client.Authentication;
using BL.Servers.CoC.Packets.Messages.Client;
using BL.Servers.CoC.Packets.Messages.Client.Authentication;
using BL.Servers.CoC.Packets.Messages.Client.Battle;

namespace BL.Servers.CoC.Packets
{
    internal class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

        public MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(Pre_Authentication)},
                {10101, typeof(Authentification)},
                {10108, typeof(Keep_Alive)},
                {10121, typeof(Unlock_Account)},
                {10212, typeof(Change_Name) },
                {14325, typeof(Avatar_Profile)},
                {14101, typeof(Go_Home) },
                {14102, typeof(Execute_Commands) },
                {14134, typeof(Attack_NPC)},
                {14715, typeof(Add_Global_Chat) },
            };
        }
    }
}
