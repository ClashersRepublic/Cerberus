using System;
using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans.Core;

using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class SearchOpponentCommand : Command
    {
        public SearchOpponentCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            try
            {
                while (true)
                {
                    var defender = ObjectManager.GetRandomOnlinePlayer();
                    if (defender != null)
                    {
                        var allianceId = defender.Avatar.GetAllianceId();
                        if (allianceId > 0)
                        {
                            var defenderAlliance = ObjectManager.GetAlliance(allianceId);
                            if (defenderAlliance == null)
                                continue;
                        }

                        level.Avatar.State = Avatar.UserState.Searching;

                        var trophyDiff = Math.Abs(level.Avatar.GetScore() - defender.Avatar.GetScore());
                        var reward = (int)Math.Round(Math.Pow((5 * trophyDiff), 0.25) + 5d);
                        var lost = (int)Math.Round(Math.Pow((2 * trophyDiff), 0.35) + 5d);

                        var info = new Avatar.AttackInfo
                        {
                            Attacker = level,
                            Defender = defender,

                            Lost = lost,
                            Reward = reward,
                            UsedTroop = new List<DataSlot>()

                        };

                        // Just fucking clear it since its per user and a user can attack only once at a time.
                        level.Avatar.AttackingInfo.Clear();
                        level.Avatar.AttackingInfo.Add(level.Avatar.Id, info); //Use UserID For a while..Working on AttackID soon

                        defender.Tick();
                        new EnemyHomeDataMessage(level.Client, defender, level).Send();
                    }
                    else
                    {
                        Logger.Error("Could not find opponent in memory, returning home.");
                        new OwnHomeDataMessage(level.Client, level).Send();
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
                // Ultimate fail safe in case unexpected shit happens.

                ExceptionLogger.Log(ex, "Could not find opponent in memory, returning home.");
                new OwnHomeDataMessage(level.Client, level).Send();
            }
        }
    }
}
