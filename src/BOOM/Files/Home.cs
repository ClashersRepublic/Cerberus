using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CRepublic.Boom.Files
{
    internal class Home
    {
        internal static string Starting_Home = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        internal Home()
        {
            if (Directory.Exists("Gamefiles/"))
            {
                if (File.Exists("Gamefiles/level/starting_home.json"))
                {
                    Home.Starting_Home = Regex.Replace(File.ReadAllText("Gamefiles/level/starting_home.json", Encoding.UTF8), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                }
            }
        }
    }
}