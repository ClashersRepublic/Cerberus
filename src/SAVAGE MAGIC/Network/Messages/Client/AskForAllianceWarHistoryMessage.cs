using System;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
{
    internal class AskForAllianceWarHistoryMessage : Message
    {
        private long AllianceID { get; set; }

        private long WarID { get; set; }

        public AskForAllianceWarHistoryMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                AllianceID = br.ReadInt64();
                WarID = br.ReadInt64();
            }
        }

        public override void Process(Level level)
        {
            AskForAllianceWarHistoryMessage warHistoryMessage = this;
            try
            {
                Alliance alliance = ObjectManager.GetAlliance(level.Avatar.GetAllianceId());
                new AllianceWarHistoryMessage(Client, alliance).Send();
            }
            catch (Exception ex)
            {
            }
        }
    }
}