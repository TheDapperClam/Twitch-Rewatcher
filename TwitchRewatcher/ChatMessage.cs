using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TwitchRewatcher
{
    public class ChatMessage
    {
        private const string IMAGE_TAG_TEMPLATE = "<img class='emote' src='{0}' />";

        [JsonProperty( "body" )]
        public string Body { get; set; }
        [JsonProperty ( "user_color" )]
        public string Color { get; set; }
        [JsonProperty ( "emoticons" )]
        public TwitchEmoticon[] Emoticons { get; set; }

        [OnDeserialized]
        private void OnDeserialized ( StreamingContext context ) {
            if ( Emoticons != null ) {
                int offset = 0;
                foreach ( TwitchEmoticon emote in Emoticons ) {
                    string image = string.Format ( IMAGE_TAG_TEMPLATE, emote.GetImage () );
                    int length = emote.End + 1 - emote.Begin;
                    Body = Body.Remove ( emote.Begin + offset, length );
                    Body = Body.Insert ( emote.Begin + offset, image );
                    offset += image.Length - length;
                }
            }

            foreach ( BTTVEmoticon emote in BTTVEmoticonLoader.EmoticonCollection.Emoticons ) {
                string image = string.Format ( IMAGE_TAG_TEMPLATE, emote.GetImage () );
                Body = Body.Replace ( emote.Code, image );
            }
        }
    }
}
