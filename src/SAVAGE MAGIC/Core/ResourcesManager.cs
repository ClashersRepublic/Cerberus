using Magic.ClashOfClans;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Magic.ClashOfClans.Core
{
    internal static class ResourcesManager
    {
        // Socket Handle -> Client instance.
        private static ConcurrentDictionary<long, Client> _clients;

        // User Id -> Level instance.
        private static ConcurrentDictionary<long, Level> _inMemoryLevels;
        // Alliance Id -> Alliance instance.
        private static ConcurrentDictionary<long, Alliance> _inMemoryAlliances;

        // Not sure why they are using this as well as InMemLevels.
        private static List<Level> _onlinePlayers;

        public static void Initialize()
        {
            _onlinePlayers = new List<Level>();
            _clients = new ConcurrentDictionary<long, Client>();

            _inMemoryLevels = new ConcurrentDictionary<long, Level>();
            _inMemoryAlliances = new ConcurrentDictionary<long, Alliance>();
        }

        public static void AddClient(Client client)
        {
            _clients.TryAdd(client.GetSocketHandle(), client);
            Program.TitleAd();
        }

        public static bool DropClient(long socketHandle)
        {
            var closedSocket = false;
            try
            {
                var client = default(Client);
                if (_clients.TryRemove(socketHandle, out client))
                {
                    Program.TitleDe();
                    
                    var socket = client.Socket;
                    try { socket.Shutdown(SocketShutdown.Both); }
                    catch { /* Swallow */ }
                    try { socket.Dispose(); }
                    catch { /* Swallow */ }

                    closedSocket = true;

                    // Clean level from memory if its Level has been loaded.
                    var level = client.Level;
                    if (level != null)
                        LogPlayerOut(level);

                    Logger.Write($"Client with socket handle {socketHandle} has been dropped.");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Exception while dropping client.");
            }
            return closedSocket;
        }

        public static List<Client> GetConnectedClients() => _clients.Values.ToList();

        public static List<Level> GetInMemoryLevels()
        {
            var levels = new List<Level>();

            lock (_inMemoryLevels) // ??
                levels.AddRange(_inMemoryLevels.Values);

            return levels;
        }

        public static List<Level> OnlinePlayers => _onlinePlayers;

        public static Level GetPlayer(long id, bool persistent = false)
        {
            // Try to get player from the memory, if not found
            // we look into the database.
            var result = GetInMemoryLevel(id);
            if (result == null)
            {
                result = DatabaseManager.Instance.GetLevel(id);
                if (result != null && persistent)
                    LoadLevel(result);
            }
            return result;
        }

        public static bool IsPlayerOnline(Level l) => _onlinePlayers.Contains(l);

        public static void LoadLevel(Level level)
        {
            _inMemoryLevels.TryAdd(level.Avatar.Id, level);
        }

        public static void LogPlayerIn(Level level, Client client)
        {
            // Set the back refs.
            level.Client = client;
            client.Level = level;

            lock (_onlinePlayers)
            {
                var index = _onlinePlayers.IndexOf(level);
                if (index == -1)
                {
                    _onlinePlayers.Add(level);

                    // Register level in dictionary.
                    LoadLevel(level);
                }
                else
                {
                    Logger.Error("A client who is already logged in is trying to log in.");

                    var oldLevel = _onlinePlayers[index];
                    DropClient(oldLevel.Client.GetSocketHandle());

                    _onlinePlayers.Add(level);
                }
            }
        }

        public static void LogPlayerOut(Level level)
        {
            // Make sure to tick before dropping client because
            // we're not morons right.
            level.Tick();

            try
            {
                DatabaseManager.Instance.Save(level);
            }
            catch
            {
                // No need logging since its already done in the Save method.
            }

            _onlinePlayers.Remove(level);
            _inMemoryLevels.TryRemove(level.Avatar.Id);
        }

        private static Level GetInMemoryLevel(long userId)
        {
            var level = default(Level);
            _inMemoryLevels.TryGetValue(userId, out level);
            return level;
        }

        public static List<Alliance> GetInMemoryAlliances() => _inMemoryAlliances.Values.ToList();

        public static void AddAllianceInMemory(Alliance alliance)
        {
            _inMemoryAlliances.TryAdd(alliance.AllianceId, alliance);
        }

        public static bool InMemoryAlliancesContain(long key) => _inMemoryAlliances.Keys.Contains(key);

        public static Alliance GetInMemoryAlliance(long key)
        {
            Alliance a;
            _inMemoryAlliances.TryGetValue(key, out a);
            return a;
        }

        public static void RemoveAllianceFromMemory(long key)
        {
            _inMemoryAlliances.TryRemove(key);
        }

        public static void DisconnectClient(Client c)
        {
            new OutOfSyncMessage(c).Send();
            DropClient(c.GetSocketHandle());
        }
    }
}
