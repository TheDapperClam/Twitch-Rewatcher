using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitchRewatcher {
    public struct TwitchBadgeSetCollection {
        [JsonProperty("badge_sets")]
        public Dictionary<string, TwitchBadgeSet> BadgeSets { get; set; }

        public bool DoesSetHaveBadge ( string set, string version ) {
            if ( !BadgeSets.ContainsKey ( set ) )
                return false;
            if ( !BadgeSets[ set ].Versions.ContainsKey ( version ) )
                return false;
            return true;
        }
    }
}
