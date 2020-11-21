namespace TwitchRewatcher
{
    public class VodDownloadEntry
    {
        public string URL { get; private set; }
        public string Name { get; private set; }
        public string Destination { get; private set; }

        public VodDownloadEntry ( string url, string name, string destination = null ) {
            URL = url;
            Name = name;
            Destination = destination;
        }
    }
}
