using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TwitchRewatcherLauncher {
    class Program {
        private const string TOKEN = "65ebf7c6530e97e95616e353937bb28cf002af46";
        private const string RELEASE_URL = "https://api.github.com/repos/jrguenther/twitch-rewatcher/releases";

        static void Main ( string[] args ) {
            try {
                Console.WriteLine ( "Checking for updates" );
                using ( HttpClient client = new HttpClient () ) {
                    client.BaseAddress = new Uri ( RELEASE_URL );
                    client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                    client.DefaultRequestHeaders.UserAgent.Add ( new ProductInfoHeaderValue ( "twitch-rewatcher", "1.0" ) );
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ( "Token", TOKEN );
                    HttpResponseMessage response = client.GetAsync ( "" ).Result;
                    string json = response.Content.ReadAsStringAsync ().Result;
                    RepoRelease latestRelease = JsonConvert.DeserializeObject<RepoRelease[]> ( json )[ 0 ];
                    Console.WriteLine ( "Latest release time: " + latestRelease.PublishTime );
                }
            }
        }
    }
}
