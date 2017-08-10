using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Files;
using CRepublic.Royale.Logic;

namespace CRepublic.Royale.Core.Resource
{
    internal static class Players
    {
        internal static ConcurrentDictionary<long, Level> Levels;
        internal static readonly object s_sync = new object();
        internal static long Seed;

        internal static void Initialize()
        {
            Levels = new ConcurrentDictionary<long, Level>();
        }

        internal static void Add(Level Player)
        {

            if (Levels.ContainsKey(Player.Avatar.UserId))
            {
                Levels[Player.Avatar.UserId] = Player;
            }
            else
            {
                Levels.TryAdd(Player.Avatar.UserId, Player);
            }
        }

        internal static Level Get(long UserId, bool Store = true)
        {
            if (!Levels.ContainsKey(UserId))
            {
                Level Player = Database_Manager.Instance.FetchLevel(UserId);

                if (Player != null && Store)
                    Add(Player);

                return Player;
            }
            return Levels[UserId];
        }

        internal static Level New(long UserId = 0, string token = "", bool Store = true)
        {
            lock (s_sync)
            {
                if (UserId == 0 || Seed == UserId)
                {
                    UserId = Seed++;
                }
                else
                {
                    if (UserId > Seed)
                        Seed = UserId + 1;
                }
            }

            var pass = Utils.RandomString(20);
            var Player = new Level(UserId)
            {
                Avatar =
                {
                    Token = string.IsNullOrEmpty(token) ? pass : token,
                    Password = pass.Reverse().Take(12).ToString()
                }         
            };

            if (Store)
            {
                Add(Player);
            }
            
            Database_Manager.Instance.AddLevel(Player);

            return Player;
        }
    }
}
