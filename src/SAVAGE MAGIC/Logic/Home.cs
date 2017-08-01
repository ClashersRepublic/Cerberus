using System.Collections.Generic;
using System.IO;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic
{
    internal class Home : BaseHome
    {
        private const string EventsJson = "{\"events\":[]}";

        private readonly long _homeId;
        private int _shieldTime;
        private string _homeJson;

        public Home(long homeId)
        {
            _homeId = homeId;
        }

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt64(_homeId);
            data.AddInt32(_shieldTime);
            data.AddInt32(1800);
            data.AddInt32(0);
            data.Add(1);
            data.AddCompressedString(_homeJson);
            data.Add(1);
            data.AddCompressedString(EventsJson);

            return data.ToArray();
        }

        public void SetHomeJson(string json)
        {
            _homeJson = json;
        }

        public void SetShieldTime(int seconds)
        {
            _shieldTime = seconds;
        }
    }
}
