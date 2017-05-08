using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BL.Assets.Editor.Helpers;

namespace BL.Assets.Editor
{
    internal static class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();

        internal static MainForm Interface;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            #region Debug
            #if DEBUG

            AllocConsole();
            Console.SetOut(new Prefixed());


            Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine(@" ___________                   _________                      _________                      ");
            Console.WriteLine(@" \__    ___/___ _____    _____ \_   ___ \____________  ___.__.\_   ___ \____________  ___.__.");
            Console.WriteLine(@"   |    |_/ __ \\__  \  /     \/    \  \/\_  __ \__  \<   |  |/    \  \/\_  __ \__  \<   |  |");
            Console.WriteLine(@"   |    |\  ___/ / __ \|  Y Y  \     \____|  | \// __ \\___  |\     \____|  | \// __ \\___  |");
            Console.WriteLine(@"   |____| \___  >____  /__|_|  /\______  /|__|  (____  / ____| \______  /|__|  (____  / ____|");
            Console.WriteLine(@"              \/     \/      \/        \/            \/\/             \/            \/\/     ");
            Console.WriteLine(@"                                                                        V1.1.B2  ");
            Console.WriteLine(@"+-------------------------------------------------------------+");
            Console.WriteLine(@"|             This program is development version             |");
            Console.WriteLine(@"|                     Of TCC Magic Editor                     |");
            Console.WriteLine(@"+-------------------------------------------------------------+");
            #endif
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Program.Interface = new MainForm());

            #region Debug
#if DEBUG
            Console.WriteLine("Debugging done");
            Console.ReadLine();
            #endif
            #endregion
        }
    }
}