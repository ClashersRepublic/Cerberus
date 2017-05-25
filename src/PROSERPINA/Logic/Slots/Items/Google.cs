using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Google.Apis.Games.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace BL.Servers.CR.Logic.Slots.Items
{
    internal class Google
    {
        [JsonProperty("gg_id")] internal string Identifier;
        [JsonProperty("gg_token")] internal string Token;

        internal UserCredential OCredentials;
        internal GamesService OClient;
        internal Player Player;

        public Google()
        {
            
        }

        public Google(Player Avatar)
        {
            this.Player = Player;

            if (this.Filled)
            {
                this.Connect();
            }
        }

        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);
            }
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        internal void GetCredentials()
        {
            using (var Stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                this.OCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(Stream).Secrets, new[]
                {
                    GamesService.Scope.Games, GamesService.Scope.PlusLogin
                }, "Proserpina", CancellationToken.None, new FileDataStore("Proserpina")).Result;
            }
        }

        /// <summary>
        /// Logs into the YouTube API's Servers.
        /// </summary>
        internal void Login()
        {
            this.OClient = new GamesService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this.OCredentials,
                ApplicationName = "BarbarianLand",
                ApiKey = "AIzaSyCIy1Zh4i3RHw8PqYNExEwMd46FaBqFb_0"
            });
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal void Connect()
        {
            this.GetCredentials();
            this.Login();
        }
    }
}
