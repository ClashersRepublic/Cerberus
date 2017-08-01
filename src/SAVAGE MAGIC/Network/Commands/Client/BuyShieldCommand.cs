using System;
using System.IO;
using Savage.Magic.Core;
using Magic.Files.Logic;
using Savage.Magic;
using Savage.Magic.Logic;

namespace Savage.Magic.Network.Commands.Client
{
    internal class BuyShieldCommand : Command
    {
        public int ShieldId;
        public int Tick;

        public BuyShieldCommand(PacketReader br)
        {
            ShieldId = br.ReadInt32();
            Tick = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            Avatar avatar = level.Avatar;
            int time = avatar.GetShieldTime + Convert.ToInt32(TimeSpan.FromHours((double)((ShieldData)CsvManager.DataTables.GetDataById(ShieldId)).TimeH).TotalSeconds);
            avatar.SetShieldTime(time);
            int diamonds = ((ShieldData) CsvManager.DataTables.GetDataById(ShieldId)).Diamonds;
            avatar.UseDiamonds(diamonds);
        }
    }
}
