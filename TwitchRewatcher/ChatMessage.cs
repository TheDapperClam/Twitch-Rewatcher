using Newtonsoft.Json;

namespace TwitchRewatcher
{
    public class ChatMessage
    {
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty ( "user_color" )]
        public string Color { get; set; }
    }
}
