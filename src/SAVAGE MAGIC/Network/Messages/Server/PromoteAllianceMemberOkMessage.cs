using System.Collections.Generic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Client;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class PromoteAllianceMemberOkMessage : Message
    {
        internal long Id;
        internal int Role;

        public PromoteAllianceMemberOkMessage(ClashOfClans.Client client)
            : base(client)
        {
            MessageType = 24306;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt64(Id);
            pack.AddInt32(Role);
            Encrypt(pack.ToArray());
        }
        public void SetID(long id)
        {
            Id = id;
        }

        public void SetRole(int role)
        {
            Role = role;
        }
    }
}