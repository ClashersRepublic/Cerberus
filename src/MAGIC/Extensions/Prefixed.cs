using System.Drawing;
using System;
using System.IO;
using System.Text;

namespace CRepublic.Magic.Extensions
{
    internal class Prefixed : TextWriter
    {
        private static readonly object s_lock = new object();
        internal readonly TextWriter Original;

        internal Prefixed()
        {
            this.Original = Console.Out;
        }

        public override Encoding Encoding => new ASCIIEncoding();

        public override void Write(string Message)
        {
            lock (s_lock)
            {
                this.Original.Write(Message);
            }
        }

        public override void WriteLine(string Message)
        {
            lock (s_lock)
            {
                if (Message.Length <= Console.WindowWidth)
                {
                    Console.SetCursorPosition((Console.WindowWidth - Message.Length) / 2, Console.CursorTop);
                }

                this.Original.WriteLine("{0}", Message);

                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            }
        }

        public override void WriteLine()
        {
            lock (s_lock)
            {
                this.Original.WriteLine();
            }
        }

    }
}