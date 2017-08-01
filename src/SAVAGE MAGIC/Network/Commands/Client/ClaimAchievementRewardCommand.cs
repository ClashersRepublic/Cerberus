using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class ClaimAchievementRewardCommand : Command
    {
        public int AchievementId;
        public uint Unknown1;

        public ClaimAchievementRewardCommand(PacketReader br)
        {
            AchievementId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            Avatar avatar = level.Avatar;
            AchievementData dataById = (AchievementData) CsvManager.DataTables.GetDataById(AchievementId);
            int diamondReward = (dataById.DiamondReward);
            avatar.AddDiamonds(diamondReward);
            int expReward = dataById.ExpReward;
            avatar.AddExperience(dataById.ExpReward);
            AchievementData ad = dataById;
            int num = 1;
            avatar.SetAchievment(ad, num != 0);
        }
    }
}