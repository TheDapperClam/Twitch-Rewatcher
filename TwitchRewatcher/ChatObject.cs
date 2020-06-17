using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace TwitchRewatcher {
    public class ChatObject
    {
        [JsonProperty( "channel_id" )]
        public string ChannelID { get; set; }
        [JsonProperty ( "commenter" )]
        public ChatCommenter Commenter { get; set; }
        [JsonProperty("message")]
        public ChatMessage Message { get; set; }
        [JsonProperty("content_offset_seconds")]
        public double PostTime { get; set; }

        [OnDeserialized]
        private void OnDeserialized ( StreamingContext context ) {
            TimeSpan t = TimeSpan.FromSeconds ( PostTime );
            string timeStamp = string.Format ( "{0}{1}{2}", 
                t.Hours > 0 ? t.Hours.ToString () + ":" : "",
                t.Hours > 0 && t.Minutes < 10 ? "0" + t.Minutes.ToString () + ":" : t.Minutes.ToString () + ":" ,
                t.Seconds < 10 ? "0" + t.Seconds.ToString () : t.Seconds.ToString () );
            Message.Body = string.Format ( "<li>{0} <span style='color:{1}'><b>{2}</b></span>: {3}</li>", timeStamp, Message.Color, Commenter.DisplayName, Message.Body );
        }
    }
}
