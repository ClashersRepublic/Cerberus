using System;
using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class RequestAvatarNameChange : Message
    {
        public string PlayerName;
        public byte Unknown1;

        public RequestAvatarNameChange(ClashOfClans.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            PlayerName = Reader.ReadString();
        }

        public override void Process(Level level)
        {
            Level player = ResourcesManager.GetPlayer(level.Avatar.Id, false);
            if (player == null)
                return;

            if (PlayerName.Length > 15)
            {
                ResourcesManager.DisconnectClient(Client);
            }
            else
            {
                player.Avatar.SetName(PlayerName);

                new AvatarNameChangeOkMessage(Client)
                {
                    AvatarName = PlayerName
                }.Send();
            }
        }
    }
}
