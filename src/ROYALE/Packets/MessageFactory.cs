using System;
using System.Collections.Generic;
using BL.Servers.CR.Packets.Messages.Client;
using BL.Servers.CR.Packets.Messages.Client.Alliance;
using BL.Servers.CR.Packets.Messages.Client.Authentication;
using BL.Servers.CR.Packets.Messages.Client.Battle;

namespace BL.Servers.CR.Packets
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
                {10107, typeof(Client_Capabilities)},
                {10108, typeof(Keep_Alive)},
                {10113, typeof(Set_Device_Token)},
                {10121, typeof(Unlock_Account)},
                {10212, typeof(Change_Name) },
                {12904, typeof(Sector_Command)},
                {12951, typeof(Battle_Commands)},
                {14358, typeof(Request_Create_Alliance)},
                {14101, typeof(Go_Home)},
                {14102, typeof(Execute_Commands)},
                {14104, typeof(Battle_NPC)},
                {14107, typeof(Cancel_Battle)},
                {14405, typeof(Avatar_Stream)},
                {14406, typeof(Battle_Stream)},
                {14600, typeof(Request_Name_Change)},
                {16103, typeof(Joinable_Tournaments)}
            };

            // 25006 = Live Clan Battle Notification, 

            //Messages.Add(10513, typeof(UnknownFacebookMessage));
            //Messages.Add(14262, typeof(BindGoogleAccount));
        }
    }
}