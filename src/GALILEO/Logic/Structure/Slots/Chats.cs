using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions.List;
using BL.Servers.CoC.Logic.Structure.Slots.Items;
using BL.Servers.CoC.Packets.Messages.Server.Clans;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BL.Servers.CoC.Logic.Structure.Slots
{
    internal class Chats
    {
        [JsonProperty("seed")] internal int Seed;
        [JsonProperty("slots")] internal List<Entry> Slots;

        internal object Gate = new object();
        internal Clan Clan;

        internal Chats()
        {
            this.Slots = new List<Entry>(100);
        }

        internal Chats(Clan Clan, int Limit = 100)
        {
            this.Clan = Clan;
            this.Slots = new List<Entry>(Limit);
        }

        internal void Add(Entry Message)
        {
            lock (this.Gate)
            {
                Message.Message_LowID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                if (this.Slots.Count < this.Slots.Capacity)
                {
                    this.Slots.Add(Message);
                }
                else
                {
                    this.Slots.RemoveAt(0);
                    this.Slots.Add(Message);
                }
            }

            foreach (Member Member in this.Clan.Members.Values)
            {
                if (Member.Connected)
                {
                    new Alliance_Stream_Entry(Member.Player.Client, Message).Send();
                }
            }
        }
        internal void Remove(Entry Message)
        {
            if (Message != null)
            {
                int MessageID = Message.Message_LowID;
                bool Deleted = true;

                lock (this.Gate)
                {
                    Deleted = this.Slots.Remove(Message);
                }

                if (Deleted)
                {
                    foreach (Member Member in this.Clan.Members.Values)
                    {
                        if (Member.Connected)
                        {
                            new Alliance_Remove_Stream(Member.Player.Client) { Message_ID = MessageID }.Send();
                        }
                    }
                }
            }
        }

        internal Entry Get(int MessageID)
        {
            lock (this.Gate)
            {
                return this.Slots.Find(Input => Input.Message_LowID == MessageID);
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddInt(this.Slots.Count);

                foreach (Entry Message in this.Slots)
                {
                    Packet.AddRange(Message.ToBytes());
                }

                return Packet.ToArray();
            }
        }

    }
}
