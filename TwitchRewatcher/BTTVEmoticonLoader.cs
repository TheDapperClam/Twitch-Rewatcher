using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TwitchRewatcher {
    public static class BTTVEmoticonLoader {
        private const string OFFICIAL_EMOTE_URL = "https://api.betterttv.net/3/cached/emotes/global";
        private const string CHANNEL_EMOTE_URL = "https://api.betterttv.net/3/cached/users/twitch/{0}";

        public delegate void BTTVEmoticonEventHandler ( BTTVEmoticon[] emoticons );
        public delegate void BTTVCollectionEventHandler ( BTTVEmoticonCollection collection );
        public static event BTTVEmoticonEventHandler OnOfficialEmoticonsLoaded;
        public static event BTTVCollectionEventHandler OnChannelEmoticonsLoaded;

        public static BTTVEmoticon[] OfficialEmoticons{ get; private set; }
        public static BTTVEmoticonCollection ChannelEmoticons { get; private set; }

        public static void LoadChannelEmoticons ( string channel ) {
            if ( channel == null )
                return;

            using ( HttpClient client = new HttpClient () ) {
                client.BaseAddress = new Uri ( string.Format ( CHANNEL_EMOTE_URL, channel ) );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;
                string json = response.Content.ReadAsStringAsync ().Result;
                ChannelEmoticons = JsonConvert.DeserializeObject<BTTVEmoticonCollection> ( json );
                OnChannelEmoticonsLoaded?.Invoke ( ChannelEmoticons );
            }
        }

        public static void LoadOfficialEmoticons () {
            using ( HttpClient client = new HttpClient () ) {
                client.BaseAddress = new Uri ( OFFICIAL_EMOTE_URL );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;
                string json = response.Content.ReadAsStringAsync ().Result;
                OfficialEmoticons = JsonConvert.DeserializeObject<BTTVEmoticon[]> ( json );
                OnOfficialEmoticonsLoaded?. Invoke ( OfficialEmoticons );
            }
        }
    }
}
