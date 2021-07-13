using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization;

namespace TwitchRewatcher {
    public class ChatMessage
    {
        private const string TAG_EMOTE = "<img class='emote' src='{0}' />";
        private const string TAG_BADGE = "<img class='badge' src={0} />";

        [JsonProperty( "body" )]
        public string Body { get; set; }
        [JsonProperty ( "user_color" )]
        public string Color { get; set; }
        [JsonProperty ( "emoticons" )]
        public TwitchEmoticon[] Emoticons { get; set; }
        [JsonProperty("user_badges")]
        public ChatUserBadge[] UserBadges { get; set; }

        public ChatMessage () {
            TwitchBadgeLoader.OnChannelBadgesLoaded += OnChannelBadgesLoaded;
            BTTVEmoticonLoader.OnChannelEmoticonsLoaded += OnBTTVChannelEmoticonsLoaded;
            FFZEmoticonLoader.OnSetsLoaded += OnFFZSetsLoaded;
        }

        private void OnBTTVChannelEmoticonsLoaded ( BTTVEmoticonCollection collection ) {
            if ( collection == null )
                return;

            if ( collection.ChannelEmoticons != null ) {
                foreach ( BTTVEmoticon emote in collection.ChannelEmoticons ) {
                    string image = string.Format ( TAG_EMOTE, emote.GetImage () );
                    Body = Body.Replace ( emote.Code, image );
                }
            }

            if ( collection.SharedEmoticons != null ) {
                foreach ( BTTVEmoticon emote in collection.SharedEmoticons ) {
                    string image = string.Format ( TAG_EMOTE, emote.GetImage () );
                    Body = Body.Replace ( emote.Code, image );
                }
            }
        }

        private void OnChannelBadgesLoaded () {
            string badges = "";
            if ( UserBadges != null ) {
                foreach ( ChatUserBadge badge in UserBadges ) {
                    if ( TwitchBadgeLoader.ChannelBadgeSets.BadgeSets != null && TwitchBadgeLoader.ChannelBadgeSets.DoesSetHaveBadge ( badge.ID, badge.Version ) ) {
                        string image = string.Format ( TAG_BADGE, TwitchBadgeLoader.ChannelBadgeSets.BadgeSets[ badge.ID ].Versions[ badge.Version ].Image );
                        badges += image;
                    } else if ( TwitchBadgeLoader.GlobalBadgeSets.BadgeSets != null ) {
                        string image = string.Format ( TAG_BADGE, TwitchBadgeLoader.GlobalBadgeSets.BadgeSets[ badge.ID ].Versions[ badge.Version ].Image );
                        badges += image;
                    }
                }
            }
            Body = Body.Replace ( ChatObject.BADGES_SPAN_INDEX, badges );
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

                    string image = string.Format ( TAG_EMOTE, emote.URLs.Values.ElementAt ( 0 ) );
                    Body = Body.Replace ( emote.Name, image );
                }
            }
        }

        [OnDeserialized]
        private void OnDeserialized ( StreamingContext context ) {
            if ( Emoticons != null ) {
                string oldBody = Body;
                int offset = 0;
                try {
                    foreach ( TwitchEmoticon emote in Emoticons ) {
                        string image = string.Format ( TAG_EMOTE, emote.GetImage () );
                        int length = emote.End - emote.Begin + 1;
                        Body = Body.Remove ( emote.Begin + offset, length );
                        Body = Body.Insert ( emote.Begin + offset, image );
                        offset += image.Length - length;
                    }
                } catch {
                    Body = oldBody;
                }
            }

            if ( BTTVEmoticonLoader.OfficialEmoticons != null ) {
                foreach ( BTTVEmoticon emote in BTTVEmoticonLoader.OfficialEmoticons ) {
                    string image = string.Format ( TAG_EMOTE, emote.GetImage () );
                    Body = Body.Replace ( emote.Code, image );
                }
            }
        }
    }
}
