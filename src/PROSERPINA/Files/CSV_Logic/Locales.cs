using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Locales : Data
    {
        internal Locales(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the locales.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Sort order.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets Has even space characters.
        /// </summary>
        public bool HasEvenSpaceCharacters { get; set; }

        /// <summary>
        /// Gets or sets Used system font.
        /// </summary>
        public string UsedSystemFont { get; set; }

        /// <summary>
        /// Gets or sets Helpshift s d k language.
        /// </summary>
        public string HelpshiftSDKLanguage { get; set; }

        /// <summary>
        /// Gets or sets Helpshift s d k language android.
        /// </summary>
        public string HelpshiftSDKLanguageAndroid { get; set; }

        /// <summary>
        /// Gets or sets Helpshift language tag.
        /// </summary>
        public string HelpshiftLanguageTag { get; set; }

        /// <summary>
        /// Gets or sets Terms and service url.
        /// </summary>
        public string TermsAndServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets Parents guide url.
        /// </summary>
        public string ParentsGuideUrl { get; set; }

        /// <summary>
        /// Gets or sets Privacy policy url.
        /// </summary>
        public string PrivacyPolicyUrl { get; set; }

        /// <summary>
        /// Gets or sets Test language.
        /// </summary>
        public bool TestLanguage { get; set; }

        /// <summary>
        /// Gets or sets Test excludes.
        /// </summary>
        public string TestExcludes { get; set; }

        /// <summary>
        /// Gets or sets Region list file.
        /// </summary>
        public string RegionListFile { get; set; }

        /// <summary>
        /// Gets or sets Maintenance royal box.
        /// </summary>
        public bool MaintenanceRoyalBox { get; set; }

        /// <summary>
        /// Gets or sets Royal box u r l.
        /// </summary>
        public string RoyalBoxURL { get; set; }

        /// <summary>
        /// Gets or sets Royal box stage u r l.
        /// </summary>
        public string RoyalBoxStageURL { get; set; }

        /// <summary>
        /// Gets or sets Royal box dev u r l.
        /// </summary>
        public string RoyalBoxDevURL { get; set; }

        /// <summary>
        /// Gets or sets Boom box u r l.
        /// </summary>
        public string BoomBoxURL { get; set; }
    }
}