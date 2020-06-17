using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TwitchRewatcher {
    public static class ChatLoader
    {
        public static ChatObject[] Load ( string path ) {
            if ( !File.Exists ( path ) )
                return null;

            string json = File.ReadAllText ( path );
            ChatObject[] chatObjs = JsonConvert.DeserializeObject<List<ChatObject>> ( json ).ToArray ();

            if ( chatObjs != null && chatObjs.Length > 0 )
                BTTVEmoticonLoader.LoadChannelEmoticons ( chatObjs[ 0 ].ChannelID );

            return chatObjs;
        }
    }
}
