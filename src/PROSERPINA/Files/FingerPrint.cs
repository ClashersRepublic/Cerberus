using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BL.Servers.CR.Files
{
    internal class Fingerprint
    {
        internal static string Json;

        internal static string Sha;

        internal static string[] Version;

        internal static bool Custom;

        internal Fingerprint()
        {
            try
            {
                if (!this.Patches())
                {
                    if (File.Exists(@"Gamefiles\fingerprint.json"))
                    {
                        Fingerprint.Json = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Gamefiles\fingerprint.json");
                        JObject _Json = JObject.Parse(Fingerprint.Json);
                        Fingerprint.Sha = _Json["sha"].ToObject<string>();
                        Fingerprint.Version = _Json["version"].ToObject<string>().Split('.');

                        Console.WriteLine("Fingerprint detected, with version " + string.Join(".", Fingerprint.Version) + "." + Environment.NewLine);
                    }
                    else
                    {
                        Console.WriteLine("The Fingerprint cannot be loaded, the file does not exist." + Environment.NewLine);
                    }
                }
                else
                {
                    Console.WriteLine("Custom patch detected, with version " + string.Join(".", Fingerprint.Version) + "." + Environment.NewLine);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An error occured while parsing the fingerprint." + Environment.NewLine);
            }
        }

        internal bool Patches()
        {
            bool _Result = false;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION"))
            {
                string[] _Lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION");

                if (!string.IsNullOrEmpty(_Lines[0]))
                {
                    Fingerprint.Version = _Lines[0].Split('.');

                    if (_Lines.Length > 1 && !string.IsNullOrEmpty(_Lines[1]))
                    {
                        Fingerprint.Sha = _Lines[1];

                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Patchs\\" + Fingerprint.Sha + "\\fingerprint.json"))
                        {
                            Fingerprint.Json = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Patchs\\" + Fingerprint.Sha + "\\fingerprint.json");
                            Fingerprint.Json.Trim('\n', '\r');

                            _Result = true;
                            Fingerprint.Custom = true;
                        }
                    }
                }
            }

            return _Result;
        }
    }
}