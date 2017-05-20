using BL.Servers.CoC.Core;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets.Messages.Server.Clans
{
    internal class Alliance_Joinable_Data : Message
    {

        internal long Alliance_Count = (Resources.Clans.Seed - 1 >= 50 ? 50 : Resources.Clans.Seed - 1);

        public Alliance_Joinable_Data(Device _Device) : base(_Device)
        {
            this.Identifier = 24304;
        }

        internal override void Encode()
        {
            /*
            long Base_ID = 0;
            if (Resources.Clans.Seed - this.Alliance_Count > 0)
                Base_ID = Resources.Random.Next(1, (int)Resources.Clans.Seed - (int)this.Alliance_Count);

            this.Data.AddInt((int)this.Alliance_Count);

            for (int Index = 0; Index < this.Alliance_Count; Index++)
            {
                Clan _Clan = Resources.Clans.Get(Base_ID + Index, Constants.Database, false);
                this.Data.AddRange(_Clan.ToBytes);
            }*/
            this.Data.AddInt((int)Resources.Clans.Count);
            foreach (var _Clan in Resources.Clans.Values)
            {
                this.Data.AddRange(_Clan.ToBytes);
            }
        }
    }
}
