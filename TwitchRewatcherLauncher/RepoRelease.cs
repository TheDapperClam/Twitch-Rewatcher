using System;
using Newtonsoft.Json;

namespace TwitchRewatcherLauncher {
    public class RepoRelease {
        [JsonProperty("published_at")]
        public DateTime PublishTime { get; set; }
    }
}
