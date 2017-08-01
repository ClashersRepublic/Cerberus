using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.Database;
using Magic.Files;
using Magic.Files.CSV;
using Magic.Files.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using static Magic.ClashOfClans.Core.Logger;
using Timer = System.Threading.Timer;

namespace Magic.ClashOfClans.Core

{
    internal static class ObjectManager
    {
        private static readonly object s_sync = new object();
        private static readonly Random s_rand = new Random();

        private static long m_vAllianceSeed;
        private static long m_vAvatarSeed;
        public static string m_vHomeDefault;
        private static Timer m_vSaveTimer;

        public static int DonationSeed;
        public static Dictionary<int, string> NpcLevels;
        public static FingerPrint FingerPrint;

        public static void Initialize()
        {
            NpcLevels = new Dictionary<int, string>();
            FingerPrint = new FingerPrint();

            m_vAvatarSeed = DatabaseManager.Instance.GetMaxPlayerId() + 1;
            m_vAllianceSeed = DatabaseManager.Instance.GetMaxAllianceId() + 1;

            // Shit went down, should probs shutdown.
            if (m_vAllianceSeed == 0 || m_vAvatarSeed == 0) { }

            m_vHomeDefault = File.ReadAllText(@"contents/starting_home.json");

            LoadNpcLevels();

            // Every 30 minutes.
            const int TIMER_PERIOD = 1000 * 60 * 30;
            m_vSaveTimer = new Timer(SaveCycle, null, 0, TIMER_PERIOD);
        }

        private static void SaveCycle(object state)
        {
            var level = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryLevels());
            var alliance = DatabaseManager.Instance.Save(ResourcesManager.GetInMemoryAlliances());

            level.Wait();
            alliance.Wait();
        }

        public static Alliance CreateAlliance()
        {
            Alliance alliance;

            var seed = m_vAllianceSeed;

            alliance = new Alliance(seed);
            m_vAllianceSeed++;

            DatabaseManager.Instance.CreateAlliance(alliance);

            ResourcesManager.AddAllianceInMemory(alliance);
            return alliance;
        }

        public static Level CreateLevel(long seed, string token)
        {
            // Increment & manage AvatarSeed thread safely.
            lock (s_sync)
            {
                if (seed == 0 || m_vAvatarSeed == seed)
                {
                    seed = m_vAvatarSeed++;
                }
                else
                {
                    if (seed > m_vAvatarSeed)
                        m_vAvatarSeed = seed + 1;
                }
            }

            var level = new Level(seed, token);
            level.LoadFromJson(m_vHomeDefault);

            //TODO: Handle errors when trying to create new accounts.
            DatabaseManager.Instance.CreateLevel(level);

            return level;
        }

        public static Alliance GetAlliance(long allianceId)
        {
            var alliance = default(Alliance);

            // Try to get alliance from memory first then db.
            // Could be improved.
            if (ResourcesManager.InMemoryAlliancesContain(allianceId))
                return ResourcesManager.GetInMemoryAlliance(allianceId);

            alliance = DatabaseManager.Instance.GetAlliance(allianceId);

            if (alliance != null)
                ResourcesManager.AddAllianceInMemory(alliance);

            return alliance;
        }

        public static List<Alliance> GetInMemoryAlliances()
        {
            return ResourcesManager.GetInMemoryAlliances();
        }

        public static Level GetRandomOnlinePlayer()
        {
            var levels = ResourcesManager.GetInMemoryLevels();
            int index = s_rand.Next(0, levels.Count);
            return levels[index];
        }

        public static void LoadNpcLevels()
        {
            int count = 0;
            NpcLevels.Add(17000000, new StreamReader(@"contents/level/NPC/tutorial_npc.json").ReadToEnd());
            NpcLevels.Add(17000001, new StreamReader(@"contents/level/NPC/tutorial_npc2.json").ReadToEnd());

            for (int i = 2; i < 50; i++)
            {
                var json = File.ReadAllText(@"contents/level/NPC/level" + (i + 1) + ".json");
                NpcLevels.Add(i + 17000000, json);
                ++count;
            }

            Say("Successfully loaded " + count + " NPC level(s).");
            Say();
        }

        public static void RemoveInMemoryAlliance(long id)
        {
            ResourcesManager.RemoveAllianceFromMemory(id);
        }
    }
}
