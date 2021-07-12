using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitchRewatcher {
    public struct TwitchBadgeSet {
        [JsonProperty("versions")]
        public Dictionary<string, TwitchBadge> Versions { get; set; }
    }
}
