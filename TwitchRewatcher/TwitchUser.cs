using Newtonsoft.Json;
namespace TwitchRewatcher {
    public class TwitchUser {
        [JsonProperty( "login" )]
        public string Login { get; set; }
    }
}
