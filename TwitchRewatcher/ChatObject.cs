using Newtonsoft.Json;

namespace TwitchRewatcher
{
    public class ChatObject
    {
        [JsonProperty ( "commenter" )]
        public ChatCommenter Commenter { get; set; }
        [JsonProperty("message")]
        public ChatMessage Message { get; set; }
        [JsonProperty("content_offset_seconds")]
        public double PostTime { get; set; }
    }
}
