using Newtonsoft.Json;
using System.IO;

namespace TwitchRewatcher {
    public static class StreamConfigManager
    {
        public static void SaveConfig ( string path, StreamConfig config ) {
            if ( config == null )
                return;

            string file = Path.Combine ( path, TwitchRewatcherForm.CONFIG_FILE_NAME );

            if ( File.Exists ( file ) )
                File.WriteAllText ( file, string.Empty );

            using ( FileStream stream = File.Open ( file, FileMode.OpenOrCreate ) ) {
                using ( StreamWriter writer = new StreamWriter ( stream ) ) {
                    JsonSerializer serializer = new JsonSerializer ();
                    serializer.Serialize ( writer, config );
                }
            }
        }

        public static StreamConfig LoadConfig ( string path ) {
            if ( !File.Exists ( path ) )
                return null;

            StreamConfig cfg = JsonConvert.DeserializeObject<StreamConfig> ( File.ReadAllText ( path ) );
            return cfg;
        }
    }
}
