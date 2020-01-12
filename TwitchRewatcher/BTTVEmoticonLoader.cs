using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TwitchRewatcher {
    public static class BTTVEmoticonLoader {
        public delegate void BTTVEmoticonEventHandler ( BTTVEmoticonCollection collection );
        public static event BTTVEmoticonEventHandler OnOfficialEmoticonsLoaded;
        public static event BTTVEmoticonEventHandler OnChannelEmoticonsLoaded;

        public static BTTVEmoticonCollection OfficialEmoticonCollection { get; private set; }
        public static BTTVEmoticonCollection ChannelEmoticonCollection { get; private set; }

        public static void LoadChannelEmoticons ( TwitchUser channel ) {
            if ( channel == null )
                return;

            using ( HttpClient client = new HttpClient () ) {
                string url = "https://api.betterttv.net/2/channels/{0}";
                client.BaseAddress = new Uri ( string.Format ( url, channel.Login ) );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;
                string json = response.Content.ReadAsStringAsync ().Result;
                ChannelEmoticonCollection = JsonConvert.DeserializeObject<BTTVEmoticonCollection> ( json );
                OnChannelEmoticonsLoaded?.Invoke( ChannelEmoticonCollection );
            }
        }

        public static void LoadOfficialEmoticons () {
            using ( HttpClient client = new HttpClient () ) {
                string url = "https://api.betterttv.net/2/emotes";
                client.BaseAddress = new Uri ( url );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;
                string json = response.Content.ReadAsStringAsync ().Result;
                OfficialEmoticonCollection = JsonConvert.DeserializeObject<BTTVEmoticonCollection> ( json );
                OnOfficialEmoticonsLoaded?. Invoke ( OfficialEmoticonCollection );
            }
        }
    }
}
