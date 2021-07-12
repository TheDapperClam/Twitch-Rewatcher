using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TwitchRewatcher {
    public static class TwitchBadgeLoader {
        private const string BADGES_GLOBAL_URL = "https://badges.twitch.tv/v1/badges/global/display";
        private const string BADGES_CHANNEL_URL = "https://badges.twitch.tv/v1/badges/channels/{0}/display";

        public static TwitchBadgeSetCollection GlobalBadgeSets { get; set; }
        public static TwitchBadgeSetCollection ChannelBadgeSets { get; set; }

        public delegate void TwitchBadgeLoaderEventHandler ();
        public static event TwitchBadgeLoaderEventHandler OnGlobalBadgesLoaded;
        public static event TwitchBadgeLoaderEventHandler OnChannelBadgesLoaded;

        public static void LoadChannelBadges ( string channel ) {
            if ( channel == null )
                return;

            try {
                using ( HttpClient client = new HttpClient () ) {
                    client.BaseAddress = new Uri ( string.Format ( BADGES_CHANNEL_URL, channel ) );
                    client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                    HttpResponseMessage response = client.GetAsync ( "" ).Result;
                    string json = response.Content.ReadAsStringAsync ().Result;
                    ChannelBadgeSets = JsonConvert.DeserializeObject<TwitchBadgeSetCollection> ( json );
                }
            } catch {
                ChannelBadgeSets = default;
            } finally {
                OnChannelBadgesLoaded?.Invoke ();
            }
        }

        public static void LoadGlobalBadges () {
            try {
                using ( HttpClient client = new HttpClient () ) {
                    client.BaseAddress = new Uri ( BADGES_GLOBAL_URL );
                    client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                    HttpResponseMessage response = client.GetAsync ( "" ).Result;
                    string json = response.Content.ReadAsStringAsync ().Result;
                    GlobalBadgeSets = JsonConvert.DeserializeObject<TwitchBadgeSetCollection> ( json );
                }
            } catch {
                GlobalBadgeSets = default;
            } finally {
                OnGlobalBadgesLoaded?.Invoke ();
            }
        }
    }
}
