using Newtonsoft.Json;

namespace TwitchRewatcher {
    public class BTTVEmoticonCollection {
        [JsonProperty("emotes")]
        public BTTVEmoticon[] Emoticons { get; set; }
    }
}
