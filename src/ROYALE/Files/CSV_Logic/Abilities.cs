using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Abilities : Data
    {
        internal Abilities(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Icon file.
        /// </summary>
        public string IconFile { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Area effect object.
        /// </summary>
        public string AreaEffectObject { get; set; }

        /// <summary>
        /// Gets or sets Buff.
        /// </summary>
        public string Buff { get; set; }

        /// <summary>
        /// Gets or sets Buff time.
        /// </summary>
        public int BuffTime { get; set; }

        /// <summary>
        /// Gets or sets Effect.
        /// </summary>
        public string Effect { get; set; }
    }
}