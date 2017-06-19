using System;
using CRepublic.Boom.Files;
using CRepublic.Boom.Files.CSV_Logic;
using CRepublic.Boom.Logic;
using CRepublic.Boom.Logic.Enums;
using CRepublic.Boom.Logic.Slots;

namespace CRepublic.Boom.Extensions
{
    internal static class GameTools
    {
        internal static void AddExperience(this Avatar User, int Value)
        {
            if (Value > 0)
            {
                User.Experience += Value;

                if (User.Level < 80)
                {
                    Experience_Levels _ExperienceLevels =
                        CSV.Tables.Get(Gamefile.Experience_Levels).GetDataWithID(User.Level - 1) as Experience_Levels;

                    if (_ExperienceLevels.ExpPoints <= User.Experience)
                    {
                        User.Experience -= _ExperienceLevels.ExpPoints;
                        User.Level++;
                    }
                }
            }
        }
        public static bool Mission_Finish(this Avatar Player, int Global_ID)
        {
            Console.WriteLine(Player.Tutorials.FindIndex(M => M == Global_ID));
            if (Player.Tutorials.FindIndex(M => M == Global_ID) < 0)
            {
                Missions Mission = CSV.Tables.Get(Gamefile.Missions).GetDataWithID(Global_ID) as Missions;

                Player.Tutorials.Add(Mission.GetGlobalID());
                return true;
            }
            else
                return false;
        }

        internal static int GetResourceDiamondCost(int Count, int _Index)
        {
            int Total_Gems = 0;
            if (Count >= 100)
            {
                if (Count >= 1000)
                {
                    if (Count >= 10000)
                    {
                        if (Count >= 100000)
                        {
                            if (Count >= 1000000)
                            {
                                int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_10000000") as
                                    Globals).NumberValue;
                                int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_1000000") as
                                    Globals).NumberValue;

                                Total_Gems =
                                    (int)
                                    Math.Round((SupCost - Inf_Cost) * (long) (Count - 1000000) /
                                               (10000000 - 1000000 * 1.0)) + Inf_Cost;
                            }
                            else
                            {
                                int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_1000000") as
                                    Globals).NumberValue;
                                int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_100000") as
                                    Globals).NumberValue;

                                Total_Gems =
                                    (int)
                                    Math.Round((SupCost - Inf_Cost) * (long) (Count - 100000) /
                                               (1000000 - 100000 * 1.0)) + Inf_Cost;
                            }
                        }
                        else
                        {
                            int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_100000") as Globals)
                                .NumberValue;
                            int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_10000") as Globals)
                                .NumberValue;

                            Total_Gems =
                                (int)
                                Math.Round((SupCost - Inf_Cost) * (long) (Count - 10000) / (100000 - 10000 * 1.0)) +
                                Inf_Cost;
                        }
                    }
                    else
                    {
                        int SupCost =
                            (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_10000") as Globals)
                            .NumberValue;
                        int Inf_Cost =
                            (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_1000") as Globals)
                            .NumberValue;

                        Total_Gems =
                            (int) Math.Round((SupCost - Inf_Cost) * (long) (Count - 1000) / (10000 - 1000 * 1.0)) +
                            Inf_Cost;
                    }
                }
                else
                {
                    int SupCost =
                        (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_1000") as Globals)
                        .NumberValue;
                    int Inf_Cost =
                        (CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_100") as Globals)
                        .NumberValue;

                    Total_Gems =
                        (int) Math.Round((SupCost - Inf_Cost) * (long) (Count - 100) / (1000 - 100 * 1.0)) +
                        Inf_Cost;
                }
            }
            else
            {
                Total_Gems = 1;
            }
            return Total_Gems;
        }


        internal static int GetSpeedUpCost(int Total_Seconds)
        {
            int Total_Gems = 0;
            if (Total_Seconds >= 1)
            {
                if (Total_Seconds >= 60)
                {
                    if (Total_Seconds >= 3600)
                    {
                        if (Total_Seconds >= 86400)
                        {
                            int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_WEEK") as Globals)
                                .NumberValue;
                            int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_24_HOURS") as Globals)
                                .NumberValue;

                            Total_Gems =
                                (int)((SupCost - Inf_Cost) * (long)(Total_Seconds - 86400) / (604800 - 86400 * 1.0)) +
                                Inf_Cost;
                        }
                        else
                        {
                            int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_24_HOURS") as Globals)
                                .NumberValue;
                            int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_HOUR") as Globals)
                                .NumberValue;

                            Total_Gems =
                                (int)((SupCost - Inf_Cost) * (long)(Total_Seconds - 3600) / (86400 - 3600 * 1.0)) +
                                Inf_Cost;
                        }
                    }
                    else
                    {
                        int SupCost =
                            (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_HOUR") as Globals)
                            .NumberValue;
                        int Inf_Cost =
                            (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_MIN") as Globals)
                            .NumberValue;

                        Total_Gems = (int)((SupCost - Inf_Cost) * (long)(Total_Seconds - 60) / (3600 - 60 * 1.0)) +
                                     Inf_Cost;
                    }
                }
                else
                {
                    Total_Gems =
                        (CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_MIN") as Globals).NumberValue;
                }
            }
            return Total_Gems;
        }
    }
}
