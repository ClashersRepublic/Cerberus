using System.IO;
using Magic.ClashOfClans.Core;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class BuyDecoCommand : Command
    {
        public int DecoId;
        public uint Unknown1;
        public int X;
        public int Y;

        public BuyDecoCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            DecoId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            Avatar avatar = level.Avatar;

            DecoData dataById = (DecoData) CsvManager.DataTables.GetDataById(DecoId);

            if (!avatar.HasEnoughResources(dataById.GetBuildResource(), dataById.GetBuildCost()))
              return;
            ResourceData buildResource = dataById.GetBuildResource();
            avatar.CommodityCountChangeHelper(0, (Data) buildResource, -dataById.GetBuildCost());

            Deco deco = new Deco((Data) dataById, level);
            deco.SetPosition(X, Y, level.Avatar.GetActiveLayout());
            level.GameObjectManager.AddGameObject((GameObject) deco);
        }
    }
}