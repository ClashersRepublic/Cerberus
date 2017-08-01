using System;
using System.Collections.Generic;
using Magic.Files.Logic;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic;

namespace Magic.ClashOfClans.Network.Commands.Server
{
    internal class DonatedAllianceUnitCommand : Command
    {
        public string Donator { get; set; }

        public int TroopLevel { get; set; }

        public int TroopID { get; set; }


        public DonatedAllianceUnitCommand()
        {
        }

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddInt32(0);
            data.AddInt32(TroopID);
            data.AddInt32(TroopLevel);
            data.AddInt32(1);
            data.AddInt32(0);
            return data.ToArray();
        }

        public int SetUnitLevel(int t)
        {
            return this.TroopLevel = t;
        }

        public int SetUnitID(int i)
        {
            return this.TroopID = i;
        }

        public void SetDonator(string name)
        {
            this.Donator = name;
        }

        public void Tick(Level level)
        {
            level.Tick();
        }
    }
}
