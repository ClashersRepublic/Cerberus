using Magic.ClashOfClans;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Network;
using System;
using System.IO;
using System.Threading;

namespace Magic.ClashOfClans
{
    internal class Program
    {
        public static int OP = 0;

        public static void Main(string[] args)
        {
            // Print welcome message.
            MagicControl.WelcomeMessage();

            // Check directories and files.
            CheckDirectories();
            CheckFiles();

            // Initialize our stuff.
            CsvManager.Initialize();
            ResourcesManager.Initialize();
            ObjectManager.Initialize();

            Logger.Initialize();
            ExceptionLogger.Initialize();

            WebApi.Initialize();
            Gateway.Initialize();

            // Start listening since we're done initializing.
            Gateway.Listen();

            while (true)
            {
                const int SLEEP_TIME = 5000;

                var numDisc = 0;
                var clients = ResourcesManager.GetConnectedClients();
                for (int i = 0; i < clients.Count; i++)
                {
                    var client = clients[i];
                    if (DateTime.Now > client.NextKeepAlive)
                    {
                        ResourcesManager.DropClient(client.GetSocketHandle());
                        numDisc++;
                    }
                }

                if (numDisc > 0)
                    Logger.Say($"Dropped {numDisc} clients due to keep alive timeouts.");

                Logger.SayInfo("-- Pools --");
                Logger.SayInfo($"SocketAsyncEventArgs: created -> {Gateway.NumberOfArgsCreated} in-use -> {Gateway.NumberOfArgsInUse} available -> {Gateway.NumberOfArgs}");
                Logger.SayInfo($"Buffers: created -> {Gateway.NumberOfBuffersCreated} in-use -> {Gateway.NumberOfBuffersInUse} available -> {Gateway.NumberOfBuffers}");

                Thread.Sleep(SLEEP_TIME);
            }
        }

        public static void TitleAd()
        {
            Console.Title = Constants.DefaultTitle + Interlocked.Increment(ref OP);
        }

        public static void TitleDe()
        {
            Console.Title = Constants.DefaultTitle + Interlocked.Decrement(ref OP);
        }

        public static void CheckDirectories()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("Logs");

            if (!Directory.Exists("patch"))
                Directory.CreateDirectory("patch");

            if (!Directory.Exists("tools"))
                Directory.CreateDirectory("tools");

            if (!Directory.Exists("libs"))
                Directory.CreateDirectory("libs");

            if (!Directory.Exists("contents"))
                Directory.CreateDirectory("contents");
        }

        public static void CheckFiles()
        {
            if (!File.Exists("filter.ucs"))
                File.WriteAllText("filter.ucs", "./savegame");
        }
    }
}
