using Magic.ClashOfClans;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Logic.StreamEntries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Magic.ClashOfClans.Logic
{
    internal class Alliance
    {
        private readonly object _sync = new object();

        private const int m_vMaxAllianceMembers = 50;
        private const int m_vMaxChatMessagesNumber = 30;
        private readonly Dictionary<long, AllianceMemberEntry> m_vAllianceMembers;
        private readonly System.Collections.Generic.List<Magic.ClashOfClans.Logic.StreamEntries.StreamEntry> m_vChatMessages;
        private int m_vAllianceBadgeData;
        private string m_vAllianceDescription;
        private int m_vAllianceExperience;
        private long m_vAllianceId;
        private int m_vAllianceLevel;
        private string m_vAllianceName;
        private int m_vAllianceOrigin;
        private int m_vAllianceType;
        private int m_vDrawWars;
        private int m_vLostWars;
        private int m_vRequiredScore;
        private int m_vScore;
        private int m_vWarFrequency;
        private byte m_vWarLogPublic;
        private int m_vWonWars;
        private byte m_vFriendlyWar;

        public Alliance()
        {
            m_vChatMessages = new List<StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public Alliance(long id)
        {
            Random random = new Random();

            m_vAllianceId = id;
            m_vAllianceName = "Default";
            m_vAllianceDescription = "Default";
            m_vAllianceBadgeData = 0;
            m_vAllianceType = 0;
            m_vRequiredScore = 0;
            m_vWarFrequency = 0;
            m_vAllianceOrigin = 32000001;
            m_vScore = 0;
            m_vAllianceExperience = random.Next(100, 5000);
            m_vAllianceLevel = random.Next(6, 10);
            m_vWonWars = random.Next(200, 500);
            m_vLostWars = random.Next(100, 300);
            m_vDrawWars = random.Next(100, 800);
            m_vChatMessages = new List<StreamEntry>();
            m_vAllianceMembers = new Dictionary<long, AllianceMemberEntry>();
        }

        public void AddAllianceMember(AllianceMemberEntry entry)
        {
            lock (_sync)
            {
                m_vAllianceMembers.Add(entry.GetAvatarId(), entry);
            }
        }

        public void AddChatMessage(Magic.ClashOfClans.Logic.StreamEntries.StreamEntry message)
        {
            lock (_sync)
            {
                if (m_vChatMessages.Count >= 30)
                    m_vChatMessages.RemoveAt(0);

                m_vChatMessages.Add(message);
            }
        }

        public byte[] EncodeFullEntry()
        {
            var data = new List<byte>();
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.AddInt32(m_vAllianceType);
            data.AddInt32(m_vAllianceMembers.Count);
            data.AddInt32(m_vScore);
            data.AddInt32(m_vRequiredScore);
            data.AddInt32(m_vWonWars);
            data.AddInt32(m_vLostWars);
            data.AddInt32(m_vDrawWars);
            data.AddInt32(20000001);
            data.AddInt32(m_vWarFrequency);
            data.AddInt32(m_vAllianceOrigin);
            data.AddInt32(m_vAllianceExperience);
            data.AddInt32(m_vAllianceLevel);

            data.AddInt32(0);
            data.AddInt32(0);
            data.Add(m_vWarLogPublic);
            data.Add(m_vFriendlyWar);
            return data.ToArray();
        }

        public byte[] EncodeHeader()
        {
            List<byte> data = new List<byte>();
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.Add(0);
            data.AddInt32(m_vAllianceLevel);
            data.AddInt32(1);
            data.AddInt32(-1);
            return data.ToArray();
        }


        public int AllianceBadgeData
        {
            get
            {
                return m_vAllianceBadgeData;
            }

            set
            {
                m_vAllianceBadgeData = value;
            }
        }


        public string AllianceDescription
        {
            get
            {
                return m_vAllianceDescription;
            }

            set
            {
                m_vAllianceDescription = value;
            }
        }

        public int AllianceExperience
        {
            get
            {
                return m_vAllianceExperience;
            }
        }

        public long AllianceId
        {
            get
            {
                return m_vAllianceId;
            }
        }


        public int AllianceLevel
        {
            get
            {
                return m_vAllianceLevel;
            }
            set
            {
                m_vAllianceLevel = value;
            }
        }

        public List<AllianceMemberEntry> AllianceMembers
        {
            get
            {
                return m_vAllianceMembers.Values.ToList<AllianceMemberEntry>();
            }
        }


        public string AllianceName
        {
            get
            {
                return m_vAllianceName;
            }
            set
            {
                m_vAllianceName = value;
            }
        }


        public int AllianceOrigin
        {
            get
            {
                return m_vAllianceOrigin;
            }
            set
            {
                m_vAllianceOrigin = value;
            }
        }


        public int AllianceType
        {
            get
            {
                return m_vAllianceType;
            }
            set
            {
                m_vAllianceType = value;
            }
        }

        public List<StreamEntry> ChatMessages
        {
            get
            {
                return m_vChatMessages;
            }
        }


        public int RequiredScore
        {
            get
            {
                return m_vRequiredScore;
            }
            set
            {
                m_vRequiredScore = value;
            }
        }

        public int Score
        {
            get
            {
                return m_vScore;
            }
        }


        public int WarFrequency
        {
            get
            {
                return m_vWarFrequency;
            }
            set
            {
                m_vWarFrequency = value;
            }
        }

        public int WarScore
        {
            get
            {
                return m_vWonWars;
            }
        }

        public byte WarLogPublic
        {
            get
            {
                return m_vWarLogPublic;
            }
        }


        public byte FriendlyWar
        {
            get
            {
                return m_vFriendlyWar;
            }
            set
            {
                m_vFriendlyWar = value;
            }
        }

        public bool IsAllianceFull
        {
            get
            {
                return m_vAllianceMembers.Count >= 50;
            }
        }

        public AllianceMemberEntry GetAllianceMember(long avatarId)
        {
            var member = (AllianceMemberEntry)null;
            if (!m_vAllianceMembers.TryGetValue(avatarId, out member))
                return null;

            return member;
        }

        public void LoadFromJson(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);
            m_vAllianceId = jsonObject["alliance_id"].ToObject<long>();
            m_vAllianceName = jsonObject["alliance_name"].ToObject<string>();
            m_vAllianceBadgeData = jsonObject["alliance_badge"].ToObject<int>();
            m_vAllianceType = jsonObject["alliance_type"].ToObject<int>();
            m_vRequiredScore = jsonObject["required_score"].ToObject<int>();
            m_vAllianceDescription = jsonObject["description"].ToObject<string>();
            m_vAllianceExperience = jsonObject["alliance_experience"].ToObject<int>();
            m_vAllianceLevel = jsonObject["alliance_level"].ToObject<int>();
            m_vWarLogPublic = jsonObject["war_log_public"].ToObject<byte>();
            m_vFriendlyWar = jsonObject["friendly_war"].ToObject<byte>();
            m_vWonWars = jsonObject["won_wars"].ToObject<int>();
            m_vLostWars = jsonObject["lost_wars"].ToObject<int>();
            m_vDrawWars = jsonObject["draw_wars"].ToObject<int>();
            m_vWarFrequency = jsonObject["war_frequency"].ToObject<int>();
            m_vAllianceOrigin = jsonObject["alliance_origin"].ToObject<int>();

            var jsonMembers = (JArray)jsonObject["members"];
            foreach (var jToken in jsonMembers)
            {
                var jsonMember = (JObject)jToken;

                var id = jsonMember["avatar_id"].ToObject<long>();
                var player = ResourcesManager.GetPlayer(id);
                var member = new AllianceMemberEntry(id);

                m_vScore = m_vScore + player.Avatar.GetScore();

                member.Load(jsonMember);
                m_vAllianceMembers.Add(id, member);
            }
            m_vScore = m_vScore / 2;

            var jsonMessages = (JArray)jsonObject["chatMessages"];
            if (jsonMessages != null)
            {
                foreach (JToken jToken in jsonMessages)
                {
                    JObject jsonMessage = (JObject)jToken;
                    StreamEntries.StreamEntry se = new StreamEntries.StreamEntry();
                    if (jsonMessage["type"].ToObject<int>() == 1)
                        se = new TroopRequestStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 2)
                        se = new ChatStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 3)
                        se = new InvitationStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 4)
                        se = new AllianceEventStreamEntry();
                    else if (jsonMessage["type"].ToObject<int>() == 5)
                        se = new ShareStreamEntry();
                    se.Load(jsonMessage);
                    m_vChatMessages.Add(se);
                }
            }
        }

        public void RemoveMember(long avatarId)
        {
            lock (_sync)
            {
                m_vAllianceMembers.Remove(avatarId);
            }
        }

        public string SaveToJson()
        {
            var jsonData = new JObject();
            jsonData.Add("alliance_id", m_vAllianceId);
            jsonData.Add("alliance_name", m_vAllianceName);
            jsonData.Add("alliance_badge", m_vAllianceBadgeData);
            jsonData.Add("alliance_type", m_vAllianceType);
            jsonData.Add("score", m_vScore);
            jsonData.Add("required_score", m_vRequiredScore);
            jsonData.Add("description", m_vAllianceDescription);
            jsonData.Add("alliance_experience", m_vAllianceExperience);
            jsonData.Add("alliance_level", m_vAllianceLevel);
            jsonData.Add("war_log_public", m_vWarLogPublic);
            jsonData.Add("friendly_war", m_vFriendlyWar);
            jsonData.Add("won_wars", m_vWonWars);
            jsonData.Add("lost_wars", m_vLostWars);
            jsonData.Add("draw_wars", m_vDrawWars);
            jsonData.Add("war_frequency", m_vWarFrequency);
            jsonData.Add("alliance_origin", m_vAllianceOrigin);

            var jsonMembersArray = new JArray();
            foreach (AllianceMemberEntry member in m_vAllianceMembers.Values)
            {
                var jsonObject = new JObject();
                member.Save(jsonObject);
                jsonMembersArray.Add(jsonObject);
            }
            jsonData.Add("members", jsonMembersArray);

            var jsonMessageArray = new JArray();
            foreach (StreamEntries.StreamEntry message in m_vChatMessages)
            {
                var jsonObject = new JObject();
                message.Save(jsonObject);
                jsonMessageArray.Add(jsonObject);
            }
            jsonData.Add("chatMessages", jsonMessageArray);
            return JsonConvert.SerializeObject(jsonData);
        }

        public void SetWarPublicStatus(byte log)
        {
            m_vWarLogPublic = log;
        }

        public void SetWarAndFriendlytStatus(byte status)
        {
            if ((int)status == 1)
                SetWarPublicStatus((byte)1);
            else if ((int)status == 2)
                FriendlyWar = (byte)1;
            else if ((int)status == 3)
            {
                SetWarPublicStatus((byte)1);
                FriendlyWar = (byte)1;
            }
            else
            {
                if ((int)status != 0)
                    return;
                SetWarPublicStatus((byte)0);
                FriendlyWar = (byte)0;
            }
        }
    }
}
