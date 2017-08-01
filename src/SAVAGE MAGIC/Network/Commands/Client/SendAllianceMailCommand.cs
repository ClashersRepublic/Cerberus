using System;
using System.IO;
using Savage.Magic.Core;

using Savage.Magic;
using Savage.Magic.Logic;
using Savage.Magic.Logic.AvatarStreamEntries;
using Savage.Magic.Network.Messages.Server;

namespace Savage.Magic.Network.Commands.Client
{
    internal class SendAllianceMailCommand : Command
    {
        internal string m_vMailContent;

        public SendAllianceMailCommand(PacketReader br)
        {
            m_vMailContent = br.ReadString();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            SendAllianceMailCommand allianceMailCommand = this;
            try
            {
                Avatar avatar = level.Avatar;
                long allianceId = avatar.GetAllianceId();
                if (allianceId > 0L)
                {
                    Alliance alliance = ObjectManager.GetAlliance(allianceId);
                    if (alliance != null)
                    {
                        AllianceMailStreamEntry allianceMailStreamEntry1 = new AllianceMailStreamEntry();
                        allianceMailStreamEntry1.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        allianceMailStreamEntry1.SetAvatar(avatar);
                        allianceMailStreamEntry1.SetIsNew((byte)2);
                        allianceMailStreamEntry1.SetSenderId(avatar.Id);
                        allianceMailStreamEntry1.SetAllianceId(allianceId);
                        allianceMailStreamEntry1.SetAllianceBadgeData(alliance.AllianceBadgeData);
                        allianceMailStreamEntry1.SetAllianceName(alliance.AllianceName);
                        allianceMailStreamEntry1.SetMessage(m_vMailContent);
                        foreach (Level onlinePlayer in ResourcesManager.OnlinePlayers)
                        {
                            if (onlinePlayer.Avatar.GetAllianceId() == allianceId)
                            {
                                AvatarStreamEntryMessage Message = new AvatarStreamEntryMessage(onlinePlayer.Client);
                                AllianceMailStreamEntry allianceMailStreamEntry2 = allianceMailStreamEntry1;
                                Message.SetAvatarStreamEntry((Savage.Magic.Logic.AvatarStreamEntries.AvatarStreamEntry)allianceMailStreamEntry2);
                                Message.Send();
                            }
                        }
                    }
                }
                avatar = (Avatar) null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}