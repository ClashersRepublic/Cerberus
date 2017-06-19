using System;
using System.Collections.Generic;
using System.IO;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.List;

namespace CRepublic.Magic.Logic
{
    internal class Objects : IDisposable
    {
        internal Level Player;
        internal string Json;
        internal DateTime Timestamp = DateTime.UtcNow;

        internal Objects(Level player, String Village)
        {
            this.Player = player;
            this.Json = Village;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddInt((int)TimeUtils.ToUnixTimestamp(Timestamp));

                _Packet.AddLong(this.Player.Avatar.UserId);

                _Packet.AddInt(this.Player.Avatar.Shield);
                _Packet.AddInt(this.Player.Avatar.Guard);

                _Packet.AddInt((int) TimeSpan.FromDays(365).TotalSeconds); //Personal break

                _Packet.AddCompressed(this.Json);
                //_Packet.AddCompressed(File.ReadAllText(@"starting_home.json"));
                _Packet.AddCompressed("{\"event\":[]}");
                _Packet.AddCompressed("{\"Village2\":{\"TownHallMaxLevel\":8}}");
                return _Packet.ToArray();
            }
        }

        void IDisposable.Dispose()
        {
            this.Player = null;
            this.Json = null;
        }
    }
}
