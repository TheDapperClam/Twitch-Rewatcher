using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TwitchRewatcher
{
    public static class VodDownloader
    {
        private const string YOUTUBE_DL_PATH = "Tools\\Youtube-DL\\youtube-dl.exe";
        private const string RECHAT_TOOL_PATH = "Tools\\RechatTool\\RechatTool.exe";
        private static readonly string ILLEGAL_PATH_CHARS = new string ( Path.GetInvalidFileNameChars () ) + new string ( Path.GetInvalidPathChars () );

        public static float VideoDownloadProgress { get; private set; }
        public static float ChatDownloadProgress { get; private set; }
        public static bool IsDownloading { get { return IsDownloadingVideo || IsDownloadingChat; } }
        public static bool IsDownloadingVideo { get { return youtubeDlProcess != null; } }
        public static bool IsDownloadingChat { get { return reChatToolProcess != null; } }

        private static Process youtubeDlProcess = null;
        private static Process reChatToolProcess = null;

        private static string currentDirectory;

        public delegate void VodEventHandler ();
        public static event VodEventHandler OnDownloadFinished;
        public static event VodEventHandler OnDownloadProgress;

        private static void ChatDataRecieved ( object sender, DataReceivedEventArgs e ) {
            if ( OnDownloadProgress != null )
                OnDownloadProgress ();
        }

        private static void ChatExit ( object sender, EventArgs e ) {
            Console.WriteLine ( "Chat Exit" );
            ChatDownloadProgress = 100.0f;
            reChatToolProcess.Dispose ();
            reChatToolProcess = null;
            CheckDownloadFinished ();

            if ( OnDownloadProgress != null )
                OnDownloadProgress ();
        }

        private static void CheckDownloadFinished () {
            if ( IsDownloading )
                return;

            VideoDownloadProgress = 0.0f;
            ChatDownloadProgress = 0.0f;

            if ( OnDownloadFinished != null )
                OnDownloadFinished ();
        }

        public static void Download ( string title, string source, string output = null ) {
            string id = Regex.Match ( source, @"videos/([0-9]+?)\b" ).Groups[ 1 ].Value;
            string o = Path.Combine ( output, title + "\\" );

            foreach ( char c in ILLEGAL_PATH_CHARS )
                o.Replace ( c.ToString (), "" );

            currentDirectory = o;
            VideoDownloadProgress = 0.0f;
            ChatDownloadProgress = 0.0f;

            if ( !Directory.Exists ( o ) )
                Directory.CreateDirectory ( o );

            ProcessStartInfo youtubeDlInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, YOUTUBE_DL_PATH ) );
            ProcessStartInfo rechatToolInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, RECHAT_TOOL_PATH ) );
            youtubeDlInfo.UseShellExecute = false;
            rechatToolInfo.UseShellExecute = false;
            youtubeDlInfo.RedirectStandardOutput = true;
            rechatToolInfo.RedirectStandardOutput = true;
            youtubeDlInfo.CreateNoWindow = true;
            rechatToolInfo.CreateNoWindow = true;
            rechatToolInfo.WindowStyle = ProcessWindowStyle.Hidden;
            youtubeDlInfo.Arguments = string.Format ( "--newline -o \"{0}\\{1}.%(ext)s\" \"{2}\"", o, title, source );
            rechatToolInfo.Arguments = string.Format ( "-d {0} \"{1}\"", id, o + title + ".dtc" );

            youtubeDlProcess = Process.Start ( youtubeDlInfo );
            reChatToolProcess = Process.Start ( rechatToolInfo );
            youtubeDlProcess.EnableRaisingEvents = true;
            reChatToolProcess.EnableRaisingEvents = true;
            youtubeDlProcess.BeginOutputReadLine ();
            reChatToolProcess.BeginOutputReadLine ();
            youtubeDlProcess.Exited += new EventHandler ( VideoExit );
            reChatToolProcess.Exited += new EventHandler ( ChatExit );

            youtubeDlProcess.OutputDataReceived += new DataReceivedEventHandler ( VideoDataRecieved );
            reChatToolProcess.OutputDataReceived += new DataReceivedEventHandler ( ChatDataRecieved );
        }

        public static string GetIllegalChars ( string path ) {
            string illegalChars = "";

            foreach ( char c in ILLEGAL_PATH_CHARS ) {
                string cString = c.ToString ();

                if ( !path.Contains ( cString ) )
                    continue;

                illegalChars += cString;
            }

            return illegalChars;
        }

        public static void KillDownloads () {
            bool downloadFinished = !IsDownloadingChat && !IsDownloadingVideo;

            if ( IsDownloadingVideo && !youtubeDlProcess.HasExited )
                youtubeDlProcess.Kill ();

            if ( IsDownloadingChat && !reChatToolProcess.HasExited )
                reChatToolProcess.Kill ();

            while ( !downloadFinished && Directory.Exists ( currentDirectory ) ) {
                if ( IsDownloading )
                    continue;

                try {
                    Directory.Delete ( currentDirectory, true );
                } catch {

                }
            }
        }

        public static float TotalDownloadProgress () {
            float total = Math.Max ( 0f, 100f / 200f * ( VideoDownloadProgress + ChatDownloadProgress ) );
            return total;
        }

        private static void VideoDataRecieved ( object sender, DataReceivedEventArgs e ) {
            if ( string.IsNullOrWhiteSpace ( e.Data ) )
                return;

            string data = e.Data.Replace ( " ", "" );
            Regex r = new Regex ( "](.+?)%" );
            string match = r.Match ( data ).Groups[ 1 ].Value;
            float progress = 0.0f;
            float.TryParse ( match, out progress );
            VideoDownloadProgress = progress;

            if ( OnDownloadProgress != null )
                OnDownloadProgress ();
        }

        private static void VideoExit ( object sender, EventArgs e ) {
            Console.WriteLine ( "Video Exit" );
            youtubeDlProcess.Dispose ();
            youtubeDlProcess = null;
            CheckDownloadFinished ();

            if ( OnDownloadProgress != null )
                OnDownloadProgress ();
        }
    }
}
