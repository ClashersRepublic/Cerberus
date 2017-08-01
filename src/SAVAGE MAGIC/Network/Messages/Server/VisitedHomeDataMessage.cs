using System;
using System.Collections.Generic;
using System.Linq;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class VisitedHomeDataMessage : Message
    {
        private readonly Level m_vOwnerLevel;
        private readonly Level m_vVisitorLevel;

        public VisitedHomeDataMessage(ClashOfClans.Client client, Level ownerLevel, Level visitorLevel) : base(client)
        {
            MessageType = 24113;
            m_vOwnerLevel = ownerLevel;
            m_vVisitorLevel = visitorLevel;
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var home = new Home(m_vOwnerLevel.Avatar.Id);
            home.SetShieldTime(m_vOwnerLevel.Avatar.RemainingShieldTime);
            home.SetHomeJson(m_vOwnerLevel.SaveToJson());

            data.AddInt32(-1);
            data.AddInt32((int)TimeSpan.FromSeconds(100).TotalSeconds);
            data.AddRange(home.Encode());
            data.AddRange(m_vOwnerLevel.Avatar.Encode());
            data.AddInt32(0);
            data.Add(1);
            data.AddRange(m_vVisitorLevel.Avatar.Encode());
            Encrypt(data.ToArray());
        }
    }
}
