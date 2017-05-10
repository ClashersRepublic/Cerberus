using System;
using System.IO;
using System.Text;
using System.Threading;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Extensions.List;
using global::Google.Apis.Games.v1;
using global::Google.Apis.Auth.OAuth2;
using global::Google.Apis.Services;
using global::Google.Apis.Util.Store;

namespace BL.Servers.CoC
{ 
    internal class Test
    {

        internal UserCredential OCredentials;
        internal GamesService OClient;
        internal Test()
        {
            using (var Stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                this.OCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(Stream).Secrets, new[]
                {   
                    GamesService.Scope.Games, GamesService.Scope.PlusLogin
                }, "com.barbarianland.galileo", CancellationToken.None, new FileDataStore("Galileo.Server")).Result;
            }

            this.OClient = new GamesService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this.OCredentials,
                ApplicationName = "Galileo",
                ApiKey = "AIzaSyBGTLGFBahC_wDIzFuHLGiRHxIEoHpC0UQ"
            });
            var ScoreSubmit = this.OClient.Achievements.Unlock("CgkIkMbTi4wBEAIQAQ");
            var ScoreResult = ScoreSubmit.Execute();
            Console.WriteLine(ScoreResult.ETag);
        }
        internal void Uncompress(string Hexa)
        {
            Reader br = new Reader(Hexa.HexaToBytes());
            var data =  br.ReadFully();

            Console.WriteLine(BitConverter.ToString(data).Replace("-", ""));
            Console.WriteLine(Encoding.UTF8.GetString(data));
        
        }
    }
}