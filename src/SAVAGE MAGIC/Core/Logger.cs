using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Convert;

namespace Magic.ClashOfClans.Core
{
    internal static class Logger
    {
        static bool ValidLogLevel;
        static int logLevel = ToInt32(ConfigurationManager.AppSettings["log_level"]);
        static string timestamp = Convert.ToString(DateTime.Today).Remove(10).Replace(".", "-").Replace("/", "-");
        static string path = "Logs/log_" + timestamp + "_.txt";
        static SemaphoreSlim _fileLock = new SemaphoreSlim(1);

        private static readonly object s_lock = new object();
        private static readonly string s_errPath = "Logs/err_" + DateTime.Now.ToFileTime() + "_.log";
        private static readonly StreamWriter s_errWriter = new StreamWriter(s_errPath);

        public static void Initialize()
        {
            if (logLevel > 2)
            {
                ValidLogLevel = false;
                LogLevelError();
            }
            else
            {
                ValidLogLevel = true;
            }

            if (logLevel != 0 || ValidLogLevel == true)
            {
                if (!File.Exists("logs/log_" + timestamp + "_.txt"))
                {
                    using (var sw = new StreamWriter("logs/log_" + timestamp + "_.txt"))
                    {
                        sw.WriteLine("Log file created at " + DateTime.Now);
                        sw.WriteLine();
                    }
                }
            }
        }

        public static async void Write(string text)
        {
            if (logLevel != 0)
            {
                try
                {
                    await _fileLock.WaitAsync();
                    if (logLevel == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[LOG]    " + text);
                        Console.ResetColor();
                    }

                    using (StreamWriter sw = new StreamWriter(path, true))
                        await sw.WriteLineAsync("[LOG]    " + text + " at " + DateTime.UtcNow);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
        }

        public static void SayInfo(string message)
        {
            lock (s_lock)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                Console.WriteLine(message);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                Console.ResetColor();
            }
        }

        public static void SayAscii(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
            Console.WriteLine(message);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }

        public static void Say(string message)
        {
            lock (s_lock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
                Console.WriteLine(message);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                Console.ResetColor();
            }
        }

        public static void Say()
        {
            lock (s_lock)
            {
                Console.WriteLine();
            }
        }

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(string message)
        {
            lock (s_lock)
            {
                var text = "[ERROR]  " + message;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ResetColor();

                s_errWriter.WriteLine(text);
            }
        }

        private static void LogLevelError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Please choose a valid Log Level");
            Console.WriteLine("UCS Emulator is now closing...");
            Console.ResetColor();
            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}
