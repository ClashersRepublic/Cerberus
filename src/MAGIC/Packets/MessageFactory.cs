using System;
using System.Collections.Generic;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Packets.Client.Authentication;
using CRepublic.Magic.Packets.Messages.Client;
using CRepublic.Magic.Packets.Messages.Client.API;
using CRepublic.Magic.Packets.Messages.Client.Authentication;
using CRepublic.Magic.Packets.Messages.Client.Battle;
using CRepublic.Magic.Packets.Messages.Client.Clans;
using CRepublic.Magic.Packets.Messages.Client.Clans.War;
using CRepublic.Magic.Packets.Messages.Client.Leaderboard;

namespace CRepublic.Magic.Packets
{
    internal class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

        public MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(Pre_Authentication)},
                {10101, typeof(Authentication)},
                {10108, typeof(Keep_Alive)},
                {10121, typeof(Unlock_Account)},
                {10212, typeof(Change_Name)},
                {10513, typeof(Facebook_Friends)},
                {10905, typeof(News_Seen)},
                {14101, typeof(Go_Home)},
                {14102, typeof(Execute_Commands)},
                {14113, typeof(Ask_Visit_Home)},
                {14114, typeof(Get_Replay)},
                {14120, typeof(Attack_Alliance_Challange)},
                {14123, typeof(Search_Opponent)},
                {14134, typeof(Attack_NPC)},
                {14201, typeof(Facebook_Connect)},
                {14301, typeof(Create_Alliance)},
                {14302, typeof(Ask_Alliance)},
                {14262, typeof(Bind_Google_Account)},
                {14303, typeof(Joinable_Alliance)},
                {14305, typeof(Join_Alliance)},
                {14306, typeof(Change_Role)},
                {14308, typeof(Leave_Alliance)},
                {14310, typeof(Donate_Alliance_Unit)},
                {14315, typeof(Add_Alliance_Message)},
                {14316, typeof(Edit_Alliance_Settings)},
                {14317, typeof(Request_Join_Alliance)},
                {14321, typeof(Take_Decision)},
                {14325, typeof(Avatar_Profile)},
                {14401, typeof(Request_Global_Clans)},
                {14402, typeof(Request_Local_Clans)},
                {14403, typeof(Request_Global_Players)},
                {14404, typeof(Request_Local_Players)},
                {14406, typeof(Request_League_Players)},
                {14510, typeof(Execute_Battle_Commands)},
                {14600, typeof(Request_Name_Change)},
                {14715, typeof(Add_Global_Chat)},
                {15000, typeof(Request_War_Home_Data)},
                {15001, typeof(Attack_War)},
            };
        }
        internal static Message Parse(Device client, Reader reader, int messageId)
        {
            if (Messages.ContainsKey(messageId))
                return (Message)Activator.CreateInstance(Messages[messageId], client, reader);

            return null;
        }
    }
}