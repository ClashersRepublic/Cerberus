using System;
using System.Threading;

namespace BL.Proxy.Lyra.Consoles
{
    class ConsoleCmdListener
    {
        private Thread ListenerThread;
        public static string Command;

        /// <summary>
        /// Listener for console commands
        /// </summary>
        public ConsoleCmdListener()
        {
            ListenerThread = new Thread(() =>
            {
                Console.CursorSize = 35;
                while ((Command = Console.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(Command) && !string.IsNullOrWhiteSpace(Command) )
                    {
                        switch (Command.ToLower())
                        {
                            case "/help":                               
                                Logger.Log("/jsonreload -> Reloads the JSON definitions");
                                Logger.Log("/clear -> Clears the entire console window");
                                Logger.Log("/restart -> Restarts the proxy");
                                Logger.Log("/shutdown -> Safely shuts the proxy down");
                                break;
                            case "/jsonreload":
                                JSONPacketManager.LoadDefinitions();
                                break;
                            case "/clear":
                                Console.Clear();
                                break;
                            case "/restart":
                                Program.Restart();
                                break;
                            case "/shutdown":
                                Logger.Log("Shutting down..");
                                Proxy.Stop();
                                Program.WaitAndClose(350);
                                break;
                            default:
                                Logger.Log("Unknown command! Type \"/help\" to see all available console commands.", LogType.WARNING);
                                break;
                        }
                    }
                    else
                        Console.CursorTop -= 1;
                }
            });
            ListenerThread.Start();
        }
    }
}
