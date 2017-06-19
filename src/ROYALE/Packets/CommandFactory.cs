using System;
using System.Collections.Generic;
using CRepublic.Royale.Packets.Commands.Client.Battles;
using CRepublic.Royale.Packets.Commands.Client.Cards;
using CRepublic.Royale.Packets.Commands.Client.Chest;
using CRepublic.Royale.Packets.Commands.Server;
using CRepublic.Royale.Packets.Commands.Server.Chest;
using CRepublic.Royale.Packets.Commands.Client.Tournaments;

namespace CRepublic.Royale.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {1, typeof(Place_Troop)},
                {201, typeof(Name_Change_Callback)},
                {210, typeof(Buy_Chest_Callback)},
                {500, typeof(Move_Card)},
                {504, typeof(Upgrade_Card)},
                {505, typeof(Open_Free_Chest)},
                {513, typeof(Card_Seen)},
                {516, typeof(Buy_Chest)},
                {525, typeof(Search_Battle)},
                {526, typeof(Next_Card)},
                {531, typeof(Create_Custom_Tournament)},
                {545, typeof(New_Card_Seen_Deck)}
            };
        }
    }
}
