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
        public static ChatObject[] Load ( string path ) {
            if ( !File.Exists ( path ) )
                return null;

            return JsonConvert.DeserializeObject<List<ChatObject>> ( File.ReadAllText ( path ) ).ToArray ();
        }
    }
}
