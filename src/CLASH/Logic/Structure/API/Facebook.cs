using Facebook;
using Newtonsoft.Json;

namespace BL.Servers.CoC.Logic.Structure.API
{
    internal class Facebook
    {
        internal const string ApplicationID = "623623897848356";
        internal const string ApplicationSecret = "22dfd5e1675c0af73c7a1780513827ee";
        internal const string ApplicationVersion = "2.8";

        [JsonProperty("fb_id")] internal string Identifier;
        [JsonProperty("fb_token")] internal string Token;

        internal FacebookClient FBClient;
        internal Player Player;

        internal Facebook()
        {
            // Facebook.
        }

        internal Facebook(Player Player)
        {
            this.Player = Player;

            if (this.Filled)
            {
                this.Connect();
            }
        }
        internal bool Connected => this.Filled && this.FBClient != null;

        internal bool Filled => !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);

        internal void Connect()
        {
            this.FBClient = new FacebookClient(this.Token)
            {
                AppId = Facebook.ApplicationID,
                AppSecret = Facebook.ApplicationSecret,
                Version = Facebook.ApplicationVersion
            };
        }

        internal object Get(string Path, bool IncludeIdentifier = true)
        {
            if (this.Connected)
            {
                return this.FBClient.Get("https://graph.facebook.com/v" + Facebook.ApplicationVersion + "/" + (IncludeIdentifier ? this.Identifier + '/' + Path : Path));
            }
            return null;
        }
    }
}
