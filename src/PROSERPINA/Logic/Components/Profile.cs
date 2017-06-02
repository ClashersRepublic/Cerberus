using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.Servers.CR.Logic;
using BL.Servers.CR.Extensions.List;
using BL.Servers.CR.Logic.Slots.Items;
using BL.Servers.CR.Core;

namespace BL.Servers.CR.Logic.Components
{
    internal class Profile
    {
        internal Player Player;

        internal Profile(Player Player)
        {
            this.Player = Player;
        }

        internal byte[] ToBytes
        {
            get
            {
                int TimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                List<byte> _Packet = new List<byte>();

                _Packet.AddVInt(this.Player.UserId);

                _Packet.AddVInt(this.Player.UserId);

                _Packet.AddVInt(this.Player.UserId);

                if (Player.Rank == Enums.Rank.User)
                    _Packet.AddString(this.Player.Username); // Name
                else if (Player.Rank == Enums.Rank.Moderator)
                    _Packet.AddString("[Mod]" + this.Player.Username);
                else
                    _Packet.AddString("[Dev]" + this.Player.Username);

                _Packet.AddVInt(this.Player.Changes); // Changes

                _Packet.AddVInt(this.Player.Arena); // Arena

                _Packet.AddVInt(this.Player.Trophies); // Trophies

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(this.Player.Legendary_Trophies); // Legend Trophies

                _Packet.AddVInt(0); // Current Trophies => Season Higheset

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Leaderboard NR => Best Season

                _Packet.AddVInt(0); // Trophies => Best Season

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(30); // Unknown

                _Packet.AddVInt(0); // Leaderboard NR => Previous Season

                _Packet.AddVInt(0); // Trophies => Previous Season

                _Packet.AddVInt(0); // Highest Trophies

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(this.Player.Resources.Count);
                _Packet.AddVInt(this.Player.Resources.Count);

                foreach (Slots.Items.Resource _Resource in this.Player.Resources.OrderBy(r => r.Identifier))
                {
                    _Packet.AddVInt(_Resource.Type);
                    _Packet.AddVInt(_Resource.Identifier);
                    _Packet.AddVInt(_Resource.Value);
                }

                _Packet.Add(0);

                _Packet.AddVInt(this.Player.Achievements.Count);

                foreach (Achievement _Achievement in this.Player.Achievements)
                {
                    _Packet.AddVInt(_Achievement.Type);
                    _Packet.AddVInt(_Achievement.Identifier);
                    _Packet.AddVInt(_Achievement.Value);
                }

                _Packet.AddVInt(this.Player.Achievements.Completed.Count);
                foreach (Achievement _Achievement in this.Player.Achievements.Completed)
                {
                    _Packet.AddVInt(_Achievement.Type);
                    _Packet.AddVInt(_Achievement.Identifier);
                    _Packet.AddVInt(_Achievement.Value);
                }

                _Packet.AddVInt(0);

                _Packet.Add(0);
                _Packet.Add(0);

                _Packet.AddVInt(this.Player.Resources.Gems); // Gems

                _Packet.AddVInt(this.Player.Resources.Gems); // Gems

                _Packet.AddVInt(this.Player.Experience); // Experience

                _Packet.AddVInt(this.Player.Level); // Level

                _Packet.Add(this.Player.NameSet);

                // 7 = Name already set + no clan
                // 8 = Set name popup + clan
                // 9 = Name already set + clan
                // < 7 =  Set name popup

                if (this.Player.ClanLowID != 0)
                {
                    Clan Clan = Core.Resources.Clans.Get(this.Player.ClanId, Constants.Database, false);

                    _Packet.Add(string.IsNullOrEmpty(this.Player.Username) ? (byte)8 : (byte)9);

                    _Packet.AddVInt(Clan.ClanID);

                    _Packet.AddString(Clan.Name);

                    _Packet.Add((byte)Clan.Badge);

                    _Packet.AddVInt((int)Clan.Members[this.Player.UserId].Role);
                }
                else
                    _Packet.Add(string.IsNullOrEmpty(this.Player.Username) ? (byte)0 : (byte)7);

                _Packet.AddVInt(this.Player.Games_Played); // Games Played

                _Packet.AddVInt(0); // Matched Played -> Tournament Stats

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(this.Player.Wins); // Win

                _Packet.AddVInt(this.Player.Loses); // Loses

                _Packet.AddVInt(0); // Win Streak

                _Packet.AddVInt(this.Player.Tutorial); // Tutorial

                _Packet.AddVInt(0); // Tournament?

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(0); // Unknown

                _Packet.AddVInt(TimeStamp); // Countdown?

                _Packet.AddVInt((int)this.Player.Created.Subtract(new DateTime(1970, 1, 1)).TotalSeconds); // Creation Date

                _Packet.AddVInt((int)this.Player.Update.Subtract(this.Player.Created).TotalSeconds); // Time Played

                return _Packet.ToArray();
            }
        }
    }
}
