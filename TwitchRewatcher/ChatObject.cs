﻿using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace TwitchRewatcher {
    public class ChatObject
    {
        public const string BADGES_SPAN_INDEX = "[BADGES]";

        private const string BODY_TEMPLATE = "<li><span class='timeStamp'>{0}</span><div class='messageBody'><span>{1}</span><span style='color:{2}'><b>{3}</b></span>: {4}</div></li>";

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
            Message.Body = string.Format ( BODY_TEMPLATE, timeStamp, BADGES_SPAN_INDEX, Message.Color, Commenter.DisplayName, Message.Body );
        }
    }
}
