using System;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Logic.Structure;

namespace BL.Servers.CoC.Packets.Commands.Client
{
    internal class Remove_Obstacle : Command
    {
        internal int ObstacleID;
        internal int Tick;
        public Remove_Obstacle(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.ObstacleID = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }
        internal override void Process()
        {
            ShowValues();
            if (!this.Device.Player.Avatar.Variables.IsBuilderVillage)
            {
                var Object = (Obstacle) this.Device.Player.GameObjectManager.GetGameObjectByID(this.ObstacleID);
                if (Object != null)
                {
                    var obstacleData = Object.GetObstacleData();
                    int ResourceID = obstacleData.GetClearingResource().GetGlobalID();
                    if (this.Device.Player.Avatar.HasEnoughResources(ResourceID, obstacleData.ClearCost) &&
                        this.Device.Player.HasFreeWorkers)
                    {
                        this.Device.Player.Avatar.Resources.Minus(ResourceID, obstacleData.ClearCost);
                        Object.StartClearing();
                    }
                }
            }
            else
            {
                var Object = (Builder_Obstacle)this.Device.Player.GameObjectManager.GetBuilderVillageGameObjectByID(this.ObstacleID);
                if (Object != null)
                {
                    var obstacleData = Object.GetObstacleData;
                    int ResourceID = obstacleData.GetClearingResource().GetGlobalID();
                    if (this.Device.Player.Avatar.HasEnoughResources(ResourceID, obstacleData.ClearCost) &&
                        this.Device.Player.HasFreeWorkers)
                    {
                        this.Device.Player.Avatar.Resources.Minus(ResourceID, obstacleData.ClearCost);
                        Object.StartClearing();
                    }
                }
            }
        }
    }
}
