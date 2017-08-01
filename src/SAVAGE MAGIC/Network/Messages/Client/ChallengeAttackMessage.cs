using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Network.Messages.Server;
using Magic.Packets.Messages.Server;

namespace Magic.Packets.Messages.Client
{
    internal class ChallengeAttackMessage : Message
    {
        public ChallengeAttackMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
            // Space
        }

        public long ID;

        public override void Decode()
        {
            ID = Reader.ReadInt64WithEndian();
        }

        public override void Process(Level level)
        {
            var a = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            var defender = ResourcesManager.GetPlayer(a.ChatMessages.Find(c => c.GetId() == ID).GetSenderId());
            if (defender != null)
            {
                defender.Tick();
                new ChallangeAttackDataMessage(Client, defender).Send();
            }
            else
            {
                new OwnHomeDataMessage(Client, level);
            }

            var alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            var s = alliance.ChatMessages.Find(c => c.GetStreamEntryType() == 12);
            if (s != null)
            {
                alliance.ChatMessages.RemoveAll(t => t == s);

                foreach (AllianceMemberEntry op in alliance.AllianceMembers)
                {
                    Level playera = ResourcesManager.GetPlayer(op.GetAvatarId());
                    if (playera.Client!= null)
                    {
                        var p = new AllianceStreamEntryMessage(playera.Client);
                        p.SetStreamEntry(s);
                        p.Send();
                    }
                }
            }
        }
    }
}
