using System;
using System.Collections.Generic;
using BL.Servers.CoC.Extensions.List;

namespace BL.Servers.CoC.Logic
{
    internal class Objects : IDisposable
    {
        internal Level Player;
        internal string Json;

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

                _Packet.AddInt((int)Player.Avatar.LastTick.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

                _Packet.AddLong(this.Player.Avatar.UserId);

                _Packet.AddInt(this.Player.Avatar.Shield);
                _Packet.AddInt(this.Player.Avatar.Guard);

                _Packet.AddInt((int) TimeSpan.FromDays(365).TotalSeconds);

                _Packet.AddCompressed(this.Json);
                _Packet.AddCompressed("{\"event\":[]}");

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
