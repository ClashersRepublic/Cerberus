using System;
using System.IO;
using System.Text;
using System.Linq;

namespace BL.Proxy.Lyra
{ 
    class Logger
    {
        /// <summary>
        /// Centers a string 
        /// </summary>
        public static void CenterString(string str)
        {
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, Console.CursorTop);
            Console.WriteLine(str);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }
            
        /// <summary>
        /// Logs passed text
        /// </summary>
        public static void Log(string text, LogType logtype = LogType.INFO, ConsoleColor color = 0)
        {
            // Reset actual line
            Console.Write("\r");

            // Determine logtype and set appropriate color
            switch (logtype)
            {
                case LogType.INFO:
                    { color = ConsoleColor.Green; break; }      
                case LogType.WARNING:
                    { color = ConsoleColor.DarkYellow; break; }
                case LogType.EXCEPTION:
                    { color = ConsoleColor.Red; break; }
                case LogType.PACKET:
                    { color = ConsoleColor.Magenta; break; }
                case LogType.CONFIG:
                    { color = ConsoleColor.DarkCyan; break; }
                case LogType.JSON:
                    { color = ConsoleColor.Cyan; break; }
            }
            Console.ForegroundColor = color;
   
            // Colored Prefix - Text
            Console.Write(logtype);
            Console.ResetColor();
            Console.WriteLine(" - " + text);

            // Save text to file
            SaveToFile(text, logtype);
        }

        /// <summary>
        /// Saves a string including the logtype to a file
        /// </summary>
        private static void SaveToFile(string text, LogType logtype)
        {
            var path = "Logs\\" + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy") + ".log";
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("[" + DateTime.UtcNow.ToLocalTime().ToString("hh-mm-ss") + "-" + logtype + "]" + text);
                    sw.Close();
                }
            }
        }
    }
}
