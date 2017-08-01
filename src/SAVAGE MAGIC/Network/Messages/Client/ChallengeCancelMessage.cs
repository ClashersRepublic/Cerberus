using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Logic.StreamEntries;
using Savage.Magic.Network;
using Savage.Magic.Network.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeCancelMessage : Message
    {
        public ChallengeCancelMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public override void Decode()
        {
            // Space
        }

        public override void Process(Level level)
        {
            var a = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            var s = a.ChatMessages.Find(c => c.GetSenderId() == level.Avatar.Id&& c.GetStreamEntryType() == 12);

            if (s != null)
            {
                a.ChatMessages.RemoveAll(t => t == s);
                foreach (AllianceMemberEntry op in a.AllianceMembers)
                {
                    Level player = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (player.Client!= null)
                    {
                        new AllianceStreamEntryRemovedMessage(Client, s.GetId()).Send();
                    }                                                   
                }
            }
        }
    }
}
