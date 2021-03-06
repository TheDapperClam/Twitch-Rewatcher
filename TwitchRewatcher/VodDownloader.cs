﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TwitchRewatcher {
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
        private static Process youtubeDlUpdateProcess = null;
        private static Process reChatToolProcess = null;

        public delegate void VodEventHandler ();
        public static event VodEventHandler OnDownloadFinished;
        public static event VodEventHandler OnDownloadProgress;
        public static event VodEventHandler OnYoutubeDLUpdated;

        private static void ChatDataRecieved ( object sender, DataReceivedEventArgs e ) {
            OnDownloadProgress?.Invoke ();
        }

        private static void ChatExit ( object sender, EventArgs e ) {
            ChatDownloadProgress = 100.0f;
            reChatToolProcess.Dispose ();
            reChatToolProcess = null;
            CheckDownloadFinished ();
            OnDownloadProgress?.Invoke ();
        }

        private static void CheckDownloadFinished () {
            if ( IsDownloading )
                return;

            VideoDownloadProgress = 0.0f;
            ChatDownloadProgress = 0.0f;
            OnDownloadFinished?.Invoke ();
        }

        public static void Download ( string title, string source, string output = null ) {
            string id = Regex.Match ( source, @"videos/([0-9]+?)\b" ).Groups[ 1 ].Value;
            string baseDirectory = Path.Combine ( output, title );
            System.Diagnostics.Debug.WriteLine ( baseDirectory + " " + source );
            VideoDownloadProgress = 0.0f;
            ChatDownloadProgress = 0.0f;
            Directory.CreateDirectory ( baseDirectory );

            ProcessStartInfo youtubeDlInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, YOUTUBE_DL_PATH ) );
            ProcessStartInfo rechatToolInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, RECHAT_TOOL_PATH ) );
            youtubeDlInfo.UseShellExecute = false;
            rechatToolInfo.UseShellExecute = false;
            youtubeDlInfo.RedirectStandardOutput = true;
            rechatToolInfo.RedirectStandardOutput = true;
            youtubeDlInfo.CreateNoWindow = true;
            rechatToolInfo.CreateNoWindow = true;
            rechatToolInfo.WindowStyle = ProcessWindowStyle.Hidden;
            youtubeDlInfo.Arguments = string.Format ( "--newline -o \"{0}\\video.%(ext)s\" \"{1}\"", baseDirectory, source );
            rechatToolInfo.Arguments = string.Format ( "-d {0} \"{1}\"", id, baseDirectory + "/chat.dtc" );

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
        }

        public static float TotalDownloadProgress () {
            float total = Math.Max ( 0f, 100f / 200f * ( VideoDownloadProgress + ChatDownloadProgress ) );
            return total;
        }

        public static void UpdateYoutubeDL () {
            ProcessStartInfo youtubeDlUpdateInfo = new ProcessStartInfo ( Path.Combine ( Environment.CurrentDirectory, YOUTUBE_DL_PATH ) );
            youtubeDlUpdateInfo.UseShellExecute = false;
            youtubeDlUpdateInfo.CreateNoWindow = true;
            youtubeDlUpdateInfo.Arguments = "-U";
            youtubeDlUpdateProcess = Process.Start ( youtubeDlUpdateInfo );
            youtubeDlUpdateProcess.EnableRaisingEvents = true;
            youtubeDlUpdateProcess.Exited += new EventHandler ( YoutubeDLUpdateFinished );
        }

        private static void VideoDataRecieved ( object sender, DataReceivedEventArgs e ) {
            if ( string.IsNullOrWhiteSpace ( e.Data ) )
                return;

            string data = e.Data.Replace ( " ", "" );
            Regex r = new Regex ( "](.+?)%" );
            string match = r.Match ( data ).Groups[ 1 ].Value;
            float.TryParse ( match, out float progress );
            VideoDownloadProgress = progress;
            OnDownloadProgress?.Invoke ();
        }

        private static void VideoExit ( object sender, EventArgs e ) {
            youtubeDlProcess.Dispose ();
            youtubeDlProcess = null;
            CheckDownloadFinished ();
            OnDownloadProgress?.Invoke ();
        }

        private static void YoutubeDLUpdateFinished ( object sender, EventArgs e ) {
            youtubeDlUpdateProcess.Dispose ();
            youtubeDlUpdateProcess = null;
            OnYoutubeDLUpdated?.Invoke ();
        }
    }
}
