using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TwitchRewatcher {
    public static class FFZEmoticonLoader {
        private const string ROOM_URL = "https://api.frankerfacez.com/v1/room/id/{0}";

        public static FFZSet[] Sets { get; private set; }

        public delegate void FFZSetEventHandler ( FFZSet[] sets );
        public static event FFZSetEventHandler OnSetsLoaded;

        public static void LoadChannelSets ( string channel ) {
            if ( channel == null )
                return;

            using ( HttpClient client = new HttpClient () ) {
                client.BaseAddress = new Uri ( string.Format ( ROOM_URL, channel ) );
                client.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                HttpResponseMessage response = client.GetAsync ( "" ).Result;
                string json = response.Content.ReadAsStringAsync ().Result;
                FFZRoom room = JsonConvert.DeserializeObject<FFZRoom> ( json );
                Sets = room.Sets.Values.ToArray ();
                OnSetsLoaded?.Invoke ( Sets );
            }
        }
    }
}
