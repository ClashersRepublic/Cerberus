using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Republic.Magic.Core;
using Republic.Magic.Extensions.List;
using Republic.Magic.Logic;
using Republic.Magic.Logic.Enums;

namespace Republic.Magic.Packets.Messages.Server.Battle
{
    internal class V2_Battle_Result : Message
    {
        public V2_Battle_Result(Device Device) : base(Device)
        {
            this.Identifier = 24371;
            this.Device.State = State.LOGGED;
        }

        internal override void Encode()
        {
            this.Data.AddHexa("00 00 00 01 00 00 00 00 00 04 36 9F BE 01 00 00 00 03 00 3F B2 BA");
            this.Data.AddString("In our memory");
            this.Data.AddHexa("5B 00 1A 5A 00 00 00 02 00 00 00 0D 01 81 EC E8 00 00 00 0D 01 81 EC E8");
            this.Data.AddString("Rip CrayCray");
            this.Data.AddInt(2);
            this.Data.AddByte(0);
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("98EA01");
            this.Data.AddHexa("009103"); //009203 also exist
            this.Data.AddByte(0);
            this.Data.AddInt(0);
            this.Data.AddInt(6969);
            this.Data.AddInt(0);
            this.Data.AddInt(0);
            this.Data.AddHexa("00 00 00 00 00 00 00 00 FF FF FF FB 03 00 00 00 00 F5 CC 17 19 00 00 00 0D 00 00 00 09 00 00 00 18 00 00 00 05 01 00 00 00 00 F5 CC 15 BB 00 00 00 0D 00 00 00 09 00 00 00 18 00 00 00 05 00 00 00 1F");
            this.Data.AddString(Resources.Battles_V2.GetPlayer(this.Device.Player.Avatar.Battle_ID_V2, this.Device.Player.Avatar.UserId).Replay_Info.Json);
            this.Data.AddString(null);
            //this.Data.AddHexa("00 00 01 3C 7B 22 76 69 6C 6C 61 67 65 54 79 70 65 22 3A 31 2C 22 75 6E 69 74 73 22 3A 5B 5B 34 30 30 30 30 33 31 2C 38 5D 5D 2C 22 6C 65 76 65 6C 73 22 3A 5B 5B 34 30 30 30 30 33 31 2C 31 5D 5D 2C 22 73 74 61 74 73 22 3A 7B 22 74 6F 77 6E 68 61 6C 6C 44 65 73 74 72 6F 79 65 64 22 3A 66 61 6C 73 65 2C 22 62 61 74 74 6C 65 45 6E 64 65 64 22 3A 74 72 75 65 2C 22 64 65 73 74 72 75 63 74 69 6F 6E 50 65 72 63 65 6E 74 61 67 65 22 3A 33 30 2C 22 62 61 74 74 6C 65 54 69 6D 65 22 3A 31 34 2C 22 61 6C 6C 69 61 6E 63 65 4E 61 6D 65 22 3A 22 55 4C 54 52 41 20 46 4F 52 43 45 22 2C 22 68 6F 6D 65 49 44 22 3A 5B 31 33 2C 32 35 32 39 32 30 30 38 5D 2C 22 61 6C 6C 69 61 6E 63 65 42 61 64 67 65 22 3A 31 35 32 36 37 33 33 34 30 32 2C 22 61 6C 6C 69 61 6E 63 65 42 61 64 67 65 32 22 3A 30 2C 22 61 6C 6C 69 61 6E 63 65 49 44 22 3A 5B 33 2C 34 31 37 34 35 32 32 5D 2C 22 61 6C 6C 69 61 6E 63 65 45 78 70 22 3A 32 2C 22 61 6C 6C 69 61 6E 63 65 45 78 70 32 22 3A 31 7D 7D".Replace(" ", "")););
        }
    }
}
