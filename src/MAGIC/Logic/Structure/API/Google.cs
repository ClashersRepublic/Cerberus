using System.IO;
using System.Threading;
using Newtonsoft.Json;

using global::Google.Apis.Games.v1;
using global::Google.Apis.Auth.OAuth2;
using global::Google.Apis.Services;
using global::Google.Apis.Util.Store;

namespace BL.Servers.CoC.Logic.Structure.API
{
    internal class Google
    {
        internal const string GlobalPlayersID = "";
        internal const string GlobalClansID = "";
        internal const string LocalPlayersID = "";
        internal const string LocalClansID = "";

        [JsonProperty("gg_id")] internal string Identifier;
        [JsonProperty("gg_token")] internal string Token;

        internal UserCredential OCredentials;
        internal GamesService OClient;
        internal Player Player;

        internal Google()
        {
            // Google.
        }

        internal Google(Player Player)
        {
            this.Player = Player;

            if (this.Filled)
            {
                this.Connect();
            }
        }

        internal bool Filled => !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);

        internal void GetCredentials()
        {
            /*using (var Stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                this.OCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(Stream).Secrets, new[]
                {
                    GamesService.Scope.Games, GamesService.Scope.PlusLogin
                }, "", CancellationToken.None, new FileDataStore("")).Result;
            }*/
        }

        /// <summary>
        /// Logs into the YouTube API's Servers.
        /// </summary>
        internal void Login()
        {
            this.OClient = new GamesService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this.OCredentials,
                ApplicationName = "",
                ApiKey = ""
            });
        }

        internal void Connect()
        {
            this.GetCredentials();
            this.Login();

            /* var ScoreSubmit = this.OClient.Scores.Submit(Google.GlobalPlayersID, this.Player.Trophies);
            var ScoreResult = ScoreSubmit.Execute(); */
        }
    }
}
