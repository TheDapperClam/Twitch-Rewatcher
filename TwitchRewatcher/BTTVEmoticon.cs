using Newtonsoft.Json;

namespace TwitchRewatcher {
    public class BTTVEmoticon {
        private const string URL_TEMPLATE = "https://cdn.betterttv.net/emote/{0}/1x";

        [JsonProperty( "id" )]
        public string ID { get; set; }
        [JsonProperty( "code" )]
        public string Code { get; set; }

        public string GetImage () {
            return string.Format ( URL_TEMPLATE, ID );
        }
    }
}
