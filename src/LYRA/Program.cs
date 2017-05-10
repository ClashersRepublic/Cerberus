using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace BL.Proxy.Lyra
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            new ConsoleArgs(args).Parse();

            Console.Title = "▁ ▂ ▄ ▅ ▆ ▇ █     BL.Proxy.Lyra - " + Helper.AssemblyVersion + "     █ ▇ ▆ ▅ ▄ ▂ ▁";

            Proxy.Start();
            
            new ConsoleCmdListener();
        }

        internal static void Close()
        {
            Environment.Exit(0);
        }

        internal static void WaitAndClose(int ms = 1350)
        {
            Thread.Sleep(ms);
            Close();
        }

        internal static void Restart()
        {
            Process.Start(Assembly.GetExecutingAssembly().Location);
            Close();
        }
    }
}