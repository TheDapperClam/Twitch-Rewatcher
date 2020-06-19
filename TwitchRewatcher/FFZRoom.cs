using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitchRewatcher {
    public class FFZRoom {
        [JsonProperty ( "sets" )]
        public Dictionary<string, FFZSet> Sets { get; set; } = new Dictionary<string, FFZSet> ();
    }
}
