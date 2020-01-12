using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TwitchRewatcher {
    public static class BTTVEmoticonLoader {
        public delegate void BTTVEmoticonEventHandler ();
        public static event BTTVEmoticonEventHandler OnOfficialEmoticonsLoaded;

        private const string URL = "https://api.betterttv.net/2/emotes";

        [JsonProperty( "emotes" )]
        public static BTTVEmoticonCollection EmoticonCollection { get; private set; }

        public static void LoadOfficialEmoticons () {
            using ( HttpClient client = new HttpClient () ) {
                client.BaseAddress = new Uri ( URL );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;

                if ( !response.IsSuccessStatusCode )
                    return;

                string json = response.Content.ReadAsStringAsync ().Result;
                EmoticonCollection = JsonConvert.DeserializeObject<BTTVEmoticonCollection> ( json );
                OnOfficialEmoticonsLoaded?. Invoke ();
            }
        }
    }
}
