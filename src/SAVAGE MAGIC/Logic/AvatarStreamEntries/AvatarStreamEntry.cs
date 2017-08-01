using System;
using System.Collections.Generic;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic.AvatarStreamEntries
{
    internal class AvatarStreamEntry
    {
        private DateTime m_vCreationTime;
        private int m_vId;
        private byte m_vIsNew;
        private long m_vSenderId;
        private int m_vSenderLeagueId;
        private int m_vSenderLevel;
        private string m_vSenderName;

        public AvatarStreamEntry()
        {
            m_vCreationTime = DateTime.UtcNow;
        }

        public virtual byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(GetStreamEntryType());
            data.AddInt64(m_vId);
            data.Add(1);
            data.AddInt64(m_vSenderId);
            data.AddString(m_vSenderName);
            data.AddInt32(m_vSenderLevel);
            data.AddInt32(m_vSenderLeagueId);
            data.AddInt32(10);
            data.Add(m_vIsNew);
            return data.ToArray();
        }

        public int GetAgeSeconds() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds -
        (int)m_vCreationTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public int GetId() => m_vId;

        public long GetSenderAvatarId() => m_vSenderId;

        public int GetSenderLevel() => m_vSenderLevel;

        public string GetSenderName() => m_vSenderName;

        public virtual int GetStreamEntryType() => -1;

        public byte IsNew() => m_vIsNew;

        public void SetAvatar(Avatar avatar)
        {
            m_vSenderId = avatar.Id;
            m_vSenderName = avatar.GetAvatarName();
            m_vSenderLevel = avatar.GetAvatarLevel();
            m_vSenderLeagueId = avatar.GetLeagueId();
        }

        public void SetId(int id)
        {
            m_vId = id;
        }

        public void SetIsNew(byte isNew)
        {
            m_vIsNew = isNew;
        }

        public void SetSenderAvatarId(long id)
        {
            m_vSenderId = id;
        }

        public void SetSenderLeagueId(int id)
        {
            m_vSenderLeagueId = id;
        }

        public void SetSenderLevel(int level)
        {
            m_vSenderLevel = level;
        }

        public void SetSenderName(string name)
        {
            m_vSenderName = name;
        }
    }
}
