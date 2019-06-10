using Newtonsoft.Json;

namespace TwitchRewatcher
{
    public class StreamConfig
    {
        [JsonProperty("PlaybackTime")]
        public double PlaybackTime { get; set; }
    }
}
