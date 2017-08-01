using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    // Packet 14310
    internal class DonateAllianceUnitMessage : Message
    {
        public int Unknown1;
        public CombatItemData Troop;
        public int Unknown3;
        public int MessageID;

        public DonateAllianceUnitMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }
        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(Data)))
            {
                Unknown1 = br.ReadInt32WithEndian();
                Troop = (CombatItemData)br.ReadDataReference();
                Unknown3 = br.ReadInt32WithEndian();
                MessageID = br.ReadInt32WithEndian();
            }
        }
        public override void Process(Level level)
        {
            Alliance a = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
            var stream = a.ChatMessages.Find(c => c.GetId() == MessageID);

            var sender = ResourcesManager.GetPlayer(stream.GetSenderId());
            int upcomingspace = stream.m_vDonatedTroop + Troop.GetHousingSpace();
            if (upcomingspace <= stream.m_vMaxTroop)
            {
                //System.Console.WriteLine("Troop Donated :" + Troop.GetName());

            }
        }

    }
}
