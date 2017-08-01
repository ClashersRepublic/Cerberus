using System.Collections.Generic;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Messages.Server
{
    internal class SetDeviceTokenMessage : Message
    {
        readonly Level level;

        public SetDeviceTokenMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20113;
            level = client.Level;
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddString(level.Avatar.Token);
            Encrypt(pack.ToArray());
        }
    }
}