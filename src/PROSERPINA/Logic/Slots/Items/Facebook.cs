using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Newtonsoft.Json;

namespace BL.Servers.CR.Logic.Slots.Items
{
    internal class Facebook
    {
        internal const string ApplicationID = "1609113955765603";
        internal const string ApplicationSecret = "708a87ee580fe571de7c9878bfb2dc96";
        internal const string ApplicationVersion = "2.9";

        [JsonProperty("fb_id")] internal string Identifier;
        [JsonProperty("fb_token")] internal string Token;

        internal FacebookClient FBClient;
        internal Avatar Avatar;

        public Facebook()
        {
        }

        public Facebook(Avatar Avatar)
        {
            this.Avatar = Avatar;

            if (this.Filled)
            {
                this.Connect();
            }
        }

        internal bool Connected
        {
            get
            {
                return this.Filled && this.FBClient != null;
            }
        }

        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);
            }
        }

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
            else
            {
                return null;
            }
        }
    }
}
