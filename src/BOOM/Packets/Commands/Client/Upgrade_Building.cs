using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Boom.Core;
using CRepublic.Boom.Extensions.Binary;
using CRepublic.Boom.Logic;
using CRepublic.Boom.Logic.Structure;

namespace CRepublic.Boom.Packets.Commands.Client
{
    internal class Upgrade_Building : Command
    {
        public int BuildingId;

        public bool Instant;
        public uint Unknown1;

        public Upgrade_Building(Reader reader, Device client, int id) : base(reader, client, id)
        {

        }
        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Instant = this.Reader.ReadBoolean();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            ShowValues();
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);
            if (go != null)
            {
                var b = (ConstructionItem) go;
                if (b.CanUpgrade())
                {
                    var bd = b.GetConstructionItemData();
                    //if (ca.HasEnoughResources(bd.GetBuildResource(b.GetUpgradeLevel() + 1), bd.GetBuildCost(b.GetUpgradeLevel() + 1)))
                    {
                        string name = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId)
                            .GetData()
                            .Row.Name;
#if DEBUG
                        Loggers.Log("Building To Upgrade : " + name + " (" + BuildingId + ')', true);
#endif

                        if (string.Equals(name, "HQ"))
                            ca.HQ_Level++;
                        //var rd = bd.GetBuildResource(b.GetUpgradeLevel() + 1);
                        //ca.SetResourceCount(rd, ca.GetResourceCount(rd) - bd.GetBuildCost(b.GetUpgradeLevel() + 1));
                        b.StartUpgrading();
                        if (this.Instant)
                        {
                            b.SpeedUpConstruction();
                        }
                    }
                }
            }
        }
    }
}
