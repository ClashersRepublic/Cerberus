using System;
using System.Collections.Generic;
using BL.Servers.CoC.Packets.Client.Authentication;
using BL.Servers.CoC.Packets.Messages.Client;
using BL.Servers.CoC.Packets.Messages.Client.API;
using BL.Servers.CoC.Packets.Messages.Client.Authentication;
using BL.Servers.CoC.Packets.Messages.Client.Battle;
using BL.Servers.CoC.Packets.Messages.Client.Clans;
using BL.Servers.CoC.Packets.Messages.Client.Clans.War;
using BL.Servers.CoC.Packets.Messages.Client.Leaderboard;

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
                {10212, typeof(Change_Name)},
                {10513, typeof(Facebook_Friends)},
                {14101, typeof(Go_Home)},
                {14102, typeof(Execute_Commands)},
                {14113, typeof(Ask_Visit_Home)},
                {14114, typeof(Get_Replay)},
                {14134, typeof(Attack_NPC)},
                {14201, typeof(Facebook_Connect)},
                {14301, typeof(Create_Alliance)},
                {14302, typeof(Ask_Alliance)},
                {14262, typeof(Bind_Google_Account)},
                {14303, typeof(Joinable_Alliance)},
                {14305, typeof(Join_Alliance)},
                {14308, typeof(Leave_Alliance)},
                {14310, typeof(Donate_Alliance_Unit)},
                {14315, typeof(Add_Alliance_Message)},
                {14316, typeof(Edit_Alliance_Settings)},
                {14325, typeof(Avatar_Profile)},
                {14401, typeof(Request_Global_Clans)},
                {14402, typeof(Request_Local_Clans)},
                {14403, typeof(Request_Global_Players)},
                {14404, typeof(Request_Local_Players)},
                {14406, typeof(Request_League_Players)},
                {14600, typeof(Request_Name_Change)},
                {14715, typeof(Add_Global_Chat)},
                {15000, typeof(Request_War_Home_Data)},
                {15001, typeof(Attack_War)},
            };
        }
    }
}