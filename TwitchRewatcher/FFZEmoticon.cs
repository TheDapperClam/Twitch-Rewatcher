using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TwitchRewatcher {
    public class FFZEmoticon {
        [JsonProperty( "name" )]
        public string Name { get; set; }
        [JsonProperty ( "urls" )]
        public Dictionary<string, string> URLs { get; set; } = new Dictionary<string, string> ();

        [OnDeserialized]
        private void OnDeserialized ( StreamingContext context ) {
            string[] keys = URLs.Keys.ToArray ();

            foreach ( string key in keys )
                URLs[ key ] = URLs[ key ].Replace ( "//cdn", "https://cdn" );
        }
    }
}
