using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;
using Magic.Packets.Commands.Client;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class MoveMultipleBuildingsCommand : Command
    {
        public List<BuildingToMove> BuildingsToMove;

        public MoveMultipleBuildingsCommand(PacketReader reader)
        {
            int count = reader.ReadInt32();
            BuildingsToMove = new List<BuildingToMove>(count);
            for (int i = 0; i < count; ++i)
            {
                BuildingsToMove.Add(new BuildingToMove()
                {
                    X = reader.ReadInt32(),
                    Y = reader.ReadInt32(),
                    GameObjectId = reader.ReadInt32()
                });
            }

            reader.ReadInt32();
        }

        public override void Execute(Level level)
        {
            for (int i = 0; i < BuildingsToMove.Count; i++)
            {
                var buildingToMove = BuildingsToMove[i];
                var gameObject = level.GameObjectManager.GetGameObjectByID(buildingToMove.GameObjectId);
                if (gameObject == null)
                {
                    //LogicLogger.Log("");
                    return;
                }

                var activeLayout = level.Avatar.GetActiveLayout();
                gameObject.SetPosition(buildingToMove.X, buildingToMove.Y, activeLayout);
            }
        }
    }
}