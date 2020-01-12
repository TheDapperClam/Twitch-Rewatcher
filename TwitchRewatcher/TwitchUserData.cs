using Newtonsoft.Json;

namespace TwitchRewatcher {
    public class TwitchUserData {
        [JsonProperty( "data" )]
        public TwitchUser[] Users { get; set; }
    }
}
