using System;
using System.IO;
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
            Loggers.Log("Region database loaded into memory.", true);
        }

        internal string GetIpCountryIso(string ipAddress)
        {
            if (ipAddress == null || Reader == null)
                return "UCS Land";

            try
            {
                return Reader.City(ipAddress).Country.IsoCode;
            }
            catch (AddressNotFoundException)
            {
                return "UCS Land";
            }
        }

        internal string GetIpCountry(string ipAddress)
        {
            if (ipAddress == null || Reader == null)
                return "UCS Land";

            try
            {
                return Reader.City(ipAddress).Country.Name;
            }
            catch (AddressNotFoundException)
            {
                return "UCS Land";
            }
        }


        internal string GetFullIpData(string ipAddress)
        {
            if (ipAddress == null || Reader == null)
                return "UCS Land";

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
                return "UCS Land";
            }
        }
    }
}