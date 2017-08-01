using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Logic.DataSlots;
using Magic.ClashOfClans.Core;
using System;
using System.Collections;

namespace Magic.ClashOfClans.Logic.StreamEntries
{
    internal class TroopRequestStreamEntry : StreamEntry
    {
        public static int ID = ObjectManager.DonationSeed;
        public int m_vMaxSpell = 1;

        public TroopRequestStreamEntry()
        {
            m_vUnitDonation = new System.Collections.Generic.List<DonationSlot>();
            m_vDonatorList = new System.Collections.Generic.List<BookmarkSlot>();
        }

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(((IEnumerable<byte>) base.Encode()));
            data.AddInt32(ID);
            data.AddInt32(m_vMaxTroop);
            data.AddInt32(m_vMaxSpell);
            data.AddInt32(m_vDonatedTroop);
            data.AddInt32(m_vDonatedSpell);
            data.AddInt32(m_vDonatorList.Count);
            for (int index1 = 0; index1 < m_vDonatorList.Count; index1++)
            {
                foreach (DonationSlot donationSlot1 in m_vUnitDonation)
                {
                    DonationSlot i = donationSlot1;
                    DonationSlot donationSlot2 = m_vUnitDonation.Find((Predicate<DonationSlot>)(t =>
                    {
                        if (t.DonatorID == i.DonatorID)
                            return t.UnitLevel == i.UnitLevel;
                        return false;
                    }));
                    data.AddInt64(donationSlot2.DonatorID);
                    data.AddInt32(donationSlot2.Count);

                    for (int index2 = 0; index2 < i.Count - 1; ++index2)
                    {
                        data.AddInt32(donationSlot2.ID);
                        data.AddInt32(donationSlot2.UnitLevel - 1);
                    }
                    data.AddInt32(donationSlot2.ID);
                    data.AddInt32(donationSlot2.UnitLevel);
                }
            }
            if (string.IsNullOrEmpty(m_vMessage))
            {
                data.Add((byte) 0);
            }
            else
            {
                data.Add((byte) 1);
                data.AddString(m_vMessage);
            }
            data.AddInt32(m_vUnitDonation.Count);
            foreach (DonationSlot donationSlot in m_vUnitDonation)
            {
                data.AddInt32(donationSlot.ID);
                data.AddInt32(donationSlot.Count);
                data.AddInt32(donationSlot.UnitLevel);
            }
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 1;

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            ID = jsonObject["rid"].ToObject<int>();
            m_vMaxTroop = jsonObject["max_troops"].ToObject<int>();
            m_vMaxSpell = jsonObject["max_spells"].ToObject<int>();
            m_vDonatedTroop = jsonObject["donated_troops"].ToObject<int>();
            m_vDonatedSpell = jsonObject["donated_spell"].ToObject<int>();
            using (IEnumerator<JToken> enumerator = ((JArray)jsonObject["donater_list"]).GetEnumerator())
            {
                while (((IEnumerator)enumerator).MoveNext())
                {
                    JObject current = (JObject)enumerator.Current;
                    BookmarkSlot bookmarkSlot = new BookmarkSlot(0L);
                    bookmarkSlot.Load(current);
                    this.m_vDonatorList.Add(bookmarkSlot);
                }
            }
            using (IEnumerator<JToken> enumerator = ((JArray)jsonObject["donated_unit"]).GetEnumerator())
            {
                while (((IEnumerator)enumerator).MoveNext())
                {
                    JObject current = (JObject)enumerator.Current;
                    DonationSlot donationSlot = new DonationSlot(0L, 0, 0, 0);
                    donationSlot.Load(current);
                    this.m_vUnitDonation.Add(donationSlot);
                }
            }
            m_vMessage = jsonObject["message"].ToObject<string>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("rid", ID);
            jsonObject.Add("max_troops", m_vMaxTroop);
            jsonObject.Add("max_spells", m_vMaxSpell);
            jsonObject.Add("donated_troops", m_vDonatedTroop);
            jsonObject.Add("donated_spell", m_vDonatedSpell);
            JArray jsonDonaterID = new JArray();
            foreach (BookmarkSlot id in m_vDonatorList)
                jsonDonaterID.Add(id.Save(new JObject()));
            jsonObject.Add("donater_list", jsonDonaterID);
            JArray jsonDonatedUnit = new JArray();
            foreach (DonationSlot unit in m_vUnitDonation)
                jsonDonatedUnit.Add(unit.Save(new JObject()));
            jsonObject.Add("donated_unit", jsonDonatedUnit);
            jsonObject.Add("message", m_vMessage);
            return jsonObject;
        }
    }
}
