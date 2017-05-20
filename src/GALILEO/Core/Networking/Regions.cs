using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;

namespace BL.Servers.CoC.Core.Networking
{
    internal class Region
    {
        internal DatabaseReader Reader;
        internal string DbPath = "Gamefiles/database/Database.mmdb";

        public Region()
        {
            if (!Directory.Exists("Gamefiles/database/"))
                throw new DirectoryNotFoundException("Directory Gamefiles/database does not exist!");

            if (!File.Exists(DbPath))
                throw new FileNotFoundException($"{DbPath} does not exist in current directory!");

            Reader = new DatabaseReader(DbPath, FileAccessMode.Memory);
            //Reader = new DatabaseReader(DbPath, FileAccessMode.MemoryMapped); //Lower ram usage on start but idk the speed
            Loggers.Log("Region database loaded into memory.", true);
        }

        internal string GetIpCountryIso(string ipAddress)
        {
            if (ipAddress == null || Reader == null)
                return "BarbarianLand";

            try
            {
                return Reader.City(ipAddress).Country.IsoCode;
            }
            catch (AddressNotFoundException)
            {
                return "BarbarianLand";
            }
        }

        internal string GetIpCountry(string ipAddress)
        {
            if (ipAddress == null || Reader == null)
                return "BarbarianLand";

            try
            {
                return Reader.City(ipAddress).Country.Name;
            }
            catch (AddressNotFoundException)
            {
                return "BarbarianLand";
            }
        }


        internal string GetFullIpData(string ipAddress)
        {
            if (ipAddress == null || Reader == null)
                return "BarbarianLand";

            try
            {
                var city = Reader.City(ipAddress);

                var sb = new StringBuilder();

                sb.AppendLine("IP Country ISO: " + city.Country.IsoCode);
                sb.AppendLine("IP Country Name: " + city.Country.Name);
                sb.AppendLine();
                sb.AppendLine("IP Specific Subdivision ISO: " + city.MostSpecificSubdivision.IsoCode);
                sb.AppendLine("IP Specific Subdivision Name: " + city.MostSpecificSubdivision.Name);
                sb.AppendLine();
                sb.AppendLine("IP City:" + city.City.Name);
                sb.AppendLine("IP Postal:" + city.Postal.Code);
                sb.AppendLine();
                sb.AppendLine("IP Latitude:" + city.Location.Latitude);
                sb.AppendLine("IP Longitude:" + city.Location.Longitude);

                return sb.ToString();
            }
            catch (AddressNotFoundException)
            {
                return "BarbarianLand";
            }
        }

        internal void Bench(string name)
        {
            Console.Write($"\n{name}: ");
            var rand = new Random(1);
            var s = Stopwatch.StartNew();
            int count = 9000;
            for (var i = 0; i < count; i++)
            {
                var ip = new IPAddress(rand.Next(int.MaxValue));
                try
                {
                    this.Reader.City(ip);
                }
                catch (AddressNotFoundException) { }
            }
            s.Stop();
            Console.WriteLine("Total second {0:N0}", s.Elapsed.TotalSeconds);
            Console.WriteLine("{0:N0} queries per second", count / s.Elapsed.TotalSeconds);
        }
    }
}