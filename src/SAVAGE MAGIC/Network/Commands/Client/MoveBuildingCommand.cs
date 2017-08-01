using System.IO;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class MoveBuildingCommand : Command
    {
        public int X;
        public int Y;
        public int GameObjectId;
        public uint Unknown1;

        public MoveBuildingCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            GameObjectId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            var gameObject = level.GameObjectManager.GetGameObjectByID(GameObjectId);
            if (gameObject == null)
            {
                //LogicLogger.Log("");
                return;
            }

            var activeLayout = level.Avatar.GetActiveLayout();
            gameObject.SetPosition(X, Y, activeLayout);
        }
    }
}