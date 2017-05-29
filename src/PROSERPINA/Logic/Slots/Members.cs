using System.Collections.Generic;
using BL.Servers.CR.Logic.Enums;
using BL.Servers.CR.Logic.Slots.Items;

namespace BL.Servers.CR.Logic.Slots
{
    internal class Members : Dictionary<long, Member>
    {
        internal Clan Clan;

        internal object Gate = new object();
        public Members()
        {
        }

        internal Members(Clan Clan) : base(50)
        {
            this.Clan = Clan;
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
    }
}
