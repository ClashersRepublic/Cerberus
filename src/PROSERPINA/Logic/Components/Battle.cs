using BL.Servers.CR.Extensions.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Servers.CR.Logic.Components
{
    internal class Battle
    {
        internal Player Player;

        internal Battle(Player Player)
        {
            this.Player = Player;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddVInt(this.Player.UserId);

                _Packet.AddVInt(this.Player.UserId);

                _Packet.AddVInt(this.Player.UserId);

                _Packet.AddString(this.Player.Username); // Name

                _Packet.AddVInt(this.Player.Arena); // Arena

                _Packet.AddVInt(this.Player.Trophies); // Trophies

                _Packet.AddRange("AC04000AA38909BC33001E919133B82E000000".HexaToBytes());

                _Packet.AddVInt(this.Player.Resources.Count);
                _Packet.AddVInt(this.Player.Resources.Count);

                foreach (var _Resource in this.Player.Resources.OrderBy(r => r.Identifier))
                {
                    _Packet.AddVInt(_Resource.Type);
                    _Packet.AddVInt(_Resource.Identifier);
                    _Packet.AddVInt(_Resource.Value);
                }

                _Packet.AddVInt(0); // Count

                _Packet.AddVInt(this.Player.Achievements.Count); // Achievement Count

                foreach (var _Achievement in this.Player.Achievements)
                {
                    _Packet.AddVInt(_Achievement.Type);
                    _Packet.AddVInt(_Achievement.Identifier);
                    _Packet.AddVInt(_Achievement.Value);
                }

                _Packet.AddVInt(this.Player.Achievements.Completed.Count);

                _Packet.AddVInt(0); // Count 0506  stuff
                _Packet.AddVInt(0); // Count 1a00 stuff

                _Packet.AddVInt(0);

                _Packet.AddHexa("0A008D30AB10008D17A3147EB901");

                return _Packet.ToArray();
            }
        }
    }
}
