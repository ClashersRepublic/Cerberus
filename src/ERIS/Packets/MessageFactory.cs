using System;
using System.Collections.Generic;
using BL.Servers.BB.Packets.Messages.Client;
using BL.Servers.BB.Packets.Messages.Client.Alliance;
using BL.Servers.BB.Packets.Messages.Client.Authentication;

namespace BL.Servers.BB.Packets
{
    internal class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

        public MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(Pre_Authentification)},
                {10101, typeof(Authentification)},
                {10121, typeof(Unlock_Account)},
                {10212, typeof(Change_Name) },
                {14113, typeof(Visit_Home) },
                {14135, typeof(Visit_Npc) },
                {14358, typeof(Request_Create_Alliance) },
                {14102, typeof(Execute_Commands)},
            };

            // 25006 = Live Clan Battle Notification, 

            //Messages.Add(10513, typeof(UnknownFacebookMessage));
            //Messages.Add(14262, typeof(BindGoogleAccount));
        }
    }
}