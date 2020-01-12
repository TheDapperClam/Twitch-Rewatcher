using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TwitchRewatcher {
    public static class TwitchAPI {
        private const string CLIENT_ID = "hjwjzbz8jo6d1pszyy16ezcpj3b91q";

        public static TwitchUser GetUser ( string clientId ) {
            using ( HttpClient client = new HttpClient () ) {
                string url = "https://api.twitch.tv/helix/users?id={0}";
                client.BaseAddress = new Uri ( string.Format ( url, clientId ) );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                client.DefaultRequestHeaders.Add ( "Client-ID", CLIENT_ID );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;

                if ( !response.IsSuccessStatusCode )
                    return null;

                string json = response.Content.ReadAsStringAsync ().Result;
                TwitchUser user = JsonConvert.DeserializeObject<TwitchUserData> ( json ).Users[ 0 ];
                return user;
            }
        }
    }
}
