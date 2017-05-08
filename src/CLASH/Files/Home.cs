using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BL.Servers.CoC.Files
{
    internal class Home
    {
        internal static string Starting_Home = string.Empty;
        internal string JsonPath = "Gamefiles/level/starting_home.json";

        internal Home()
        {
            if (!Directory.Exists("Gamefiles/level/"))
                throw new DirectoryNotFoundException("Directory Gamefiles/level does not exist!");

            if (!File.Exists(JsonPath))
                throw new Exception($"{JsonPath} does not exist in current directory!");

            Home.Starting_Home = Regex.Replace(File.ReadAllText(JsonPath, Encoding.UTF8), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

        }
    }
}