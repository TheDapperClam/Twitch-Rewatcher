using Newtonsoft.Json;

namespace TwitchRewatcher {
    public struct ChatUserBadge {
        [JsonProperty("_id")]
        public string ID { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
