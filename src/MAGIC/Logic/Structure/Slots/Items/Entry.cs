using CRepublic.Magic.Extensions.List;
using CRepublic.Magic.Logic.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class Entry
    {
        [JsonProperty("type")] internal Alliance_Stream Stream_Type = Alliance_Stream.NONE;
        [JsonProperty("high_id")] internal int Message_HighID;
        [JsonProperty("low_id")] internal int Message_LowID;

        [JsonProperty("sender_id")] internal long Sender_ID;
        [JsonProperty("sender_name")] internal string Sender_Name;
        [JsonProperty("sender_role")] internal Role Sender_Role;
        [JsonProperty("sender_league")] internal int Sender_League;
        [JsonProperty("sender_lvl")] internal int Sender_Level;
        [JsonProperty("date")] internal DateTime Sent = DateTime.UtcNow;

        // Stream 1
        [JsonProperty("max_troops", DefaultValueHandling = DefaultValueHandling.Ignore)] internal int Max_Troops = 0;
        [JsonProperty("max_spells", DefaultValueHandling = DefaultValueHandling.Ignore)] internal int Max_Spells = 0;

        [JsonProperty("space_troops", DefaultValueHandling = DefaultValueHandling.Ignore)] internal int Used_Space_Troops = 0;
        [JsonProperty("space_spells", DefaultValueHandling = DefaultValueHandling.Ignore)] internal int Used_Space_Spells = 0;

        [JsonProperty("units", DefaultValueHandling = DefaultValueHandling.Ignore)] internal List<Alliance_Unit> Units = new List<Alliance_Unit>();

        [JsonProperty("spells", DefaultValueHandling = DefaultValueHandling.Ignore)] internal List<Alliance_Unit> Spells = new List<Alliance_Unit>();

        [JsonProperty("have_message", DefaultValueHandling = DefaultValueHandling.Ignore)] internal bool Have_Message = false;

        // Stream 1/2/3
        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Message = string.Empty;

        // Stream 3InviteState
        [JsonProperty("judge_name", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Judge_Name = string.Empty;
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)] internal InviteState Stream_State = InviteState.WAITING;

        // Stream 4
        [JsonProperty("event_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal Events Event_ID = 0;
        [JsonProperty("event_pl_id", DefaultValueHandling = DefaultValueHandling.Ignore)] internal long Event_Player_ID = 0;
        [JsonProperty("event_pl_name", DefaultValueHandling = DefaultValueHandling.Ignore)] internal string Event_Player_Name = string.Empty;

        // Steam 12
        [JsonProperty("amical_status", DefaultValueHandling = DefaultValueHandling.Ignore)] internal Amical_Mode Amical_State = Amical_Mode.ATTACK;

        [JsonIgnore]
        internal int GetTime => (int)DateTime.UtcNow.Subtract(this.Sent).TotalSeconds;

        internal void AddTroop(long DonatorId, int Data, int Count, int level)
        {
            int _Index = this.Units.FindIndex(t => t.Data == Data && t.Player_ID == DonatorId && t.Level == level);
            if (_Index > -1)
            {
                this.Units[_Index].Count += Count;
            }
            else
            {
                Alliance_Unit ds = new Alliance_Unit(DonatorId, Data, Count, level);
                this.Units.Add(ds);
            }
        }

        internal void AddSpell(long DonatorId, int Data, int Count, int level)
        {
            int _Index = this.Spells.FindIndex(t => t.Data == Data && t.Player_ID == DonatorId && t.Level == level);
            if (_Index > -1)
            {
                this.Spells[_Index].Count += Count;
            }
            else
            {
                Alliance_Unit ds = new Alliance_Unit(DonatorId, Data, Count, level);
                this.Spells.Add(ds);
            }
        }


        internal byte[] ToBytes()
        {
            List<byte> _Packet = new List<byte>();
            _Packet.AddInt((int)this.Stream_Type);
            _Packet.AddInt(this.Message_HighID);    
            _Packet.AddInt(this.Message_LowID);
            _Packet.Add(3);

            _Packet.AddLong(this.Sender_ID);
            _Packet.AddLong(this.Sender_ID);
            _Packet.AddString(this.Sender_Name);

            _Packet.AddInt(this.Sender_Level);
            _Packet.AddInt(this.Sender_League);
            _Packet.AddInt((int)this.Sender_Role);
            _Packet.AddInt(this.GetTime);

            switch (this.Stream_Type)
            {
                case Alliance_Stream.TROOP_REQUEST:
                    _Packet.AddInt(this.Message_LowID);
                    _Packet.AddInt(this.Max_Troops);
                    _Packet.AddInt(this.Max_Spells);
                    _Packet.AddInt(this.Used_Space_Troops);
                    _Packet.AddInt(this.Used_Space_Spells);
                    _Packet.AddInt(0); // Donator Count

                    _Packet.AddBool(this.Have_Message);
                    if (this.Have_Message)
                        _Packet.AddString(this.Message);

                    _Packet.AddInt(this.Units.Count + this.Spells.Count);
                    foreach (Alliance_Unit Alliance_Unit in this.Units)
                    {
                        _Packet.AddInt(Alliance_Unit.Data);
                        _Packet.AddInt(Alliance_Unit.Count);
                        _Packet.AddInt(Alliance_Unit.Level);
                    }
                    foreach (Alliance_Unit Alliance_Spell in this.Spells)
                    {
                        _Packet.AddInt(Alliance_Spell.Data);
                        _Packet.AddInt(Alliance_Spell.Count);
                        _Packet.AddInt(Alliance_Spell.Level);
                    }

                    break;
                case Alliance_Stream.CHAT:
                    _Packet.AddString(this.Message);
                    break;
                case Alliance_Stream.INVITATION:
                    _Packet.AddString(this.Message);
                    _Packet.AddString(this.Judge_Name);
                    _Packet.AddInt((int)this.Stream_State);
                    break;
                case Alliance_Stream.EVENT:
                    _Packet.AddInt((int)this.Event_ID);
                    _Packet.AddLong(this.Event_Player_ID);
                    _Packet.AddString(this.Event_Player_Name);
                    break;
                case Alliance_Stream.AMICAL_BATTLE:
                    _Packet.AddString(this.Message);
                    _Packet.AddInt((int)this.Amical_State);
                    break;
            }
            return _Packet.ToArray();
        }
    }
}