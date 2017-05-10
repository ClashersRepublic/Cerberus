using System;
using System.Collections.Generic;
using BL.Servers.CR.Packets.Commands.Client.Battles;
using BL.Servers.CR.Packets.Commands.Client.Cards;
using BL.Servers.CR.Packets.Commands.Client.Chest;
using BL.Servers.CR.Packets.Commands.Server.Chest;


namespace BL.Servers.CR.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {1, typeof(Place_Troop)},
                {505, typeof(Open_Free_Chest)},
                {516, typeof(Buy_Chest)},
                {525, typeof(Search_Battle)},
                {526, typeof(Next_Card)},
                {545, typeof(New_Card_Seen_Deck)}
            };
        }
    }
}
