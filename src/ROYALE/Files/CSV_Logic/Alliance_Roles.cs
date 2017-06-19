using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Alliance_Roles : Data
    {
        internal Alliance_Roles(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets T i d.
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// Gets or sets Can invite.
        /// </summary>
        public bool CanInvite { get; set; }

        /// <summary>
        /// Gets or sets Can send mail.
        /// </summary>
        public bool CanSendMail { get; set; }

        /// <summary>
        /// Gets or sets Can change alliance settings.
        /// </summary>
        public bool CanChangeAllianceSettings { get; set; }

        /// <summary>
        /// Gets or sets Can accept join request.
        /// </summary>
        public bool CanAcceptJoinRequest { get; set; }

        /// <summary>
        /// Gets or sets Can kick.
        /// </summary>
        public bool CanKick { get; set; }

        /// <summary>
        /// Gets or sets Can be promoted to leader.
        /// </summary>
        public bool CanBePromotedToLeader { get; set; }

        /// <summary>
        /// Gets or sets Can promote to own level.
        /// </summary>
        public bool CanPromoteToOwnLevel { get; set; }
    }
}