using Newtonsoft.Json;

namespace TwitchRewatcher {
    public class FFZSet {
        [JsonProperty ( "emoticons" )]
        public FFZEmoticon[] Emoticons;
    }
}
