using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BL.Assets.LZMA
{
    internal class Prefixed : TextWriter
    {
        internal readonly TextWriter Original;

        /// <summary>
        /// Initializes a new instance of the <see cref="Prefixed"/> class.
        /// </summary>
        internal Prefixed()
        {
            this.Original = Console.Out;
        }

        public override Encoding Encoding => new UTF8Encoding();

        public override void Write(string Message)
        {
            this.Original.Write("[Earth]    {0}", Message);
        }

        public override void WriteLine(string Message)
        {
            try
            {
                if (Message.Length <= Console.WindowWidth)
                {
                    Console.SetCursorPosition((Console.WindowWidth - Message.Length) / 2, Console.CursorTop);
                }
            }
            catch
            {
                //   
            }

            this.Original.WriteLine("{0}", Message);
        }

        public override void WriteLine()
        {
            this.Original.WriteLine();
        }
    }
}