using System;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Messages.Client
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
