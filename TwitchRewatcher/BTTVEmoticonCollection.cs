using Newtonsoft.Json;

namespace TwitchRewatcher {
    public class BTTVEmoticonCollection {
        [JsonProperty("channelEmotes")]
        public BTTVEmoticon[] ChannelEmoticons { get; set; }
        [JsonProperty ( "sharedEmotes" )]
        public BTTVEmoticon[] SharedEmoticons { get; set; }
    }
}
