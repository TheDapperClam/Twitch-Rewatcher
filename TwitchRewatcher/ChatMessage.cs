using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;

namespace TwitchRewatcher {
    public class ChatMessage
    {
        private const string IMAGE_TAG_TEMPLATE = "<img class='emote' src='{0}' />";

        [JsonProperty( "body" )]
        public string Body { get; set; }
        [JsonProperty ( "user_color" )]
        public string Color { get; set; }
        [JsonProperty ( "emoticons" )]
        public TwitchEmoticon[] Emoticons { get; set; }

        public ChatMessage () {
            BTTVEmoticonLoader.OnChannelEmoticonsLoaded += new BTTVEmoticonLoader.BTTVCollectionEventHandler ( OnBTTVChannelEmoticonsLoaded );
            FFZEmoticonLoader.OnSetsLoaded += new FFZEmoticonLoader.FFZSetEventHandler ( OnFFZSetsLoaded );
        }

        private void OnBTTVChannelEmoticonsLoaded ( BTTVEmoticonCollection collection ) {
            if ( collection == null )
                return;

            if ( collection.ChannelEmoticons != null ) {
                foreach ( BTTVEmoticon emote in collection.ChannelEmoticons ) {
                    string image = string.Format ( IMAGE_TAG_TEMPLATE, emote.GetImage () );
                    Body = Body.Replace ( emote.Code, image );
                }
            }

            if ( collection.SharedEmoticons != null ) {
                foreach ( BTTVEmoticon emote in collection.SharedEmoticons ) {
                    string image = string.Format ( IMAGE_TAG_TEMPLATE, emote.GetImage () );
                    Body = Body.Replace ( emote.Code, image );
                }
            }
        }

        private void OnFFZSetsLoaded ( FFZSet[] sets ) {
            if ( sets == null )
                return;

            foreach ( FFZSet set in sets ) {
                if ( set.Emoticons == null )
                    continue;

                if ( set.Emoticons.Length <= 0 )
                    continue;

                foreach ( FFZEmoticon emote in set.Emoticons ) {
                    if ( emote.URLs == null )
                        continue;

                    if ( emote.URLs.Count <= 0 )
                        continue;

                    string image = string.Format ( IMAGE_TAG_TEMPLATE, emote.URLs.Values.ElementAt ( 0 ) );
                    Body = Body.Replace ( emote.Name, image );
                }
            }
        }

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

            if ( BTTVEmoticonLoader.OfficialEmoticons != null ) {
                foreach ( BTTVEmoticon emote in BTTVEmoticonLoader.OfficialEmoticons ) {
                    string image = string.Format ( IMAGE_TAG_TEMPLATE, emote.GetImage () );
                    Body = Body.Replace ( emote.Code, image );
                }
            }
        }
    }
}
