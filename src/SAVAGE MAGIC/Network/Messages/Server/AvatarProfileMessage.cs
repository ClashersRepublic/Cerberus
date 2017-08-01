using System;
using System.Collections.Generic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class AvatarProfileMessage : Message
    {
        public Level Level;

        public AvatarProfileMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 24334;
        }

        public override void Encode()
        {
            var data = new List<byte>();

            data.AddRange(Level.Avatar.Encode());
            data.AddCompressedString(Level.SaveToJson());

            data.AddInt32(0); //Donated
            data.AddInt32(0); //Received
            data.AddInt32(0); //War Cooldown

            data.AddInt32(0); //Unknown
            data.Add(0); //Unknown

            Encrypt(data.ToArray());
        }
    }
}
