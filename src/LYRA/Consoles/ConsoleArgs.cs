using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Proxy.Lyra.Consoles
{
    internal class ConsoleArgs
    {
        private readonly List<string> PassedArgs;

        public static bool Verbose = false;

        public ConsoleArgs(IEnumerable<string> args)
        {
            PassedArgs = new List<string>(args);
        }

        private static string RemovePrefix(string Command) => (Command.ElementAt(0) == '-') ? Command.Replace("-", "") : Command;

        public void Parse()
        {
            try
            {
                if (PassedArgs.Count == 0) return;

                foreach (string Arg in PassedArgs)
                {
                    switch (RemovePrefix(Arg))
                    {
                        case "help":
                            Logger.CenterString("=> Argument usage <=");
                            Console.Write(Environment.NewLine);
                            Logger.CenterString("-help -> Displays this.");
                            Logger.CenterString("-ver  -> Shows detailed version info");
                            break;
                        case "ver":
                            Logger.CenterString("=> Version <=");
                            Console.Write(Environment.NewLine);
                            Logger.CenterString("BL.Proxy.Lyra Public Version " + Helper.AssemblyVersion);
                            Logger.CenterString("Copyright © 2016, expl0itr");
                            Logger.CenterString("https://opensource.org/licenses/MIT/");
                            break;
                        // more args...
                        default:
                            Logger.CenterString("!!! Unknown arg: " + Arg + " !!!");
                            break;
                    }
                    Program.Close();
                }
            }

            catch (Exception ex)
            {
                Logger.Log("Failed to parse console arguments (" + ex.GetType() + ")!", LogType.EXCEPTION);
                Logger.Log("Please avoid invalid UTF-8 characters, this may be the cause of this exception.",
                    LogType.EXCEPTION);
                Program.WaitAndClose();
            }
        }
    }
}