using Newtonsoft.Json;

namespace TwitchRewatcher {
    public struct TwitchBadge {
        [JsonProperty("image_url_1x")]
        public string Image { get; set; }
    }
}
