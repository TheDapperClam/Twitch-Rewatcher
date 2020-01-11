using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TwitchRewatcher
{
    public class ChatMessage
    {
        [JsonProperty( "body" )]
        public string Body { get; set; }
        [JsonProperty ( "user_color" )]
        public string Color { get; set; }
        [JsonProperty ( "emoticons" )]
        public Emoticon[] Emoticons { get; set; }

        [OnDeserialized]
        private void OnDeserialized ( StreamingContext context ) {
            int offset = 0;

            if ( Emoticons == null )
                return;

            foreach ( Emoticon emote in Emoticons ) {
                string image = string.Format ( "<img class='emote' src='{0}' />", emote.GetImage () );
                int length = emote.End + 1 - emote.Begin;
                Body = Body.Remove ( emote.Begin + offset, length );
                Body = Body.Insert ( emote.Begin + offset, image );
                offset += image.Length - length;
            }
        }
    }
}
