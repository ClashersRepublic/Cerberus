using System;
using System.Linq;
using System.Text;
using BL.Servers.CoC.Files;
using BL.Servers.CoC.Files.CSV_Logic;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Enums;

namespace BL.Servers.CoC.Extensions
{
    internal static class GameUtils
    {
        internal const int SEARCH_TAG_LENGTH = 14;

        internal static readonly char[] SEARCH_TAG_CHARS = "0289PYLQGRJCUV".ToCharArray();
        
        internal static string GetHashtag(long Identifier)
        {
            if (GameUtils.GetHighID(Identifier) <= 255)
            {
                StringBuilder Stringer = new StringBuilder();
                int Count = 11;
                Identifier = ((long)GameUtils.GetLowID(Identifier) << 8) + GameUtils.GetHighID(Identifier);

                while (++Count > 0)
                {
                    Stringer.Append(GameUtils.SEARCH_TAG_CHARS[(int)(Identifier % GameUtils.SEARCH_TAG_LENGTH)]);
                    Identifier /= GameUtils.SEARCH_TAG_LENGTH;
                    if (Identifier <= 0)
                    {
                        break;
                    }
                }

                return new string(Stringer.Append("#").ToString().Reverse().ToArray());
            }

            return string.Empty;
        }

        internal static int GetLowID(long Identifier)
        {
            return (int)(Identifier & 0xFFFFFFFF);
        }

        internal static int GetHighID(long Identifier)
        {
            return (int)(Identifier >> 32);
        }

        internal static double WinTrophies(/*this Battle _Battle*/int AttackerTrophies, int DefenderTrophies)
        {
            //double Difference = (_Battle.Attacker.Trophies - _Battle.Defender.Trophies) < 0 ? +(_Battle.Attacker.Trophies - _Battle.Defender.Trophies) : (_Battle.Attacker.Trophies - _Battle.Defender.Trophies);
            double Difference = (AttackerTrophies - DefenderTrophies) < 0 ? +(DefenderTrophies - AttackerTrophies) : (AttackerTrophies - DefenderTrophies);
            if (Difference >= 13 && Difference <= 34)
            {
                return Math.Round(-0.0794 * (AttackerTrophies - DefenderTrophies) + 29.35838);
            }
            else
            {
                return 0;
            }
        }
        internal static void AddExperience(this Player User, int Value)
        {
            if (Value > 0)
            {
                User.Experience += Value;

                if (User.Level < 500)
                {
                    Experience_Levels _ExperienceLevels = CSV.Tables.Get(Gamefile.Experience_Levels)
                        .GetDataWithID(User.Level + 1) as Experience_Levels;

                    if (_ExperienceLevels.ExpPoints <= User.Experience)
                    {
                        User.Experience -= _ExperienceLevels.ExpPoints;
                        User.Level++;
                    }
                }
                else
                    User.Experience = 0;
            }
        }

        internal static double LoseTrophies(/*this Battle _Battle*/int AttackerTrophies, int DefenderTrophies)
        {
            if (AttackerTrophies >= 1000 && DefenderTrophies >= 1000)
            {
                return Math.Round(0.0531 * (AttackerTrophies - DefenderTrophies) + 19.60453);
            }
            else
            {
                return 0;
            }
        }
        internal static void Check_Missions(this Player Player)
        {
            if (Player != null)
            {
                foreach (Missions CSV_Missions in CSV.Tables.Get(Gamefile.Missions).Datas)
                {
                    if (Player.Tutorials.FindIndex(M => M == CSV_Missions.GetGlobalID()) < 0)
                    {
                        if (!string.IsNullOrEmpty(CSV_Missions.Dependencies))
                        {
                            Missions Dependencies_Missions = CSV.Tables.Get(Gamefile.Missions).GetData(CSV_Missions.Dependencies) as Missions;

                            if (Player.Tutorials.FindIndex(T => T == Dependencies_Missions.GetGlobalID()) < 0)
                            {
                                continue;
                            }
                        }
                        if (!string.IsNullOrEmpty(CSV_Missions.BuildBuilding))
                        {
                            Buildings CSV_Buildings = CSV.Tables.Get(Gamefile.Buildings).GetData(CSV_Missions.BuildBuilding) as Buildings;

                            //List<Building> Building = Player.Objects.Buildings.FindAll(B => B.Data == CSV_Buildings.GetGlobalID());

                            if (CSV_Missions.BuildBuildingCount == 1)
                            {
                            //    if (Building.Count > 0)
                                {
                              //      int Index = Building.FindIndex(B => B.Level + 1 >= CSV_Missions.BuildBuildingLevel);

                                //    if (Index > -1)
                                    {
                                  //      Player.Tutorials.Add(CSV_Missions.GetGlobalID());
                                    }
                                }
                            }
                            else
                            {
                                //int Count = Building.FindAll(B => B.Level + 1 >= CSV_Missions.BuildBuildingLevel).Count;

                                //if (Count >= CSV_Missions.BuildBuildingCount)
                                {
                                  //  Player.Tutorials.Add(CSV_Missions.GetGlobalID());
                                }
                            }
                        }
                        else if (CSV_Missions.OpenAchievements)
                        {
                            Player.Tutorials.Add(CSV_Missions.GetGlobalID());
                        }
                        else if (CSV_Missions.TrainTroops > 0)
                        {
                            int Troops_Count = 0;

                            //foreach (Slot Slot in Player.Units)
                            {
                              //  Troops_Count += Slot.Count;
                            }

                            if (Troops_Count >= CSV_Missions.TrainTroops)
                                Player.Tutorials.Add(CSV_Missions.GetGlobalID());
                        }
                        else if (!string.IsNullOrEmpty(CSV_Missions.AttackNPC))
                        {
                            //Npcs CSV_Npcs = CSV.Tables.Get(Gamefile.Npcs).GetData(CSV_Missions.AttackNPC) as Npcs;

                            //if (Player.Npcs.FindIndex(N => N.Data == CSV_Npcs.GetGlobalID()) > -1)
                            {
                                Player.Tutorials.Add(CSV_Missions.GetGlobalID());
                                Player.AddExperience(CSV_Missions.RewardXP);

                                //  Player.Resources.Plus(Logic.Enums.Resource.GOLD, CSV_Npcs.Gold);
                                //Player.Resources.Plus(Logic.Enums.Resource.ELIXIR, CSV_Npcs.Elixir);
                            }
                        }
                        else if (CSV_Missions.ChangeName)
                        {
                            //if (Player.NameSet > 0)
                                Player.Tutorials.Add(CSV_Missions.GetGlobalID());
                        }
                        if (!string.IsNullOrEmpty(CSV_Missions.RewardResource))
                        {
                            //Resources CSV_Resources = CSV.Tables.Get(Gamefile.Resources).GetData(CSV_Missions.RewardResource) as Resources;

                            //Player.Resources.Plus((Logic.Enums.Resource)CSV_Resources.GetGlobalID(), CSV_Missions.RewardResourceCount);
                            Player.AddExperience(CSV_Missions.RewardXP);
                        }
                        if (!string.IsNullOrEmpty(CSV_Missions.RewardTroop))
                        {
                            //Characters CSV_Characters = CSV.Tables.Get(Gamefile.Characters).GetData(CSV_Missions.RewardResource) as Characters;

                            //Player.Units.Add(new Slot(CSV_Characters.GetGlobalID(), CSV_Missions.RewardTroopCount));
                            Player.AddExperience(CSV_Missions.RewardXP);
                        }
                    }
                }
            }
        }

        public static bool Mission_Finish(this Player Player, int Global_ID)
        {
            if (Player.Tutorials.FindIndex(M => M == Global_ID) < 0)
            {
                Missions Mission = CSV.Tables.Get(Gamefile.Missions).GetDataWithID(Global_ID) as Missions;

#if DEBUG
                Console.WriteLine($"Mission received {Mission.Name} marked as finished");
#endif
                if (!string.IsNullOrEmpty(Mission.RewardResource))
                {
                    Files.CSV_Logic.Resource CSV_Resources =
                        CSV.Tables.Get(Gamefile.Resources).GetData(Mission.RewardResource) as Files.CSV_Logic.Resource;

                    Player.Resources.Plus(CSV_Resources.GetGlobalID(), Mission.RewardResourceCount);
                }
                if (!string.IsNullOrEmpty(Mission.RewardTroop))
                {
                    Characters CSV_Characters =
                        CSV.Tables.Get(Gamefile.Characters).GetData(Mission.RewardTroop) as Characters;

#if DEBUG
                    Console.WriteLine($"Player received {CSV_Characters.Name} as mission rewards");
#endif
                    Player.Add_Unit(CSV_Characters.GetGlobalID(), Mission.RewardTroopCount);
                }

                if (!string.IsNullOrEmpty(Mission.Dependencies))
                {
                    int DependenciesID = CSV.Tables.Get(Gamefile.Missions).GetData(Mission.Dependencies).GetGlobalID();
                    if (Player.Tutorials.FindIndex(M => M == DependenciesID) < 0)
                    {
#if DEBUG
                        Console.WriteLine($"Mission Dependencies {(CSV.Tables.Get(Gamefile.Missions).GetDataWithID(DependenciesID) as Missions).Name} marked as finished");
#endif
                        Mission_Finish(Player, DependenciesID);
                    }
                }
                if (!string.IsNullOrEmpty(Mission.AttackNPC))
                {
                    Npcs CSV_Npcs = CSV.Tables.Get(Gamefile.Npcs).GetData(Mission.AttackNPC) as Npcs;

                    if (Player.Npcs.FindIndex(N => N.NPC_Id == CSV_Npcs.GetGlobalID()) > -1)
                    {
                        Player.Resources.Plus(Logic.Enums.Resource.Gold, CSV_Npcs.Gold);
                        Player.Resources.Plus(Logic.Enums.Resource.Elixir, CSV_Npcs.Elixir);
                    }
                }
                if (Mission.ChangeName)
                {
                    Player.Resources.Plus(Logic.Enums.Resource.Gold, 900);
                    Player.Resources.Plus(Logic.Enums.Resource.Elixir, 400);
                }

                Player.AddExperience(Mission.RewardXP);
                Player.Tutorials.Add(Mission.GetGlobalID());
                return true;
            }
            return false;
        }

        internal static int GetDarkElixirDiamondCost(int Count)
        {
            int Total_Gems = 0;
            if (Count >= 1)
            {
                if (Count >= 10)
                {
                    if (Count >= 100)
                    {
                        if (Count >= 1000)
                        {
                            if (Count >= 10000)
                            {
                                int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_100000") as
                                    Globals).NumberValue;
                                int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_10000") as
                                    Globals).NumberValue;

                                Total_Gems =
                                    (int)
                                    Math.Round((SupCost - Inf_Cost) * (long)(Count - 10000) / (100000 - 10000 * 1.0)) +
                                    Inf_Cost;
                            }
                            else
                            {
                                int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_10000") as
                                    Globals).NumberValue;
                                int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_1000") as
                                    Globals).NumberValue;

                                Total_Gems =
                                    (int)
                                    Math.Round((SupCost - Inf_Cost) * (long)(Count - 1000) / (10000 - 1000 * 1.0)) +
                                    Inf_Cost;
                            }
                        }
                        else
                        {
                            int SupCost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_1000") as Globals)
                                .NumberValue;
                            int Inf_Cost =
                                (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_100") as Globals)
                                .NumberValue;

                            Total_Gems =
                                (int)Math.Round((SupCost - Inf_Cost) * (long)(Count - 100) / (1000 - 100 * 1.0)) +
                                Inf_Cost;
                        }
                    }
                    else
                    {
                        int SupCost =
                            (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_100") as Globals)
                            .NumberValue;
                        int Inf_Cost =
                            (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_10") as Globals)
                            .NumberValue;

                        Total_Gems = (int)Math.Round((SupCost - Inf_Cost) * (long)(Count - 10) / (100 - 10 * 1.0)) +
                                     Inf_Cost;
                    }
                }
                else
                {
                    int SupCost =
                        (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_10") as Globals).NumberValue;
                    int Inf_Cost =
                        (CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_1") as Globals).NumberValue;

                    Total_Gems = (int)Math.Round((SupCost - Inf_Cost) * (long)(Count - 1) / (10 - 1 * 1.0)) + Inf_Cost;
                }
            }
            return Total_Gems;
        }

        internal static int GetResourceDiamondCost(int Count, int _Index)
        {
            if (_Index == CSV.Tables.Get(Gamefile.Resources).GetData("DarkElixir").GetID())
            {
                int Total_Gems = GetDarkElixirDiamondCost(Count);
                return Total_Gems;
            }
            else
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
                                        Math.Round((SupCost - Inf_Cost) * (long)(Count - 1000000) /
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
                                        Math.Round((SupCost - Inf_Cost) * (long)(Count - 100000) /
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
                                    Math.Round((SupCost - Inf_Cost) * (long)(Count - 10000) / (100000 - 10000 * 1.0)) +
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
                                (int)Math.Round((SupCost - Inf_Cost) * (long)(Count - 1000) / (10000 - 1000 * 1.0)) +
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
                            (int)Math.Round((SupCost - Inf_Cost) * (long)(Count - 100) / (1000 - 100 * 1.0)) +
                            Inf_Cost;
                    }
                }
                else
                {
                    Total_Gems = 1;
                }
                return Total_Gems;
            }
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
