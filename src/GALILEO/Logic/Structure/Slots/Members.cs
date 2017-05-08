using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic.Enums;
using BL.Servers.CoC.Logic.Structure.Slots.Items;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Members : Dictionary<long, Member>
    {

        internal Clan Alliance;

        internal object Gate = new object();
        public Members()
        {
        }

        internal Members(Clan Alliance) : base(50)
        {
            this.Alliance = Alliance;
        }

        internal void Add(Player Player)
        {
            lock (this.Gate)
            {
                Member Member = new Member(Player);
                if (this.ContainsKey(Member.UserID))
                {
                    this[Member.UserID] = Member;
                }
                else
                {
                    if (this.Count < 1)
                    {
                        Member.Role = Role.Leader;
                    }

                    this.Add(Member.UserID, Member);
                }
            }
        }

        internal void Add(Member _Member)
        {
            lock (this.Gate)
            {
                if (this.ContainsKey(_Member.UserID))
                    this[_Member.UserID] = _Member;
                else
                    this.Add(_Member.UserID, _Member);
            }
        }
        internal void Remove(Player Player)
        {
            lock (this.Gate)
            {
                if (this.ContainsKey(Player.UserId))
                {
                    this.Remove(Player.UserId);
                }
            }
        }

        internal void Remove(Member Member)
        {
            lock (this.Gate)
            {
                if (this.ContainsKey(Member.UserID))
                {
                    this.Remove(Member.UserID);
                }
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> _Packet = new List<byte>();

                _Packet.AddInt(this.Values.Count);
                    
                foreach (Member Member in this.Values)
                {
                    Level _Player = Core.Resources.Players.Get(Member.UserID, Constants.Database, false);

                    _Packet.AddLong(_Player.Avatar.UserId);
                    _Packet.AddString(_Player.Avatar.Name);
                    _Packet.AddInt((int) Member.Role);
                    _Packet.AddInt(_Player.Avatar.Level);
                    _Packet.AddInt(_Player.Avatar.League);
                    _Packet.AddInt(_Player.Avatar.Trophies);
                    _Packet.AddInt(Member.Donations);
                    _Packet.AddInt(Member.Received);
                    _Packet.AddInt(0); // Order ?
                    _Packet.AddInt(0); // Previous Order ?
                    _Packet.AddBool(Member.New);
                    _Packet.AddInt(0);
                    _Packet.AddInt(_Player.Avatar.WarState ? 1 : 0);
                    _Packet.AddInt(1);
                    _Packet.AddLong(_Player.Avatar.UserId);
                }
                return _Packet.ToArray();
            }
        }
    }
}
