using BL.Servers.CR.Files.CSV_Helpers;
using BL.Servers.CR.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Files.CSV_Logic
{
    internal class Decos : Data
    {
        internal Decos(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the decos.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }

        /// <summary>
        /// Gets or sets Layer.
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        /// Gets or sets Lowend layer.
        /// </summary>
        public string LowendLayer { get; set; }

        /// <summary>
        /// Gets or sets Shadow scale.
        /// </summary>
        public int ShadowScale { get; set; }

        /// <summary>
        /// Gets or sets Shadow x.
        /// </summary>
        public int ShadowX { get; set; }

        /// <summary>
        /// Gets or sets Shadow y.
        /// </summary>
        public int ShadowY { get; set; }

        /// <summary>
        /// Gets or sets Shadow skew.
        /// </summary>
        public int ShadowSkew { get; set; }

        /// <summary>
        /// Gets or sets Collision radius.
        /// </summary>
        public int CollisionRadius { get; set; }

        /// <summary>
        /// Gets or sets Effect.
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Gets or sets Asset min trophy.
        /// </summary>
        public string AssetMinTrophy { get; set; }

        /// <summary>
        /// Gets or sets Asset min trophy score.
        /// </summary>
        public int AssetMinTrophyScore { get; set; }

        /// <summary>
        /// Gets or sets Asset min trophy file name.
        /// </summary>
        public string AssetMinTrophyFileName { get; set; }
    }
}