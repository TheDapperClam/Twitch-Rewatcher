using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TwitchRewatcher
{
    public static class ChatLoader
    {
        public static TwitchUser CurrentChannel { get; private set; }

        public static ChatObject[] Load ( string path ) {
            if ( !File.Exists ( path ) )
                return null;

            string json = File.ReadAllText ( path );
            ChatObject[] chatObjs = JsonConvert.DeserializeObject<List<ChatObject>> ( json ).ToArray ();

            if ( chatObjs != null && chatObjs.Length > 0 ) {
                CurrentChannel = TwitchAPI.GetUser ( chatObjs[ 0 ].ChannelID );
                BTTVEmoticonLoader.LoadChannelEmoticons ( CurrentChannel );
            }

            return chatObjs;
        }
    }
}
