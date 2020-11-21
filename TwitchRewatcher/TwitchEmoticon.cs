using Newtonsoft.Json;

namespace TwitchRewatcher {
    public class TwitchEmoticon {
        private const string URL_TEMPLATE = "https://static-cdn.jtvnw.net/emoticons/v1/{0}/1.0";

        [JsonProperty ( "_id" )]
        public string ID { get; set; }
        [JsonProperty ( "begin" )]
        public int Begin { get; set; }
        [JsonProperty ( "end" )]
        public int End { get; set; }

        public string GetImage () {
            return string.Format ( URL_TEMPLATE, ID );
        }
    }
}
