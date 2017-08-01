using System;
using System.IO;
using System.Text;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Commands.Client;
using Magic.ClashOfClans.Network.Messages.Server;
using static Magic.ClashOfClans.Logic.Avatar;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class GoHomeMessage : Message
    {
        public int State;

        public GoHomeMessage(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            State = Reader.ReadInt32();
        }

        public override void Process(Level level)
        {
            if (level.Avatar.State == UserState.PVP)
            {
                var info = default(AttackInfo);
                if (!level.Avatar.AttackingInfo.TryGetValue(level.Avatar.Id, out info))
                {
                    Logger.Write("Unable to obtain attack info.");
                }
                else
                {
                    var defender = info.Defender;
                    var attacker = info.Attacker;

                    var lost = info.Lost;
                    var reward = info.Reward;

                    var usedtroop = info.UsedTroop;

                    int attackerscore = attacker.Avatar.GetScore();
                    int defenderscore = defender.Avatar.GetScore();

                    if (defender.Avatar.GetScore() > 0)
                        defender.Avatar.SetScore(defenderscore -= lost);

                    //Logger.Write("Used troop type: " + usedtroop.Count);
                    //foreach(var a in usedtroop)
                    //{
                    //    Logger.Write("Troop Name: " + a.Data.GetName());
                    //    Logger.Write("Troop Used Value: " + a.Value);
                    //}
                    attacker.Avatar.SetScore(attackerscore += reward);
                    attacker.Avatar.AttackingInfo.Clear(); //Since we use userid for now,We need to clear to prevent overlapping
                    Resources(attacker);

                    DatabaseManager.Instance.Save(attacker);
                    DatabaseManager.Instance.Save(defender);
                }
            }

            if (level.Avatar.State == UserState.CHA)
            {
                //Attack 
            }

            if (State == 1)
            {
                var player = level.Avatar;
                player.State = UserState.Editmode;
            }
            else
            {
                var player = level.Avatar;
                player.State = UserState.Home;
            }

            level.Tick();

            var alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            new OwnHomeDataMessage(Client, level).Send();
            if (alliance != null)
            {
                new AllianceStreamMessage(Client, alliance).Send();
            }
        }

        public void Resources(Level level)
        {
            var avatar = level.Avatar;
            var currentGold = avatar.GetResourceCount(CsvManager.DataTables.GetResourceByName("Gold"));
            var currentElixir = avatar.GetResourceCount(CsvManager.DataTables.GetResourceByName("Elixir"));
            var goldLocation = CsvManager.DataTables.GetResourceByName("Gold");
            var elixirLocation = CsvManager.DataTables.GetResourceByName("Elixir");

            if (currentGold >= 1000000000 | currentElixir >= 1000000000)
            {
                avatar.SetResourceCount(goldLocation, currentGold + 10);
                avatar.SetResourceCount(elixirLocation, currentElixir + 10);
            }
            else if (currentGold <= 999999999 || currentElixir <= 999999999)
            {
                avatar.SetResourceCount(goldLocation, currentGold + 1000);
                avatar.SetResourceCount(elixirLocation, currentElixir + 1000);
            }
        }
    }
}
