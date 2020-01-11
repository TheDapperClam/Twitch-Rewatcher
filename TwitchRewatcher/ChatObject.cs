using Newtonsoft.Json;
using System.Runtime.Serialization;

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

        public string GetFormattedMessage () {
            return string.Format ( "<li><span style='color:{0}'><b>{1}</b></span>: {2}</li>", Message.Color, Commenter.DisplayName, Message.Body );
        }
    }
}
