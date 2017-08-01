using System.Collections.Generic;
using System.Configuration;
using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    // Packet 20103
    internal class LoginFailedMessage : Message
    {
        public LoginFailedMessage(ClashOfClans.Client client) : base(client)
        {
            MessageType = 20103;
            SetUpdateURL(ConfigurationManager.AppSettings["UpdateUrl"]);
            MessageVersion = 2;

            // 8  : Update
            // 10 : Maintenance
            // 11 : Banned
            // 12 : Timeout
            // 13 : Locked Account
        }

        string m_vContentURL;
        int m_vErrorCode;
        string m_vReason;
        string m_vRedirectDomain;
        int m_vRemainingTime;
        string m_vResourceFingerprintData;
        string m_vUpdateURL;

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(m_vErrorCode);

            pack.AddString(this.m_vErrorCode == 7 ? m_vResourceFingerprintData : null);
            pack.AddString(m_vRedirectDomain);
            pack.AddString(m_vContentURL);
            pack.AddString(m_vUpdateURL);
            pack.AddString(m_vReason);

            pack.AddInt32(0);

            pack.AddString(null); // Compressed json

            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddInt32(0);

            if (Client.State == ClashOfClans.Client.ClientState.Login)
                Encrypt(pack.ToArray());
            else
                Data = pack.ToArray();
        } 

        public void RemainingTime(int code) => m_vRemainingTime = code;

        public void SetContentURL(string url) => m_vContentURL = url;

        public void SetErrorCode(int code) => m_vErrorCode = code;

        public void SetReason(string reason) => m_vReason = reason;

        public void SetRedirectDomain(string domain) => m_vRedirectDomain = domain;

        public void SetResourceFingerprintData(string data) => m_vResourceFingerprintData = data;

        public void SetUpdateURL(string url) => m_vUpdateURL = url;
    }
}
