﻿using System;
using System.Collections.Generic;
using CRepublic.Magic.Packets.Commands.Server;
using CRepublic.Magic.Packets.Commands.Client;
using CRepublic.Magic.Packets.Commands.Client.Battle;
using CRepublic.Magic.Packets.Commands.Client.Clan;

namespace CRepublic.Magic.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {1, typeof(Joined_Alliance)},
                {2, typeof(Leaved_Alliance)},
                {3, typeof(Name_Change_Callback)},
                {4, typeof(Donate_Troop_Callback)},
                {5, typeof(Receive_Troop_Callback)},
                {6, typeof(Changed_Alliance_Setting)},
                {8, typeof(Role_Update)},
                {21, typeof(New_Promotion)},
                
                {500, typeof(Buy_Building)},
                {501, typeof(Move_Building)},
                {502, typeof(Upgrade_Building)},
                {504, typeof(SpeedUp_Construction)},
                {506, typeof(Collect_Resources)},
                {507, typeof(Remove_Obstacle)},
                {508, typeof(Train_Unit)},
                {510, typeof(Buy_Trap)},
                {511, typeof(Request_Alliance_Troops)},
                {512, typeof(Buy_Deco)},
                {516, typeof(Upgrade_Unit)},
                {517, typeof(SpeedUp_Upgrade_Unit)},
                {518, typeof(Buy_Resource)},
                {519, typeof(Mission_Progress)},
                {520, typeof(Unlock_Building)},
                {521, typeof(Free_Worker)},
                {524, typeof(Change_Weapon_Mode)},
                {526, typeof(Boost_Building)},
                {527, typeof(Upgrade_Hero)},
                {528, typeof(SpeedUp_Hero_Upgrade)},
                {584, typeof(Boost_Building)},
                {533, typeof(Move_Multiple_Buildings)},
                {537, typeof(Send_Mail)},
                {532, typeof(New_Shop_Items_Seen)},
                {538, typeof(My_League)},
                {539, typeof(News_Seen)},
                {549, typeof(Upgrade_Multiple_Buildings)},
                {554, typeof(Change_Weapon_Heading)},
                {574, typeof(Request_Amical_Challenge)},
                {577, typeof(Swap_Buildings)},
                {590, typeof(Buy_Multiple_Wall)},
                {591, typeof(Change_Village_Mode)},
                {592, typeof(Train_Unit_V2)},
                {600, typeof(Gear_Up)},
                {601, typeof(Search_Opponent_2)},
                {700, typeof(Place_Attacker)},
                {701, typeof(Place_Alliance_Attacker)},
                {703, typeof(Surrender_Attack)},
                {704, typeof(Place_Spell)},
                {705, typeof(Place_Hero)},
                {706, typeof(Hero_Rage)},
                {800, typeof(Search_Opponent)}
            };
        }
    }
}
