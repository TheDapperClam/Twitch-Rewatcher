using System.IO;
using Newtonsoft.Json;

namespace TwitchRewatcher
{
    public static class StreamConfigManager
    {
        public static void SaveConfig ( string path, StreamConfig config ) {
            if ( config == null )
                return;

            string configPath = Path.Combine ( path, "StreamConfig.dtj" );
            using ( StreamWriter stream = new StreamWriter ( configPath ) ) {
                using ( JsonWriter writer = new JsonTextWriter ( stream ) ) {
                    JsonSerializer serializer = new JsonSerializer ();
                    serializer.Serialize ( stream, config );
                }
            }
        }

        public static StreamConfig LoadConfig ( string path ) {
            StreamConfig cfg = JsonConvert.DeserializeObject<StreamConfig> ( File.ReadAllText ( path ) );
            return cfg;
        }
    }
}
