using System.IO;
using Magic.ClashOfClans.Core;

using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Network.Messages.Server;

namespace Magic.ClashOfClans.Network.Messages.Client
{
    internal class ChangeAvatarNameMessage : Message
    {
        private string PlayerName { get; set; }

        public ChangeAvatarNameMessage(ClashOfClans.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (var br = new PacketReader(new MemoryStream(Data)))
            {
                PlayerName = br.ReadString();
            }
        }

        public override void Process(Level level)
        {
            if (string.IsNullOrEmpty(PlayerName) || PlayerName.Length > 15)
            {
                ResourcesManager.DisconnectClient(Client);
            }
            else
            {
                level.Avatar.SetName(PlayerName);
                new AvatarNameChangeOkMessage(Client)
                {
                    AvatarName = level.Avatar.GetAvatarName()
            }.Send();
            }
        }
    }
}