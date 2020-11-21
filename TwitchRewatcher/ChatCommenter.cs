using Newtonsoft.Json;

namespace TwitchRewatcher
{
    public class ChatCommenter
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
